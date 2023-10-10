using AutomateDesign.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

DatabaseSettings dbSettings = builder.Configuration.GetSection("DatabaseSettings")
    .Get<DatabaseSettings>() ?? new DatabaseSettings();

builder.Services.AddSingleton(new DatabaseConnector(dbSettings));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
