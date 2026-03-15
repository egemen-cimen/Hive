namespace Hive.Core.Models
{
    public class BeetlePiece(PlayerColor color) : IPiece
    {
        public PlayerColor Color { get; } = color;

    }
}
