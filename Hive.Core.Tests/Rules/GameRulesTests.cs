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
            foreach(var action in availableActions.Cast<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.WHITE, action.PieceToSpawn.Color);
                Assert.IsNotInstanceOfType<QueenPiece>(action.PieceToSpawn);
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
