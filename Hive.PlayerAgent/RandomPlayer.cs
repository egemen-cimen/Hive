using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.PlayerAgent
{
    public class RandomPlayer : IPlayerAgent
    {
        private static readonly Random randomNumberGenerator = new();

        public IPlayerAction SuggestNextPlayerAction(GameState gameState)
        {
            var allPossibleActions = GameRules.GetAllAvailablePlayerActions(gameState).ToArray();
            var nextRandomActionIndex = randomNumberGenerator.Next(allPossibleActions.Length);

            return allPossibleActions[nextRandomActionIndex];
        }
    }
}
