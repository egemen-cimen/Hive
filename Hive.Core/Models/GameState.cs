namespace Hive.Core.Models
{
    public class GameState(ICoordinateSystem coordinateSystem, int turnNumber, PlayerColor playerTurn)
    {
        private readonly ICoordinateSystem _coordinateSystem = coordinateSystem;
        private int turnNumber = turnNumber;
        private PlayerColor playerTurn = playerTurn;

        public bool VerifyWhetherGameStateIsTerminal()
        {
            throw new NotImplementedException();
        }

        public int EvaluateValueOfCurrentGameState()
        {
            throw new NotImplementedException();
        }

        public PlayerColor GetPlayerTurn()
        {
            throw new NotImplementedException();
        }

        public List<IPlayerAction> GetAvailableActionsToPlayer()
        {
            throw new NotImplementedException();
        }

        public GameState ApplyPlayerActionToCurrentGameState(IPlayerAction playerAction)
        {
            throw new NotImplementedException();
        }
    }
}
