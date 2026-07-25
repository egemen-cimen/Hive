namespace Hive.Core.Models
{
    public class GameState(ICoordinateSystem coordinateSystem,
        Stack<IPlayerAction> pastPlayerActions,
        PlayerColor currentPlayerTurn,
        int turnNumber
        )
    {
        public ICoordinateSystem CoordinateSystem { get; } = coordinateSystem;
        public Stack<IPlayerAction> PastPlayerActions { get; } = pastPlayerActions;
        public PlayerColor CurrentPlayerTurnColor { get; } = currentPlayerTurn;
        public int TurnNumber { get; } = turnNumber;
    }
}
