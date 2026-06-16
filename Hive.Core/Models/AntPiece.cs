namespace Hive.Core.Models
{
    public class AntPiece(PlayerColor color) : IPiece
    {
        public PlayerColor Color { get; } = color;
        public static string Name => "Ant";
        public string GetPieceName() => Name;
    }
}
