using Asp.Versioning;
using Microsoft.Extensions.Options;
using RemoteConfig.Api;
using RemoteConfig.Api.Services;
using RemoteConfig.Application.Common.Mappings;
using RemoteConfig.Application.DI;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Persistence.Caching.Providers;
using RemoteConfig.Persistence.DI;
using Shared.Api.Authentication;
using Shared.Api.Endpoints;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Application
builder.Services
    .AddAutoMapper(profile =>
    {
        profile.AddProfile(new HeroMappingProfile());
        profile.AddProfile(new EnemyMappingProfile());
        profile.AddProfile(new InventoryItemMappingProfile());
        profile.AddProfile(new StageMappingProfile());
    })
    .AddApplication()
    .AddPersistence(builder.Configuration)
    .AddCaching(config =>
    {
        config.AddProvider<MemoryCacheProvider>();
        config.AddProvider<RedisCacheProvider>();
    })
    .AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    })
    .AddControllers();

//Versioning and Swagger
builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services
    .AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>()
    .AddSwaggerGen();

// Auth
var identitySettings = builder.Configuration.GetIdentitySettings();
builder.Services.AddCustomAuthentication(identitySettings);

builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy("ApiScopePolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "ECR.Web");
    });

// Custom services
builder.Services
    .AddSingleton<ICurrentUserService, CurrentUserService>()
    .AddHttpContextAccessor();

builder.Services
    .AddHealthChecks();

var app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI(config =>
    {
        config.RoutePrefix = string.Empty;
        foreach (var description in app.DescribeApiVersions())
            config.SwaggerEndpoint(
                $"swagger/{description.GroupName}/swagger.json",
                $"{description.ApiVersion}"
            );
    })
    .UseRouting()
    .UseHttpsRedirection()
    .UseCors("AllowAll")
    .UseAuthentication()
    .UseAuthorization();

app
    .MapControllers();

app.UseDiagnosticEndpoints();

app.Run();