using AutomateDesign.Server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.MapGrpcService<UsersService>();

app.Run();
