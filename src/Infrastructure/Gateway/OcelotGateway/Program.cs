using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shared.Api.Authentication;

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
var identitySettings = builder.Configuration.GetIdentitySettings();
builder.Services.AddCustomAuthentication(identitySettings);
    

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