namespace Hive.Core.Models
{
    public class PlayerMovementAction((int column, int row) startCoordinate,
        (int column, int row) destinationCoordinate
        ) : IPlayerAction
    {
        public (int column, int row) StartCoordinate { get; } = startCoordinate;
        public (int column, int row) DestinationCoordinate { get; } = destinationCoordinate;
    }
}
