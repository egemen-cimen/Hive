using Hive.Core.Models;

namespace Hive.PlayerAgent
{
    public interface IPlayerAgent
    {
        public IPlayerAction SuggestNextPlayerAction(GameState gameState);
    }
}
