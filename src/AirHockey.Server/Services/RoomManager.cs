using AirHockey.Shared.Enums;
using AirHockey.Shared.Models;
using System.Collections.Concurrent;

namespace AirHockey.Server.Services;

public class RoomManager
{
    private readonly ConcurrentDictionary<string, Room> _rooms = new();

    public RoomInfo? CreateRoom(string name, GameMode mode, 
        string connectionId, string nickname)
    {
        var room = new Room
        {
            Id = Guid.NewGuid().ToString("N")[..8],
            Name = string.IsNullOrWhiteSpace(name) ? "Room" : name.Trim(),
            Mode = mode
        };

        // Добавляем создателя
        var player = new PlayerInfo
        {
            ConnectionId = connectionId,
            Nickname = string.IsNullOrWhiteSpace(nickname) ? "Player" : nickname.Trim(),
            IsBot = false,
            Side = 0
        };

        room.Players.Add(player);

        // Если режим против бота — сразу добавляем бота
        if (mode == GameMode.PlayerVsBot)
        {
            room.Players.Add(new PlayerInfo
            {
                ConnectionId = "bot",
                Nickname = "Bot",
                IsBot = true,
                Side = 1
            });
        }

        _rooms.TryAdd(room.Id, room);

        return room.ToRoomInfo();
    }



    //public IReadOnlyList<RoomInfo> GetAvailableRooms()
    //{
    //    return _rooms.Values
    //        .Where(r => !r.IsStarted && r.Players.Count < 2)
    //        .Select(r => r.ToRoomInfo())
    //        .ToList();
    //}

    public RoomInfo? JoinRoom(string roomId, string connectionId, string nickname)
    {
        if (!_rooms.TryGetValue(roomId, out var room))
            return null;

        if (room.IsStarted || room.Players.Count >= 2)
            return null;

        // Не даём одному человеку зайти дважды
        if (room.Players.Any(p => p.ConnectionId == connectionId))
            return room.ToRoomInfo();

        var player = new PlayerInfo
        {
            ConnectionId = connectionId,
            Nickname = string.IsNullOrWhiteSpace(nickname) ? "Player" : nickname.Trim(),
            IsBot = false,
            Side = room.Players.Count // 0 или 1
        };

        room.Players.Add(player);
        return room.ToRoomInfo();
    }

    public bool LeaveRoom(string connectionId)
    {
        var room = _rooms.Values.FirstOrDefault(r => r.Players.Any(p => p.ConnectionId == connectionId));
        if (room is null) return false;

        room.Players.RemoveAll(p => p.ConnectionId == connectionId);

        // Если комната пустая — удаляем
        if (room.Players.Count == 0 || room.Players.All(p => p.IsBot))
        {
            _rooms.TryRemove(room.Id, out _);
        }

        return true;
    }

    public IReadOnlyList<RoomInfo> GetAvailableRooms()
    {
        return _rooms.Values
            .Where(r => !r.IsStarted && r.Players.Count(p => !p.IsBot) < 2)
            .Select(r => r.ToRoomInfo())
            .ToList();
    }

    public Room? GetRoomByConnection(string connectionId)
    {
        return _rooms.Values.FirstOrDefault(r => r.Players.Any(p => p.ConnectionId == connectionId));
    }


    public Room? GetRoom(string roomId)
    {
        _rooms.TryGetValue(roomId, out var room);
        return room;
    }

    // Внутренний класс комнаты 
    public class Room
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
            PlayersCount = Players.Count(p => !p.IsBot),
            MaxPlayers = 2,
            IsStarted = IsStarted
        };
    }
}