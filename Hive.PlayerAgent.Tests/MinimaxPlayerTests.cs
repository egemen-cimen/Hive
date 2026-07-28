using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.PlayerAgent.Tests
{
    [TestClass]
    public class MinimaxPlayerTests
    {
        // TODO: use game rules to generate game states. also write a test helper for this

        [TestMethod]
        public void Given_GameStateWhereBlackCanWinInOneMove_When_SuggestedNextMoveRetrieved_Then_ReturnsWinningMove()
        {
            // GIVEN
            var minimaxPlayer = new MinimaxPlayer();
            var gameState = GameRules.CreateFreshGameState();
            // Spawn first pieces in a line
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(SpiderPiece) && a.DestinationCoordinate.column == 0);
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(SpiderPiece) && a.DestinationCoordinate.column == 0);
            // Spawn queens as second pieces in a line and get the coordinates
            var queenSpawnAction = ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(QueenPiece) && a.DestinationCoordinate.column == 0);
            var spacesNextToQueen = gameState.CoordinateSystem.GetAdjacentCoordinates(queenSpawnAction.DestinationCoordinate);
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(QueenPiece) && a.DestinationCoordinate.column == 0);
            // Finally spawn next 5 pieces near white queen to surround it (9 pieces in total)
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => spacesNextToQueen.Contains(a.DestinationCoordinate));
            // Don't surround black queen & also save ants for the finishing move
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() != typeof(AntPiece) && a.DestinationCoordinate.column == 0);
            for (var i = 0; i < 3; i++)
            {
                // Continue surrounding the white queen
                ApplyPlayerSpawnActionPredicateToGameState(gameState, a => spacesNextToQueen.Contains(a.DestinationCoordinate));

                // Don't surround black queen & also spawn the ants
                ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(AntPiece) && a.DestinationCoordinate.column == 0);
            }

            // Don't spawn a piece to the last free space next to the queen
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => !spacesNextToQueen.Contains(a.DestinationCoordinate));

            // WHEN
            var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);

            // THEN
            Assert.IsInstanceOfType<PlayerMovementAction>(suggestedAction);
            Assert.Contains(((PlayerMovementAction)suggestedAction).DestinationCoordinate, spacesNextToQueen);
            GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);
        }


        private static PlayerSpawnAction ApplyPlayerSpawnActionPredicateToGameState(GameState gameState, Func<PlayerSpawnAction, bool> spawnPredicate)
        {
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(spawnPredicate);
            GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);

            return actionToBeApplied;
        }
    }
}
