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
    int port = 5001;
    Action<ListenOptions> config = options => {
        options.Protocols = HttpProtocols.Http2;
        options.UseHttps();
    };

    if (Environment.GetEnvironmentVariable("AUTOMATEDESIGN_LISTENANYIP") == "YES")
    {
        options.ListenAnyIP(port, config);
    }
    else
    {
        options.ListenLocalhost(5001, config);
    }
});

DatabaseSettings dbSettings = builder.Configuration.GetSection("DatabaseSettings")
    .Get<DatabaseSettings>() ?? new DatabaseSettings();

builder.Services.AddSingleton(new DatabaseConnector(dbSettings))
                .AddScoped<IUserDao, UserDao>()
                .AddScoped<IRegistrationDao, RegistrationDao>()
                .AddScoped<ISessionDao, SessionDao>()
                .AddScoped<IDocumentDao, DocumentDao>();

builder.Services.AddAuthentication("Bearer").AddBearer();
builder.Services.AddAuthorization();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailSender>();

Template.TemplateDirectory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Templates");

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<UsersService>();
app.MapGrpcService<DocumentsService>();

app.Run();
