using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.Core.Tests.Rules
{
    [TestClass]
    public class GameRulesTests
    {
        [TestMethod]
        public void Given_GameRules_When_FreshGameStateCreated_Then_ReturnsNewFreshGameState()
        {
            // WHEN
            var freshGameState = GameRules.CreateFreshGameState();

            // THEN
            Assert.AreEqual(PlayerColor.WHITE, freshGameState.CurrentPlayerTurnColor);
            Assert.AreEqual(1, freshGameState.TurnNumber);
            Assert.IsEmpty(freshGameState.CoordinateSystem.GetAllCoordinates());
        }

        [TestMethod]
        public void Given_FreshGameState_When_AllAvailableActionsRetrieved_Then_ReturnsAllPossibleSpawnActions()
        {
            // GIVEN
            var freshGameState = GameRules.CreateFreshGameState();

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(freshGameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(availableActions, typeof(PlayerSpawnAction));
            Assert.HasCount(4, availableActions);
            foreach (var action in availableActions.Cast<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.WHITE, action.PieceToSpawn.Color);
                Assert.IsNotInstanceOfType<QueenPiece>(action.PieceToSpawn);
            }
        }

        [TestMethod]
        public void Given_GameStateWithFirstPlayerAction_When_AllAvailableActionsRetrieved_Then_ReturnsAllPossibleSpawnActions()
        {
            // GIVEN
            var freshGameState = GameRules.CreateFreshGameState();
            var appliedAction = GameRules.GetAllAvailablePlayerActions(freshGameState).Cast<PlayerSpawnAction>().First();
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(freshGameState, appliedAction);

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(updatedGameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(availableActions, typeof(PlayerSpawnAction));
            Assert.HasCount(24, availableActions);
            foreach (var action in availableActions.Cast<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.BLACK, action.PieceToSpawn.Color);
                Assert.IsNotInstanceOfType<QueenPiece>(action.PieceToSpawn);
            }
        }

        [TestMethod]
        public void Given_GameStateWithTwoSpawnedPieces_When_AllAvailableActionsRetrieved_Then_ReturnsAllPossibleSpawnActions()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn the first piece
            var appliedAction = GameRules.GetAllAvailablePlayerActions(gameState).Cast<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, appliedAction);
            // Spawn the second piece
            appliedAction = GameRules.GetAllAvailablePlayerActions(gameState).Cast<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, appliedAction);

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(gameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(availableActions, typeof(PlayerSpawnAction));
            Assert.HasCount(15, availableActions);
            foreach (var action in availableActions.Cast<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.WHITE, action.PieceToSpawn.Color);
            }
        }

        [TestMethod]
        public void Given_GameStateWithAllPiecesSpawned_When_AllAvailableActionsRetrieved_Then_ReturnsOnlyMovementActions()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn the all 22 pieces
            for (var i = 0; i < 22; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).First(a => a.GetType() == typeof(PlayerSpawnAction));
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(gameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(availableActions, typeof(PlayerMovementAction));
            Assert.AreEqual(0, availableActions.Count(a => a.GetType() == typeof(PlayerSpawnAction)));
        }

        [TestMethod]
        public void Given_ReveredGameState_When_AllAvailableActionsRetrieved_Then_ReturnsAllPossibleSpawnActions()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn the first piece
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).Cast<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            // Spawn the second piece
            actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).Cast<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            // Spawn the third piece
            actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).Cast<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            // Record count of available actions for this state (should be 15)
            var firstAvailableActions = GameRules.GetAllAvailablePlayerActions(gameState);
            var availableActionsCount = firstAvailableActions.Count;
            // Undo the last move
            gameState = GameRules.UndoLastMoveFromGameState(gameState);
            // Spawn the third piece again
            actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).Cast<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);

            // WHEN
            var secondAvailableActions = GameRules.GetAllAvailablePlayerActions(gameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(firstAvailableActions, typeof(PlayerSpawnAction));
            CollectionAssert.AllItemsAreInstancesOfType(secondAvailableActions, typeof(PlayerSpawnAction));
            Assert.HasCount(firstAvailableActions.Count, secondAvailableActions);
            Assert.HasCount(15, secondAvailableActions);
            foreach (var action in firstAvailableActions.Cast<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.BLACK, action.PieceToSpawn.Color);
            }

            foreach (var action in secondAvailableActions.Cast<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.BLACK, action.PieceToSpawn.Color);
            }
        }

        [TestMethod]
        public void Given_FreshGameState_When_FirstPlayerActionApplied_Then_ReturnsUpdatedGameState()
        {
            // GIVEN
            var freshGameState = GameRules.CreateFreshGameState();
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(freshGameState).Cast<PlayerSpawnAction>().First();

            // WHEN
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(freshGameState, actionToBeApplied);

            // THEN
            var allCoordinatesFromUpdatedGameState = updatedGameState.CoordinateSystem.GetAllCoordinates();
            Assert.HasCount(1, allCoordinatesFromUpdatedGameState);
            updatedGameState.CoordinateSystem.TryGetHexagon(allCoordinatesFromUpdatedGameState.First(), out var onlyHexagon);
            Assert.IsNotNull(onlyHexagon);
            Assert.AreEqual(actionToBeApplied.PieceToSpawn.GetType(), onlyHexagon.PeekPiece().GetType());
        }

        [TestMethod]
        public void Given_GameStateWithFirstPlayerAction_When_PlayerActionReverted_Then_ReturnsFreshGameState()
        {
            // GIVEN
            var freshGameState = GameRules.CreateFreshGameState();
            var appliedAction = GameRules.GetAllAvailablePlayerActions(freshGameState).Cast<PlayerSpawnAction>().First();
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(freshGameState, appliedAction);

            // WHEN
            var revertedGameState = GameRules.UndoLastMoveFromGameState(updatedGameState);

            // THEN
            Assert.AreEqual(PlayerColor.WHITE, revertedGameState.CurrentPlayerTurnColor);
            Assert.AreEqual(1, revertedGameState.TurnNumber);
            Assert.IsEmpty(revertedGameState.CoordinateSystem.GetAllCoordinates());
        }

        [TestMethod]
        public void Given_GameStateInProgress_When_InvalidSpawnActionIsAttempted_Then_ThrowsException()
        {
            // GIVEN
            var freshGameState = GameRules.CreateFreshGameState();
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(freshGameState).Cast<PlayerSpawnAction>().First();
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(freshGameState, actionToBeApplied);

            // WHEN & THEN
            Assert.Throws<Exception>(() => GameRules.ApplyPlayerActionToGameState(updatedGameState, actionToBeApplied));
        }
    }
}
