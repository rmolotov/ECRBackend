using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Kubernetes;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile($"Ocelot.{builder.Environment.EnvironmentName}.json", true, true);

// Application
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    });

// Auth
builder.Services
    .AddAuthentication(config =>
    {
        config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("IdentityUri");
        options.Audience = "ECR.Web";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

// Ocelot
builder.Services
    .AddOcelot()
    // .AddKubernetes()
    // .AddCacheManager(config => config.WithDictionaryHandle())
    ;

var app = builder.Build();

app.UseRouting()
    .UseHttpsRedirection()
    .UseCors("AllowAll");

await app.UseOcelot();


Console.WriteLine($"[DEBUG] Ocelot running in {app.Environment.EnvironmentName} environment");

app.Run();