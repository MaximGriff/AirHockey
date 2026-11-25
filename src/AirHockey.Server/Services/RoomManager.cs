using AirHockey.Shared.Enums;
using AirHockey.Shared.Models;
using System.Collections.Concurrent;

namespace AirHockey.Server.Services;

public class RoomManager
{
    private readonly ConcurrentDictionary<string, Room> _rooms = new();

    public RoomInfo CreateRoom(string name, GameMode mode)
    {
        var room = new Room
        {
            Id = Guid.NewGuid().ToString("N")[..8],
            Name = name,
            Mode = mode
        };

        _rooms.TryAdd(room.Id, room);

        return room.ToRoomInfo();
    }

    public IReadOnlyList<RoomInfo> GetAvailableRooms()
    {
        return _rooms.Values
            .Where(r => !r.IsStarted && r.Players.Count < 2)
            .Select(r => r.ToRoomInfo())
            .ToList();
    }

    // Внутренний класс комнаты 
    private class Room
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public GameMode Mode { get; set; }
        public bool IsStarted { get; set; }
        public List<PlayerInfo> Players { get; set; } = new();

        public RoomInfo ToRoomInfo() => new()
        {
            Id = Id,
            Name = Name,
            Mode = Mode,
            PlayersCount = Players.Count,
            MaxPlayers = 2,
            IsStarted = IsStarted
        };
    }
}