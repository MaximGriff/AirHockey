using AirHockey.Server.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Настройки
builder.WebHost.UseUrls("http://0.0.0.0:5050"); // слушаем на всех интерфейсах

builder.Services.AddSignalR();
builder.Services.AddSingleton<RoomManager>();

var app = builder.Build();


app.MapGet("/", () => "AirHockey server is running");


app.Run();


