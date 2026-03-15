namespace Hive.Core.Models
{
    public class SpiderPiece(PlayerColor color) : IPiece
    {
        public PlayerColor Color { get; } = color;

    }
}
