namespace Hive.Core.Models
{
    public class PlayerSpawnAction(IPiece pieceToSpawn, (int column, int row) destinationCoordinate) : IPlayerAction
    {
        public IPiece PieceToSpawn { get; } = pieceToSpawn;
        public (int column, int row) DestinationCoordinate { get; } = destinationCoordinate;
    }
}
