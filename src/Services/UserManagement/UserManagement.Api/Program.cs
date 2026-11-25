using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Shared.Api.Authentication;
using Shared.Api.Endpoints;
using UserManagement.Core.Entities;
using UserManagement.Persistence.Database;

using static Shared.Api.Constants.Authentication;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddDbContext<UsersDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("UsersConnection"));
        });

    builder.Services
        .AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<UsersDbContext>()
        .AddDefaultTokenProviders();

// Auth
    var identitySettings = builder.Configuration.GetIdentitySettings();
    builder.Services.AddCustomAuthentication(identitySettings);

    builder.Services
        .AddAuthorizationBuilder()
        .AddPolicy(AuthorizationPolicies.ApiScopePolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", AuthorizationPolicies.ApiScope);
        })
        .AddPolicy(AuthorizationPolicies.UserScopePolicy, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", AuthorizationPolicies.UserScope);
        });

    builder.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddControllers();

    builder.Services.AddHealthChecks();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        if (context.Database.GetPendingMigrations().Any())
            context.Database.Migrate();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        IdentityModelEventSource.ShowPII = true;
    }

    app
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization();

    app.MapControllers();

    app.UseDiagnosticEndpoints();

    app.Run();
}
catch (Exception exception)
{
    Log.Error(exception.Message);
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}
