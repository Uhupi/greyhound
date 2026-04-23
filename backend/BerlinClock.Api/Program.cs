using BerlinClock.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();
app.UseCors();

app.MapGet("/time", () => BerlinClockService.GetState(DateTime.Now));

app.Run("http://localhost:5050");
