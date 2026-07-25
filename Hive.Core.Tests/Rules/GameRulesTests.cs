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
            foreach (var action in availableActions.OfType<PlayerSpawnAction>())
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
            var appliedAction = GameRules.GetAllAvailablePlayerActions(freshGameState).OfType<PlayerSpawnAction>().First();
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(freshGameState, appliedAction);

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(updatedGameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(availableActions, typeof(PlayerSpawnAction));
            Assert.HasCount(24, availableActions);
            foreach (var action in availableActions.OfType<PlayerSpawnAction>())
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
            var appliedAction = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, appliedAction);
            // Spawn the second piece
            appliedAction = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, appliedAction);

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(gameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(availableActions, typeof(PlayerSpawnAction));
            Assert.HasCount(15, availableActions);
            foreach (var action in availableActions.OfType<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.WHITE, action.PieceToSpawn.Color);
            }
        }

        [TestMethod]
        public void Given_GameStateWithAllPiecesSpawned_When_AllAvailableActionsRetrieved_Then_ReturnsOnlyMovementActions()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn all 22 pieces
            for (var i = 0; i < 22; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(gameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(availableActions, typeof(PlayerMovementAction));
            Assert.AreEqual(0, availableActions.OfType<PlayerSpawnAction>().Count());
            Assert.IsGreaterThan(0, availableActions.OfType<PlayerMovementAction>().Count());
        }

        [TestMethod]
        public void Given_ReveredGameState_When_AllAvailableActionsRetrieved_Then_ReturnsAllPossibleSpawnActions()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn the first piece
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            // Spawn the second piece
            actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            // Spawn the third piece
            actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(a
                => a.PieceToSpawn.GetType() != typeof(QueenPiece));
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            // Record count of available actions for this state (should be 15)
            var firstAvailableActions = GameRules.GetAllAvailablePlayerActions(gameState);
            // Undo the last move
            gameState = GameRules.UndoLastMoveFromGameState(gameState);
            // Spawn the third piece again
            actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(a
                => a.PieceToSpawn.GetType() != typeof(QueenPiece));
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);

            // WHEN
            var secondAvailableActions = GameRules.GetAllAvailablePlayerActions(gameState);

            // THEN
            CollectionAssert.AllItemsAreInstancesOfType(firstAvailableActions, typeof(PlayerSpawnAction));
            CollectionAssert.AllItemsAreInstancesOfType(secondAvailableActions, typeof(PlayerSpawnAction));
            Assert.HasCount(firstAvailableActions.Count, secondAvailableActions);
            Assert.HasCount(15, secondAvailableActions);
            foreach (var action in firstAvailableActions.OfType<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.BLACK, action.PieceToSpawn.Color);
            }

            foreach (var action in secondAvailableActions.OfType<PlayerSpawnAction>())
            {
                Assert.AreEqual(PlayerColor.BLACK, action.PieceToSpawn.Color);
            }
        }

        [TestMethod]
        public void Given_GameStateWherePlayerHasNoMoves_When_AllAvailableActionsRetrieved_Then_ReturnsOnlyPlayerUnableToPlayAction()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn all non-ant pieces in a line
            for (var i = 0; i < 16; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(a
                    => a.PieceToSpawn.GetType() != typeof(AntPiece) && a.DestinationCoordinate.column == 0);
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }

            // Spawn rest of the pieces (all ants) in a line
            for (var i = 0; i < 6; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(a
                    => a.DestinationCoordinate.column == 0);
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }

            var antMovementToOtherSideAction = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerMovementAction>().First(a
                => a.DestinationCoordinate.column == 0);
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, antMovementToOtherSideAction);

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(gameState);

            // THEN
            Assert.HasCount(1, availableActions);
            Assert.IsInstanceOfType<PlayerUnableToPlayAction>(availableActions[0]);
        }

        [TestMethod]
        public void Given_FreshGameState_When_FirstPlayerSpawnActionApplied_Then_ReturnsUpdatedGameState()
        {
            // GIVEN
            var freshGameState = GameRules.CreateFreshGameState();
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(freshGameState).OfType<PlayerSpawnAction>().First();

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
        public void Given_GameStateWithFirstPlayerAction_When_PlayerSpawnActionReverted_Then_ReturnsFreshGameState()
        {
            // GIVEN
            var freshGameState = GameRules.CreateFreshGameState();
            var appliedAction = GameRules.GetAllAvailablePlayerActions(freshGameState).OfType<PlayerSpawnAction>().First();
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
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(freshGameState).OfType<PlayerSpawnAction>().First();
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(freshGameState, actionToBeApplied);

            // WHEN & THEN
            Assert.Throws<Exception>(() => GameRules.ApplyPlayerActionToGameState(updatedGameState, actionToBeApplied));
        }

        [TestMethod]
        public void Given_GameStateWithFourPiecesSpawned_When_AllAvailableActionsRetrieved_Then_ReturnsSpawnAndMovementActions()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn 4 pieces including the queen for each player
            for (var i = 0; i < 8; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }

            // WHEN
            var availableActions = GameRules.GetAllAvailablePlayerActions(gameState);

            // THEN
            Assert.IsGreaterThan(0, availableActions.OfType<PlayerSpawnAction>().Count());
            Assert.IsGreaterThan(0, availableActions.OfType<PlayerMovementAction>().Count());
        }

        [TestMethod]
        public void Given_GameStateWithFourPiecesSpawned_When_PlayerMovementActionApplied_Then_ReturnsUpdatedGameState()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn 4 pieces including the queen for each player
            for (var i = 0; i < 8; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }
            var allCoordinatesFromOriginalGameState = gameState.CoordinateSystem.GetAllCoordinates();
            var availableMovementActions = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerMovementAction>().ToList();

            // WHEN
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(gameState, availableMovementActions.First());

            // THEN
            var allCoordinatesFromUpdatedGameState = updatedGameState.CoordinateSystem.GetAllCoordinates();
            Assert.HasCount(8, allCoordinatesFromUpdatedGameState);
            Assert.IsFalse(allCoordinatesFromOriginalGameState.SetEquals(allCoordinatesFromUpdatedGameState));
            Assert.AreEqual(5, updatedGameState.TurnNumber);
            Assert.AreEqual(PlayerColor.BLACK, updatedGameState.CurrentPlayerTurnColor);
        }

        [TestMethod]
        public void Given_GameStateWherePlayerMoved_When_PlayerMovementActionReverted_Then_ReturnsPreviousGameState()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn 4 pieces including the queen for each player
            for (var i = 0; i < 8; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }
            var allCoordinatesFromPreviousGameState = gameState.CoordinateSystem.GetAllCoordinates();
            var availableMovementActions = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerMovementAction>();
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(gameState, availableMovementActions.First());

            // WHEN
            var revertedGameState = GameRules.UndoLastMoveFromGameState(updatedGameState);

            // THEN
            var allCoordinatesFromRevertedGameState = updatedGameState.CoordinateSystem.GetAllCoordinates();
            Assert.HasCount(8, allCoordinatesFromRevertedGameState);
            Assert.IsTrue(allCoordinatesFromPreviousGameState.SetEquals(allCoordinatesFromRevertedGameState));
            Assert.AreEqual(5, revertedGameState.TurnNumber);
            Assert.AreEqual(PlayerColor.WHITE, revertedGameState.CurrentPlayerTurnColor);
        }

        [TestMethod]
        public void Given_GameStateWithBeetleOnSecondLevel_When_PlayerMovementActionApplied_Then_ReturnsUpdatedGameState()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn 4 pieces including the queen for each player
            for (var i = 0; i < 8; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }
            // Spawn a beetle for each player.
            var spawnBeetleAction = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(a
                => a.PieceToSpawn.GetType() == typeof(BeetlePiece));
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, spawnBeetleAction);
            spawnBeetleAction = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(a
                => a.PieceToSpawn.GetType() == typeof(BeetlePiece));
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, spawnBeetleAction);

            var allCoordinatesFromOriginalGameState = gameState.CoordinateSystem.GetAllCoordinates();
            var availableMovementActions = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerMovementAction>();

            PlayerMovementAction? beetleMovementToSecondFloorAction = null;
            foreach (var action in availableMovementActions)
            {
                gameState.CoordinateSystem.TryGetHexagon(action.StartCoordinate, out var hexagon);
                if (hexagon!.PeekPiece().GetType() == typeof(BeetlePiece) && gameState.CoordinateSystem.GetAllCoordinates().Contains(action.DestinationCoordinate))
                {
                    beetleMovementToSecondFloorAction = action;
                    break;
                }
            }
            Assert.IsNotNull(beetleMovementToSecondFloorAction);

            // WHEN
            var updatedGameState = GameRules.ApplyPlayerActionToGameState(gameState, beetleMovementToSecondFloorAction);

            // THEN
            var allCoordinatesFromUpdatedGameState = updatedGameState.CoordinateSystem.GetAllCoordinates();
            Assert.HasCount(9, allCoordinatesFromUpdatedGameState);
            Assert.IsFalse(allCoordinatesFromOriginalGameState.SetEquals(allCoordinatesFromUpdatedGameState));
            Assert.AreEqual(6, updatedGameState.TurnNumber);
            Assert.AreEqual(PlayerColor.BLACK, updatedGameState.CurrentPlayerTurnColor);
            gameState.CoordinateSystem.TryGetHexagon(beetleMovementToSecondFloorAction.DestinationCoordinate, out var destinationHexagon);
            Assert.IsNotNull(destinationHexagon);
            Assert.AreEqual(2, destinationHexagon.GetPieceCount());
        }

        [TestMethod]
        public void Given_GameStateWithBeetleOnSecondLevel_When_PlayerMovementActionReverted_Then_ReturnsPreviousGameState()
        {
            // GIVEN
            var gameState = GameRules.CreateFreshGameState();
            // Spawn 4 pieces including the queen for each player
            for (var i = 0; i < 8; i++)
            {
                var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First();
                gameState = GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);
            }
            // Spawn a beetle for each player.
            var spawnBeetleAction = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(a
                => a.PieceToSpawn.GetType() == typeof(BeetlePiece));
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, spawnBeetleAction);
            spawnBeetleAction = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(a
                => a.PieceToSpawn.GetType() == typeof(BeetlePiece));
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, spawnBeetleAction);

            var allCoordinatesFromPreviousGameState = gameState.CoordinateSystem.GetAllCoordinates();
            var availableMovementActions = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerMovementAction>();

            PlayerMovementAction? beetleMovementToSecondFloorAction = null;
            foreach (var action in availableMovementActions)
            {
                gameState.CoordinateSystem.TryGetHexagon(action.StartCoordinate, out var hexagon);
                if (hexagon!.PeekPiece().GetType() == typeof(BeetlePiece) && gameState.CoordinateSystem.GetAllCoordinates().Contains(action.DestinationCoordinate))
                {
                    beetleMovementToSecondFloorAction = action;
                    break;
                }
            }
            Assert.IsNotNull(beetleMovementToSecondFloorAction);
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, beetleMovementToSecondFloorAction);

            // WHEN
            var revertedGameState = GameRules.UndoLastMoveFromGameState(gameState);

            // THEN
            var allCoordinatesFromRevertedGameState = revertedGameState.CoordinateSystem.GetAllCoordinates();
            Assert.HasCount(10, allCoordinatesFromRevertedGameState);
            Assert.IsTrue(allCoordinatesFromPreviousGameState.SetEquals(allCoordinatesFromRevertedGameState));
            Assert.AreEqual(6, revertedGameState.TurnNumber);
            Assert.AreEqual(PlayerColor.WHITE, revertedGameState.CurrentPlayerTurnColor);
            gameState.CoordinateSystem.TryGetHexagon(beetleMovementToSecondFloorAction.DestinationCoordinate, out var destinationHexagon);
            Assert.IsNotNull(destinationHexagon);
            Assert.AreEqual(1, destinationHexagon.GetPieceCount());
        }

        [TestMethod]
        public void Given_GameStateWherePlayerHasNoMoves_When_PlayerMovementActionApplied_Then_ReturnsUpdatedGameState()
        {
            // GIVEN
        }

        [TestMethod]
        public void Given_GameStateWherePlayerHasNoMoves_When_PlayerMovementActionReverted_Then_ReturnsPreviousGameState()
        {
            // GIVEN
        }
    }
}
