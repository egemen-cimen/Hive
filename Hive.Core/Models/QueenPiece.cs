namespace Hive.Core.Models
{
    public class QueenPiece(PlayerColor color) : IPiece
    {
        public PlayerColor Color { get; } = color;

    }
}
