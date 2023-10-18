using AutomateDesign.Server.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using AutomateDesign.Server.Data.MariaDb;
using AutomateDesign.Server.Data;
using AutomateDesign.Server.Data.MariaDb.Implementations;
using AutomateDesign.Server.Model;
using EmailSender = AutomateDesign.Server.Model.EmailSender;
using AutomateDesign.Server.Middleware.Authentication;

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

builder.Services.AddAuthentication("Bearer").AddBearer();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailSender>();

Template.TemplateDirectory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Templates");

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseAuthentication();

app.MapGrpcService<UsersService>();

app.Run();
