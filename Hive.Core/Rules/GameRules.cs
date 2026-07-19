using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class GameRules
    {
        /// <summary>
        /// Returns all valid actions from a given game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<IPlayerAction> GetAllAvailableActions(GameState gameState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks the given player action against the game state to determine if the action is valid.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerAction"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool VerifyWhetherActionIsLegal(GameState gameState, IPlayerAction playerAction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the result of the game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public GameResult GetGameResult(GameState gameState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether the game has concluded or is still ongoing.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool VerifyWhetherGameStateIsTerminal(GameState gameState)
        {
            //var gameResult = GetGameResult(gameState);
            //return gameResult == GameResult.ONGOING;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies the player action to the given game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerAction"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public GameState ApplyPlayerActionToGameState(GameState gameState, IPlayerAction playerAction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns player's color for the current turn.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public PlayerColor GetPlayerTurn(GameState gameState)
        {
            throw new NotImplementedException();
        }
    }
}
