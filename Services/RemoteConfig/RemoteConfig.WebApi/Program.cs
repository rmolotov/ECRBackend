using RemoteConfig.Application.DI;
using RemoteConfig.Persistence.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAutoMapper(config =>
    {
        config.AddProfile(typeof(Program));
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

var app = builder.Build();

app
    .MapControllers();

app.Run();