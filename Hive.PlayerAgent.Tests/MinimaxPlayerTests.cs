using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.PlayerAgent.Tests
{
    [TestClass]
    public class MinimaxPlayerTests
    {
        [TestMethod]
        public void Given_GameStateWhereWhiteCanWinInOneMove_When_SuggestedNextMoveRetrieved_Then_ReturnsWinningMove()
        {
            // GIVEN
            var minimaxPlayer = new MinimaxPlayer();
            var gameState = GameRules.CreateFreshGameState();
            // Spawn first pieces in a line
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(SpiderPiece) && a.DestinationCoordinate.column == 0);
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(SpiderPiece) && a.DestinationCoordinate.column == 0);
            // Spawn queens as second pieces in a line and get the coordinates
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(QueenPiece) && a.DestinationCoordinate.column == 0);
            var queenSpawnAction = ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(QueenPiece) && a.DestinationCoordinate.column == 0);
            var spacesNextToQueen = gameState.CoordinateSystem.GetAdjacentCoordinates(queenSpawnAction.DestinationCoordinate);
            // Don't surround white queen & also save ants for the finishing move
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() != typeof(AntPiece) && a.DestinationCoordinate.column == 0);
            // Finally spawn next 4 pieces near black queen to surround it
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => spacesNextToQueen.Contains(a.DestinationCoordinate));
            for (var i = 0; i < 3; i++)
            {
                // Don't surround white queen & also spawn the ants
                ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(AntPiece) && a.DestinationCoordinate.column == 0);

                // Continue surrounding the black queen
                ApplyPlayerSpawnActionPredicateToGameState(gameState, a => spacesNextToQueen.Contains(a.DestinationCoordinate));
            }

            // WHEN
            var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);

            // THEN
            // TODO: check if the game state is not modified after the method call
            Assert.IsInstanceOfType<PlayerMovementAction>(suggestedAction);
            Assert.Contains(((PlayerMovementAction)suggestedAction).DestinationCoordinate, spacesNextToQueen);
            GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);
            Assert.IsTrue(GameRules.VerifyWhetherGameStateIsTerminal(gameState));
            Assert.AreEqual(GameResult.WHITE_WON, GameRules.GetGameResult(gameState));
        }

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
            // Spawn next 4 pieces near white queen to surround it (9 pieces in total)
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
            // TODO: check if the game state is not modified after the method call
            Assert.IsInstanceOfType<PlayerMovementAction>(suggestedAction);
            Assert.Contains(((PlayerMovementAction)suggestedAction).DestinationCoordinate, spacesNextToQueen);
            GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);
            Assert.IsTrue(GameRules.VerifyWhetherGameStateIsTerminal(gameState));
            Assert.AreEqual(GameResult.BLACK_WON, GameRules.GetGameResult(gameState));
        }


        private static PlayerSpawnAction ApplyPlayerSpawnActionPredicateToGameState(GameState gameState, Func<PlayerSpawnAction, bool> spawnPredicate)
        {
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(spawnPredicate);
            GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);

            return actionToBeApplied;
        }
    }
}
