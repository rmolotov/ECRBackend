using Asp.Versioning;
using Microsoft.Extensions.Options;
using RemoteConfig.Application.Common.Mappings;
using RemoteConfig.Application.DI;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Persistence.DI;
using RemoteConfig.WebApi;
using RemoteConfig.WebApi.Services;
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

// Logging
builder.Services
    .AddSingleton<ICurrentUserService, CurrentUserService>()
    .AddHttpContextAccessor();

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
    .UseCors("AllowAll");

app
    .MapControllers();

app.Run();