namespace Hive.Core.Models
{
    public class GameState(ICoordinateSystem coordinateSystem,
        Stack<IPlayerAction> pastPlayerActions,
        PlayerColor currentPlayerTurn,
        int turnNumber
        )
    {
        private PlayerColor _currentPlayerTurnColor = currentPlayerTurn;
        private int _turnNumber = turnNumber;

        public ICoordinateSystem CoordinateSystem { get; } = coordinateSystem;
        public Stack<IPlayerAction> PastPlayerActions { get; } = pastPlayerActions;
        public PlayerColor CurrentPlayerTurnColor => _currentPlayerTurnColor;
        public int TurnNumber => _turnNumber;

        public void IncrementTurnCounter()
        {
            _currentPlayerTurnColor = CurrentPlayerTurnColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;

            if (_currentPlayerTurnColor == PlayerColor.WHITE)
            {
                _turnNumber++;
            }
        }

        public void DecrementTurnCounter()
        {
            _currentPlayerTurnColor = CurrentPlayerTurnColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;

            if (_currentPlayerTurnColor == PlayerColor.BLACK)
            {
                _turnNumber--;
            }
        }
    }
}
