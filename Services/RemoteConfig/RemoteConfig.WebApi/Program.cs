using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RemoteConfig.Application.Common.Mappings;
using RemoteConfig.Application.DI;
using RemoteConfig.Application.Interfaces;
using RemoteConfig.Persistence.Caching.Providers;
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

app.Run();