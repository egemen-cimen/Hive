namespace Hive.Core.Models
{
    public class Hexagon
    {
        private Stack<IPiece> Pieces { get; } = new Stack<IPiece>();

        public void PushPiece(IPiece piece) => Pieces.Push(piece);

        public IPiece PopPiece() => Pieces.Pop();

        public PlayerColor GetColor() => Pieces.Peek().Color;

        public int GetPieceCount() => Pieces.Count;

        public IEnumerable<IPiece> GetAllPieces() => Pieces.Select(p => p);
    }
}