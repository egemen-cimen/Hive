using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.PlayerAgent.Tests
{
    [TestClass]
    public class MinimaxPlayerTests
    {
        [TestMethod]
        public void Given_GameStateWhereWhiteCanWinInOneMove_When_SuggestedNextMoveRetrievedForWhite_Then_ReturnsWinningMove()
        {
            // GIVEN
            var minimaxPlayer = new MinimaxPlayer();

            //      [BLK A]
            //      [ 0,-3]
            //
            //  [BLK A] [BLK Q] [BLK A]
            //  [-1,-2] [ 0,-2] [ 1,-2]
            //
            //      [BLK A] [BLK S]
            //      [-1,-1] [ 0,-1]
            //
            //                  [WHT S]
            //                  [ 0, 0]
            //
            //                      [WHT Q]
            //                      [ 0, 1]
            //
            //                          [WHT B]
            //                          [ 0, 2]
            //
            //                              [WHT A]
            //                              [ 0, 3]
            //
            //                                  [WHT A]
            //                                  [ 0, 4]
            //
            //                                      [WHT A]
            //                                      [ 0, 5]
            //
            // White can win in one move.
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

            // Take a snapshot before calling the method
            var beforeMethodCall = GetStringSnapshotOfGameState(gameState);

            // WHEN
            var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);

            // THEN
            // Take a snapshot after calling the method and compare
            var afterMethodCall = GetStringSnapshotOfGameState(gameState);
            CollectionAssert.AreEquivalent(beforeMethodCall, afterMethodCall);

            Assert.IsInstanceOfType<PlayerMovementAction>(suggestedAction);
            Assert.Contains(((PlayerMovementAction)suggestedAction).DestinationCoordinate, spacesNextToQueen);
            GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);
            Assert.IsTrue(GameRules.VerifyWhetherGameStateIsTerminal(gameState));
            Assert.AreEqual(GameResult.WHITE_WON, GameRules.GetGameResult(gameState));

            Assert.AreEqual(119_033, minimaxPlayer.GetEvaluationCount()); // evaluation count without alpha-beta pruning was 317_494
        }

        [TestMethod]
        public void Given_GameStateWhereBlackCanWinInOneMove_When_SuggestedNextMoveRetrievedForBlack_Then_ReturnsWinningMove()
        {
            // GIVEN
            var minimaxPlayer = new MinimaxPlayer();

            //      [BLK A]
            //      [ 0,-6]
            //
            //          [BLK A]
            //          [ 0,-5]
            //
            //              [BLK A]
            //              [ 0,-4]
            //
            //                  [BLK B]
            //                  [ 0,-3]
            //
            //                      [BLK Q]
            //                      [ 0,-2]
            //
            //                          [BLK S]
            //                          [ 0,-1]
            //
            //                              [WHT S] [WHT A]
            //                              [ 0, 0] [ 1, 0]
            //
            //                          [WHT A] [WHT Q] [WHT A]
            //                          [-1, 1] [ 0, 1] [ 1, 1]
            //
            //                      [WHT B] [WHT B]
            //                      [-2, 2] [-1, 2]
            //
            // Black can win in one move.
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

            // Take a snapshot before calling the method
            var beforeMethodCall = GetStringSnapshotOfGameState(gameState);

            // WHEN
            var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);

            // THEN
            // Take a snapshot after calling the method and compare
            var afterMethodCall = GetStringSnapshotOfGameState(gameState);
            CollectionAssert.AreEquivalent(beforeMethodCall, afterMethodCall);

            Assert.IsInstanceOfType<PlayerMovementAction>(suggestedAction);
            Assert.Contains(((PlayerMovementAction)suggestedAction).DestinationCoordinate, spacesNextToQueen);
            GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);
            Assert.IsTrue(GameRules.VerifyWhetherGameStateIsTerminal(gameState));
            Assert.AreEqual(GameResult.BLACK_WON, GameRules.GetGameResult(gameState));

            Assert.AreEqual(152_200, minimaxPlayer.GetEvaluationCount()); // evaluation count without alpha-beta pruning was 341_491
        }

        [TestMethod]
        public void Given_GameStateWhereWhiteCanWinInNextTurn_When_SuggestedNextMoveRetrievedForBlack_Then_ReturnsBlockingForWinningMove()
        {
            // GIVEN
            var minimaxPlayer = new MinimaxPlayer();

            //      [BLK A]
            //      [ 0,-3]
            //
            //  [BLK A] [BLK Q] [BLK A]
            //  [-1,-2] [ 0,-2] [ 1,-2]
            //
            //      [BLK A] [BLK S]
            //      [-1,-1] [ 0,-1]
            //
            //                  [WHT S] [WHT B]
            //                  [ 0, 0] [ 1, 0]
            //
            //                      [WHT Q]
            //                      [ 0, 1]
            //
            //                          [WHT B]
            //                          [ 0, 2]
            //
            //                              [WHT A]
            //                              [ 0, 3]
            //
            //                                  [WHT A]
            //                                  [ 0, 4]
            //
            //                                      [WHT A]
            //                                      [ 0, 5]
            //
            // White can win in the next turn. Black should pin the white ant to avoid a game over.
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
            (int column, int row) lastWhiteAntCoordinate = (int.MinValue, int.MinValue);
            for (var i = 0; i < 3; i++)
            {
                // Don't surround white queen & also spawn the ants
                var spawnAction = ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(AntPiece) && a.DestinationCoordinate.column == 0);
                lastWhiteAntCoordinate = spawnAction.DestinationCoordinate;

                // Continue surrounding the black queen
                ApplyPlayerSpawnActionPredicateToGameState(gameState, a => spacesNextToQueen.Contains(a.DestinationCoordinate));
            }

            // Blunder finishing the game so black has a change to block
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() != typeof(AntPiece) && a.DestinationCoordinate.column != 0);

            // Take a snapshot before calling the method
            var beforeMethodCall = GetStringSnapshotOfGameState(gameState);

            // WHEN
            var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);

            // THEN
            // Take a snapshot after calling the method and compare
            var afterMethodCall = GetStringSnapshotOfGameState(gameState);
            CollectionAssert.AreEquivalent(beforeMethodCall, afterMethodCall);

            Assert.IsInstanceOfType<PlayerMovementAction>(suggestedAction);
            var spacesNextToWhiteAnt = gameState.CoordinateSystem.GetAdjacentCoordinates(lastWhiteAntCoordinate);
            Assert.Contains(((PlayerMovementAction)suggestedAction).DestinationCoordinate, spacesNextToWhiteAnt);

            Assert.AreEqual(174_828, minimaxPlayer.GetEvaluationCount()); // evaluation count without alpha-beta pruning was 480_157
        }

        [TestMethod]
        public void Given_GameStateWhereBlackCanWinInNextTurn_When_SuggestedNextMoveRetrievedForWhite_Then_ReturnsBlockingForWinningMove()
        {
            // GIVEN
            var minimaxPlayer = new MinimaxPlayer();

            //      [BLK A]
            //      [ 0,-6]
            //
            //          [BLK A]
            //          [ 0,-5]
            //
            //              [BLK A]
            //              [ 0,-4]
            //
            //                  [BLK B]
            //                  [ 0,-3]
            //
            //                      [BLK Q]
            //                      [ 0,-2]
            //
            //                          [BLK S]
            //                          [ 0,-1]
            //
            //                              [WHT S] [WHT A]
            //                              [ 0, 0] [ 1, 0]
            //
            //                          [WHT A] [WHT Q] [WHT A]
            //                          [-1, 1] [ 0, 1] [ 1, 1]
            //
            //                              [WHT B]
            //                              [-1, 2]
            //
            // Black can win in the next turn. White should pin the black ant to avoid a game over.
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
            (int column, int row) lastBlackAntCoordinate = (int.MinValue, int.MinValue);
            for (var i = 0; i < 3; i++)
            {
                // Continue surrounding the white queen
                ApplyPlayerSpawnActionPredicateToGameState(gameState, a => spacesNextToQueen.Contains(a.DestinationCoordinate));

                // Don't surround black queen & also spawn the ants
                var spawnAction = ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(AntPiece) && a.DestinationCoordinate.column == 0);
                lastBlackAntCoordinate = spawnAction.DestinationCoordinate;
            }

            // Take a snapshot before calling the method
            var beforeMethodCall = GetStringSnapshotOfGameState(gameState);

            // WHEN
            var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);

            // THEN
            // Take a snapshot after calling the method and compare
            var afterMethodCall = GetStringSnapshotOfGameState(gameState);
            CollectionAssert.AreEquivalent(beforeMethodCall, afterMethodCall);

            Assert.IsInstanceOfType<PlayerMovementAction>(suggestedAction);
            var spacesNextToBlackAnt = gameState.CoordinateSystem.GetAdjacentCoordinates(lastBlackAntCoordinate);
            Assert.Contains(((PlayerMovementAction)suggestedAction).DestinationCoordinate, spacesNextToBlackAnt);

            Assert.AreEqual(244_274, minimaxPlayer.GetEvaluationCount()); // evaluation count without alpha-beta pruning was 508_426
        }

        [TestMethod]
        public void Given_GameStateWherePlayerHasNoMoves_When_SuggestedNextMoveRetrieved_Then_ReturnsUnableToPlayAction()
        {
            // GIVEN
            var minimaxPlayer = new MinimaxPlayer();

            // Game state copied from GameRulesTests
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

            // Move ant to the other side to block the other player's last possible move
            var antMovementToOtherSideAction = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerMovementAction>().First(a
                => a.DestinationCoordinate.column == 0);
            gameState = GameRules.ApplyPlayerActionToGameState(gameState, antMovementToOtherSideAction);

            // WHEN
            var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);

            // THEN
            Assert.IsInstanceOfType<PlayerUnableToPlayAction>(suggestedAction);

            Assert.AreEqual(90, minimaxPlayer.GetEvaluationCount()); // evaluation count without alpha-beta pruning was 1_944
        }

        [TestMethod]
        public void Given_GameStateWithTie_When_SuggestedNextMoveRetrieved_Then_ReturnsActionToImproveSituation()
        {
            // GIVEN
            var minimaxPlayer = new MinimaxPlayer();

            //              [BLK A] [BLK Q] [BLK A]
            //              [-1,-2] [ 0,-2] [ 1,-2]
            //
            //                  [BLK A] [BLK S]
            //                  [-1,-1] [ 0,-1]
            //
            //                              [WHT S] [WHT A]
            //                              [ 0, 0] [ 1, 0]
            //
            //                          [WHT A] [WHT Q] [WHT A]
            //                          [-1, 1] [ 0, 1] [ 1, 1]
            //
            // Players are in a tie. White queen should move to improve it's situation.
            var gameState = GameRules.CreateFreshGameState();
            // Spawn first pieces in a line
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(SpiderPiece) && a.DestinationCoordinate.column == 0);
            ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(SpiderPiece) && a.DestinationCoordinate.column == 0);
            // Spawn queens as second pieces in a line and get the coordinates
            var queenSpawnAction = ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(QueenPiece) && a.DestinationCoordinate.column == 0);
            var spacesNextToWhiteQueen = gameState.CoordinateSystem.GetAdjacentCoordinates(queenSpawnAction.DestinationCoordinate);
            queenSpawnAction = ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(QueenPiece) && a.DestinationCoordinate.column == 0);
            var spacesNextToBlackQueen = gameState.CoordinateSystem.GetAdjacentCoordinates(queenSpawnAction.DestinationCoordinate);

            for (var i = 0; i < 3; i++)
            {
                // Surround white queen with ants
                ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(AntPiece) && spacesNextToWhiteQueen.Contains(a.DestinationCoordinate));

                // Surround black queen with ants
                ApplyPlayerSpawnActionPredicateToGameState(gameState, a => a.PieceToSpawn.GetType() == typeof(AntPiece) && spacesNextToBlackQueen.Contains(a.DestinationCoordinate));
            }

            // WHEN
            var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);

            // THEN
            Assert.IsInstanceOfType<PlayerMovementAction>(suggestedAction);
            var hexagon = gameState.CoordinateSystem.GetHexagonAtCoordinate(((PlayerMovementAction)suggestedAction).StartCoordinate);
            Assert.IsInstanceOfType<QueenPiece>(hexagon.PeekPiece());

            Assert.AreEqual(115_027, minimaxPlayer.GetEvaluationCount()); // evaluation count without alpha-beta pruning was 357_966
        }

        private static PlayerSpawnAction ApplyPlayerSpawnActionPredicateToGameState(GameState gameState, Func<PlayerSpawnAction, bool> spawnPredicate)
        {
            var actionToBeApplied = GameRules.GetAllAvailablePlayerActions(gameState).OfType<PlayerSpawnAction>().First(spawnPredicate);
            GameRules.ApplyPlayerActionToGameState(gameState, actionToBeApplied);

            return actionToBeApplied;
        }

        private static List<string> GetStringSnapshotOfGameState(GameState gameState)
        {
            var result = new List<string>
            {
                $"TurnNumber:{gameState.TurnNumber}",
                $"CurrentPlayerTurnColor:{gameState.CurrentPlayerTurnColor}"
            };

            var allCoordinates = gameState.CoordinateSystem.GetAllCoordinates();
            foreach (var coordinate in allCoordinates)
            {
                var hexagon = gameState.CoordinateSystem.GetHexagonAtCoordinate(coordinate);
                var stringRepresentationOfHexagon = $"HexagonAt{coordinate}:" +
                    string.Join(";", hexagon.GetAllPieces().Select(p => (p.Color, p.GetPieceName())));
                result.Add(stringRepresentationOfHexagon);
            }

            foreach (var pastPlayerAction in gameState.PastPlayerActions)
            {
                string stringRepresentationOfPlayerAction;
                if (pastPlayerAction.GetType() == typeof(PlayerSpawnAction))
                {
                    var spawnAction = (PlayerSpawnAction)pastPlayerAction;
                    stringRepresentationOfPlayerAction = $"Spawn{(spawnAction.PieceToSpawn.Color, spawnAction.PieceToSpawn.GetPieceName())}" +
                        $"At:{spawnAction.DestinationCoordinate}";
                }
                else if (pastPlayerAction.GetType() == typeof(PlayerMovementAction))
                {
                    var movementAction = (PlayerMovementAction)pastPlayerAction;
                    stringRepresentationOfPlayerAction = $"MoveFrom:{movementAction.StartCoordinate}" +
                        $"To:{movementAction.DestinationCoordinate}";
                }
                else // Player unable to play
                {
                    stringRepresentationOfPlayerAction = "UnableToPlay";
                }

                result.Add(stringRepresentationOfPlayerAction);
            }

            return result;
        }
    }
}
