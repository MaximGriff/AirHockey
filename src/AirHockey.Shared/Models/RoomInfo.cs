
using AirHockey.Shared.Enums;

namespace AirHockey.Shared.Models
{
    public class RoomInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = "Room";
        public GameMode Mode { get; set; }
        public int PlayersCount { get; set; }
        public int MaxPlayers { get; set; } = 2;
        public bool IsStarted { get; set; }
    }
}
