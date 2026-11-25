

namespace AirHockey.Shared.Models
{
    public class PlayerInfo
    {
        public string ConnectionId { get; set; } = String.Empty;
        public string Nickname { get; set; } = "Player";
        public bool IsBot { get; set; }
        public int Side { get; set; } // 0 - левая сторона, 1 - правая сторона

    }
}
