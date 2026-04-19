namespace Hive.Core.Models
{
    public class QueenPiece(PlayerColor color) : IPiece
    {
        public PlayerColor Color { get; } = color;
        public static string Name => "Queen";

        public string GetPieceName()
        {
            return Name;
        }
    }
}
