namespace Hive.Core.Models
{
    public class AntPiece(PlayerColor color) : IPiece
    {
        public PlayerColor Color { get; } = color;
    }
}
