using AutomateDesign.Server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using AutomateDesign.Server.Data.MariaDb;
using AutomateDesign.Server.Data;
using AutomateDesign.Server.Data.MariaDb.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001, options => {
        options.Protocols = HttpProtocols.Http2;
        options.UseHttps();
    });
});

DatabaseSettings dbSettings = builder.Configuration.GetSection("DatabaseSettings")
    .Get<DatabaseSettings>() ?? new DatabaseSettings();

builder.Services.AddSingleton(new DatabaseConnector(dbSettings))
                .AddScoped<IUserDao, UserDao>()
                .AddScoped<IRegistrationDao, RegistrationDao>()
                .AddScoped<ISessionDao, SessionDao>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.MapGrpcService<UsersService>();

app.Run();
