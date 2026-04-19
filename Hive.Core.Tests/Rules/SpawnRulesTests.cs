using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.Core.Tests.Rules
{
    [TestClass]
    public class SpawnRulesTests
    {
        [TestMethod]
        public void Given_EmptyCoordinateSystem_When_FirstPieceSpawnValidated_Then_ReturnsValid()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();

            // WHEN
            var piece = new SpiderPiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 0));

            // THEN
            Assert.AreEqual(SpawnValidationResult.VALID, result);
        }

        [TestMethod]
        public void Given_EmptyCoordinateSystem_When_FirstPieceSpawnValidatedForWrongColor_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();

            // WHEN
            var piece = new SpiderPiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 0));

            // THEN
            Assert.AreEqual(SpawnValidationResult.WRONG_COLOR_PLAYED, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithOnePiece_When_PieceSpawnOnAdjacentCoordinateValidated_Then_ReturnsValid()
        {
            // GIVEN
            var coordinateSystem = CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new SpiderPiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 1));

            // THEN
            Assert.AreEqual(SpawnValidationResult.VALID, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithOnePiece_When_PieceSpawnOnSameCoordinateValidated_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new BeetlePiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 0));

            // THEN
            Assert.AreEqual(SpawnValidationResult.ANOTHER_PIECE_AT_DESTINATION, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithSixPiecesButNoQueenForWhite_When_NonQueenPieceSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //              [WHT B]
            //              [ 1,-1]
            //
            //  [WHT A] [WHT S]
            //  [-1, 0] [ q, r]
            //
            //              [BLK S]
            //              [ 0, 1]
            //
            //          [BLK Q] [BLK G]
            //          [-1, 2] [ 0, 2]
            var coordinateSystem = CreatePopulatedCoordinateSystem(
            [
                (( 0,  0), typeof(SpiderPiece)),
                (( 0,  1), typeof(SpiderPiece)),
                ((-1,  0), typeof(AntPiece)),
                (( 0,  2), typeof(GrasshopperPiece)),
                (( 1, -1), typeof(BeetlePiece)),
                ((-1,  2), typeof(QueenPiece))
            ]);

            // WHEN
            var piece = new BeetlePiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (-2, 0));

            // THEN
            Assert.AreEqual(SpawnValidationResult.QUEEN_SHOULD_BE_PLAYED, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithSevenPiecesButNoQueenForBlack_When_NonQueenPieceSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //                      [WHT B]
            //                      [ 1,-1]
            //
            //  [WHT Q] [WHT A] [WHT S]
            //  [-2, 0] [-1, 0] [ q, r]
            //
            //                      [BLK S]
            //                      [ 0, 1]
            //
            //                  [BLK A] [BLK G]
            //                  [-1, 2] [ 0, 2]
            var coordinateSystem = CreatePopulatedCoordinateSystem(
            [
                (( 0,  0), typeof(SpiderPiece)),
                (( 0,  1), typeof(SpiderPiece)),
                ((-1,  0), typeof(AntPiece)),
                (( 0,  2), typeof(GrasshopperPiece)),
                (( 1, -1), typeof(BeetlePiece)),
                ((-1,  2), typeof(AntPiece)),
                ((-2,  0), typeof(QueenPiece))
            ]);

            // WHEN
            var piece = new BeetlePiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 3));

            // THEN
            Assert.AreEqual(SpawnValidationResult.QUEEN_SHOULD_BE_PLAYED, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_UnreachablePieceSpawned_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new SpiderPiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 2));

            // THEN
            Assert.AreEqual(SpawnValidationResult.PIECE_MUST_TOUCH_THE_HIVE, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithMoreThanTwoPiece_When_PieceSpawnedAdjacentToEnemyPieces_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece)),
                ((0, 1), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new SpiderPiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 2));

            // THEN
            Assert.AreEqual(SpawnValidationResult.PIECE_MUST_ONLY_TOUCH_FRIENDLY_PIECES, result);
        }

        private static AxialCoordinateSystem CreatePopulatedCoordinateSystem(List<((int column, int row) coordinate, Type pieceType)> exampleMoves)
        {
            var coordinateSystem = new AxialCoordinateSystem();
            var currentColor = PlayerColor.WHITE;

            foreach (var (coordinate, pieceType) in exampleMoves)
            {
                SpawnPiece(coordinateSystem, coordinate, pieceType, currentColor);
                currentColor = currentColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;
            }

            return coordinateSystem;
        }

        private static void SpawnPiece(AxialCoordinateSystem coordinateSystem, (int column, int row) coordinate, Type pieceType, PlayerColor pieceColor)
        {
            var hexagon = new Hexagon();

            if (pieceType == typeof(AntPiece))
            {
                hexagon.PushPiece(new AntPiece(pieceColor));
            }
            else if (pieceType == typeof(BeetlePiece))
            {
                hexagon.PushPiece(new BeetlePiece(pieceColor));
            }
            else if (pieceType == typeof(GrasshopperPiece))
            {
                hexagon.PushPiece(new GrasshopperPiece(pieceColor));
            }
            else if (pieceType == typeof(QueenPiece))
            {
                hexagon.PushPiece(new QueenPiece(pieceColor));
            }
            else if (pieceType == typeof(SpiderPiece))
            {
                hexagon.PushPiece(new SpiderPiece(pieceColor));
            }

            coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);
        }
    }
}
