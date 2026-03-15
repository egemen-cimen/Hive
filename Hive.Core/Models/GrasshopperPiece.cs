namespace Hive.Core.Models
{
    public class GrasshopperPiece(PlayerColor color) : IPiece
    {
        public PlayerColor Color { get; } = color;

    }
}
