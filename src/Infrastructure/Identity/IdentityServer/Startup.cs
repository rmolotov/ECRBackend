// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using System.Reflection;
using System.Text;
using IdentityServer;
using IdentityServer.Configuration;
using IdentityServer.Services.CorsPolicy;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using UserManagement.Core.Entities;
using UserManagement.Persistence.Database;

namespace Identity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public IdentityServerConfiguration IdentityServerConfiguration { get; private set; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityServerConfiguration = Configuration
                .GetSection(IdentityServerConfiguration.WellKnownName)
                .Get<IdentityServerConfiguration>();
            services.Configure<IdentityServerConfiguration>(Configuration.GetSection(IdentityServerConfiguration.WellKnownName));
            
            services.AddControllersWithViews();
            
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<UsersDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("UsersConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<UsersDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;
                    options.IssuerUri = IdentityServerConfiguration.IssuerUri;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(
                            Configuration.GetConnectionString("IdentityServer"),
                            sql =>
                            {
                                sql.MigrationsAssembly(migrationsAssembly);
                                sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            })
                        .ConfigureWarnings(warnings =>
                            warnings.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(
                            Configuration.GetConnectionString("IdentityServer"),
                            sql =>
                            {
                                sql.MigrationsAssembly(migrationsAssembly);
                                sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            })
                        .ConfigureWarnings(warnings =>
                            warnings.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
                })
                .AddAspNetIdentity<ApplicationUser>();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IdentityServerConfiguration.IssuerSigningKey));
            
            services.AddSingleton<ISigningCredentialStore>(
                new InMemorySigningCredentialsStore(new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)));
            services.AddSingleton<IValidationKeysStore>(
                new InMemoryValidationKeysStore([
                    new SecurityKeyInfo { Key = signingKey, SigningAlgorithm = SecurityAlgorithms.HmacSha256 }
                ]));

            // Add custom CORS PolicyService and Validator
            services
                .AddTransient<ICorsPolicyService, CustomCorsPolicyService>()
                .AddTransient<IClientConfigurationValidator, CustomClientConfigurationValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            InitializeDatabase(app);
            
            if (Environment.IsDevelopment())
            {
                Log.Information("is development");
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
        
        private void InitializeDatabase(IApplicationBuilder app)
        {
            Log.Information("Database initialization...");

            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            var persistedContext = serviceScope.ServiceProvider
                .GetRequiredService<PersistedGrantDbContext>();

            var configContext = serviceScope.ServiceProvider
                .GetRequiredService<ConfigurationDbContext>();

            persistedContext.Database.Migrate();
            configContext.Database.Migrate();

            // Configuration (appsettings) is the source of truth: configured entries are re-created on every start,
            // so changes of secrets, scopes and resources are applied to already initialized database.
            using var transaction = configContext.Database.BeginTransaction();

            foreach (var client in Config.GetClients(IdentityServerConfiguration))
            {
                configContext.Clients.Where(c => c.ClientId == client.ClientId).ExecuteDelete();
                configContext.Clients.Add(client.ToEntity());
            }

            foreach (var resource in Config.GetIdentityResources(IdentityServerConfiguration))
            {
                configContext.IdentityResources.Where(r => r.Name == resource.Name).ExecuteDelete();
                configContext.IdentityResources.Add(resource.ToEntity());
            }

            foreach (var scope in Config.GetApiScopes(IdentityServerConfiguration))
            {
                configContext.ApiScopes.Where(s => s.Name == scope.Name).ExecuteDelete();
                configContext.ApiScopes.Add(scope.ToEntity());
            }

            foreach (var resource in Config.GetApiResources(IdentityServerConfiguration))
            {
                configContext.ApiResources.Where(r => r.Name == resource.Name).ExecuteDelete();
                configContext.ApiResources.Add(resource.ToEntity());
            }

            configContext.SaveChanges();
            transaction.Commit();

            Log.Information("Database initialized");
        }
    }
}
