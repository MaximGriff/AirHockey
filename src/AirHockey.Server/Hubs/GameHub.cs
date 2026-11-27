using AirHockey.Server.Services;
using AirHockey.Shared.Enums;
using AirHockey.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace AirHockey.Server.Hubs;

public class GameHub : Hub
{
    private readonly RoomManager _roomManager;

    public GameHub(RoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public async Task<RoomInfo> CreateRoom(string roomName, GameMode mode, string nickname)
    {
        var room = _roomManager.CreateRoom(roomName, mode);

        // Пока просто возвращаем информацию о комнате
        // Логику добавления игрока добавим позже

        await Clients.Caller.SendAsync("RoomCreated", room);
        return room;
    }

    public Task<List<RoomInfo>> GetRooms()
    {
        return Task.FromResult(_roomManager.GetAvailableRooms().ToList());
    }
}