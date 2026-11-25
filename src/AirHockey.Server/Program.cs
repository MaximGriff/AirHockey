var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();


app.MapGet("/", () => "AirHockey server is running");


app.Run();


