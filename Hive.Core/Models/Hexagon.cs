#if DEBUG
using System.Diagnostics;
#endif

namespace Hive.Core.Models
{
#if DEBUG
    [DebuggerDisplay("Hexagon with {GetPieceCount()} piece(s): {PeekPiece().Color} {PeekPiece().GetPieceName()}")]
#endif
    public class Hexagon
    {
        private Stack<IPiece> Pieces { get; } = new Stack<IPiece>();

        public void PushPiece(IPiece piece) => Pieces.Push(piece);

        public IPiece PopPiece() => Pieces.Pop();

        public IPiece PeekPiece() => Pieces.Peek();

        public PlayerColor GetColor() => Pieces.Peek().Color;

        public int GetPieceCount() => Pieces.Count;

        public IEnumerable<IPiece> GetAllPieces() => Pieces.Select(p => p);
    }
}