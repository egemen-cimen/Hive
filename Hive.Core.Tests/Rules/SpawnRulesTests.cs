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
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 0), PlayerColor.WHITE, 1);

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
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 0), PlayerColor.WHITE, 1);

            // THEN
            Assert.AreEqual(SpawnValidationResult.WRONG_COLOR_PLACED, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithOnePiece_When_PieceSpawnOnAdjacentCoordinateValidated_Then_ReturnsValid()
        {
            // GIVEN
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new SpiderPiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 1), PlayerColor.BLACK, 1);

            // THEN
            Assert.AreEqual(SpawnValidationResult.VALID, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithOnePiece_When_PieceSpawnOnSameCoordinateValidated_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new BeetlePiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 0), PlayerColor.BLACK, 1);

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
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
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
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (-2, 0), PlayerColor.WHITE, 4);

            // THEN
            Assert.AreEqual(SpawnValidationResult.QUEEN_WAS_NOT_PLACED_UNTIL_FOURTH_TURN, result);
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
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
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
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 3), PlayerColor.BLACK, 4);

            // THEN
            Assert.AreEqual(SpawnValidationResult.QUEEN_WAS_NOT_PLACED_UNTIL_FOURTH_TURN, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_UnreachablePieceSpawned_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new SpiderPiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 2), PlayerColor.BLACK, 1);

            // THEN
            Assert.AreEqual(SpawnValidationResult.PIECE_DID_NOT_TOUCH_THE_HIVE, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithMoreThanTwoPiece_When_PieceSpawnedAdjacentToEnemyPieces_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece)),
                ((0, 1), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new SpiderPiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 2), PlayerColor.WHITE, 2);

            // THEN
            Assert.AreEqual(SpawnValidationResult.PIECE_TOUCHED_ENEMY_PIECE, result);
        }

        [TestMethod]
        public void Given_EmptyCoordinateSystem_When_QueenSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();

            // WHEN
            var piece = new QueenPiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 0), PlayerColor.WHITE, 1);

            // THEN
            Assert.AreEqual(SpawnValidationResult.QUEEN_CANNOT_BE_PLACED_ON_FIRST_TURN, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithOnePiece_When_QueenSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                ((0, 0), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new QueenPiece(PlayerColor.BLACK);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, 1), PlayerColor.BLACK, 1);

            // THEN
            Assert.AreEqual(SpawnValidationResult.QUEEN_CANNOT_BE_PLACED_ON_FIRST_TURN, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithOneWhiteQueen_When_AnotherQueenPieceSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0,  0), typeof(SpiderPiece)),
                (( 0,  1), typeof(SpiderPiece)),
                (( 0, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(QueenPiece))
            ]);

            // WHEN
            var piece = new QueenPiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, -2), PlayerColor.WHITE, 3);

            // THEN
            Assert.AreEqual(SpawnValidationResult.MORE_THAN_AVAILABLE_PIECES_SPAWNED, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithTwoWhiteSpiderPieces_When_AnotherSpiderPieceSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0,  0), typeof(SpiderPiece)),
                (( 0,  1), typeof(SpiderPiece)),
                (( 0, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(QueenPiece)),
                (( 0, -2), typeof(SpiderPiece)),
                (( 0,  3), typeof(SpiderPiece))
            ]);

            // WHEN
            var piece = new SpiderPiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, -3), PlayerColor.WHITE, 4);

            // THEN
            Assert.AreEqual(SpawnValidationResult.MORE_THAN_AVAILABLE_PIECES_SPAWNED, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithTwoWhiteBeetlePieces_When_AnotherBeetlePieceSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0,  0), typeof(BeetlePiece)),
                (( 0,  1), typeof(BeetlePiece)),
                (( 0, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(QueenPiece)),
                (( 0, -2), typeof(BeetlePiece)),
                (( 0,  3), typeof(BeetlePiece))
            ]);

            // WHEN
            var piece = new BeetlePiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, -3), PlayerColor.WHITE, 4);

            // THEN
            Assert.AreEqual(SpawnValidationResult.MORE_THAN_AVAILABLE_PIECES_SPAWNED, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithThreeWhiteGrasshopperPieces_When_AnotherGrasshopperPieceSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0,  0), typeof(GrasshopperPiece)),
                (( 0,  1), typeof(GrasshopperPiece)),
                (( 0, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(QueenPiece)),
                (( 0, -2), typeof(GrasshopperPiece)),
                (( 0,  3), typeof(GrasshopperPiece)),
                (( 0, -3), typeof(GrasshopperPiece)),
                (( 0,  4), typeof(GrasshopperPiece))
            ]);

            // WHEN
            var piece = new GrasshopperPiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, -4), PlayerColor.WHITE, 5);

            // THEN
            Assert.AreEqual(SpawnValidationResult.MORE_THAN_AVAILABLE_PIECES_SPAWNED, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithThreeWhiteAntPieces_When_AnotherAntPieceSpawnValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0,  0), typeof(AntPiece)),
                (( 0,  1), typeof(AntPiece)),
                (( 0, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(QueenPiece)),
                (( 0, -2), typeof(AntPiece)),
                (( 0,  3), typeof(AntPiece)),
                (( 0, -3), typeof(AntPiece)),
                (( 0,  4), typeof(AntPiece))
            ]);

            // WHEN
            var piece = new AntPiece(PlayerColor.WHITE);
            var result = SpawnRules.ValidatePieceSpawn(piece, coordinateSystem, (0, -4), PlayerColor.WHITE, 5);

            // THEN
            Assert.AreEqual(SpawnValidationResult.MORE_THAN_AVAILABLE_PIECES_SPAWNED, result);
        }
    }
}
