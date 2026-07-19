namespace Hive.Core.Models
{
    public class GameState(ICoordinateSystem coordinateSystem, int turnNumber, PlayerColor currentPlayerTurn)
    {
        public ICoordinateSystem CoordinateSystem { get; } = coordinateSystem;
        public int TurnNumber { get; } = turnNumber;
        public PlayerColor CurrentPlayerTurn { get; } = currentPlayerTurn;
    }
}
