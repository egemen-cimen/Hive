using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.Core.Tests.Rules
{
    [TestClass]
    public class SpiderMovementRulesTests
    {
        [TestMethod]
        [DataRow(2, -1)]
        [DataRow(-2, 2)]
        public void Given_PopulatedCoordinateSystem_When_SpiderThreeSpaceMovementIsValidated_Then_ReturnsValid(int endColumn, int endRow)
        {
            // GIVEN

            //  [WHT S] [WHT Q]
            //  [ 0,-1] [ 1,-1]
            //
            //      [WHT S]
            //      [ q, r]
            //
            //          [BLK S]
            //          [ 0, 1]
            //
            //      [BLK S] [BLK A]
            //      [-1, 2] [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 0,-1), typeof(SpiderPiece)),
                ((-1, 2), typeof(SpiderPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (endColumn, endRow), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.VALID, result);
        }

        [TestMethod]
        [DataRow(1, -2)]
        [DataRow(2, -2)]
        [DataRow(-1, 0)]
        [DataRow(-1, 1)]
        [DataRow(-2, 3)]
        [DataRow(-1, 3)]
        [DataRow(0, 3)]
        [DataRow(1, 2)]
        [DataRow(1, 1)]
        [DataRow(1, 0)]
        public void Given_PopulatedCoordinateSystem_When_SpiderNotThreeSpaceMovementIsValidated_Then_ReturnsValidationFail(int endColumn, int endRow)
        {
            // GIVEN

            //  [WHT S] [WHT Q]
            //  [ 0,-1] [ 1,-1]
            //
            //      [WHT S]
            //      [ q, r]
            //
            //          [BLK S]
            //          [ 0, 1]
            //
            //      [BLK S] [BLK A]
            //      [-1, 2] [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 0,-1), typeof(SpiderPiece)),
                ((-1, 2), typeof(SpiderPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (endColumn, endRow), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_SpiderThreeSpaceMovementToOccupiedSpaceIsValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //  [WHT S] [WHT Q]
            //  [ 0,-1] [ 1,-1]
            //
            //      [WHT S]
            //      [ q, r]
            //
            //          [BLK S]
            //          [ 0, 1]
            //
            //      [BLK S] [BLK A]
            //      [-1, 2] [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 0,-1), typeof(SpiderPiece)),
                ((-1, 2), typeof(SpiderPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (-1, 2), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.DESTINATION_IS_NOT_EMPTY, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_AnotherTypeIsValidatedWithSpiderValidator_Then_ReturnsValidationFail()
        {
            // GIVEN

            //  [WHT S] [WHT Q]
            //  [ 0,-1] [ 1,-1]
            //
            //      [WHT S]
            //      [ q, r]
            //
            //          [BLK S]
            //          [ 0, 1]
            //
            //      [BLK S] [BLK A]
            //      [-1, 2] [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 0,-1), typeof(SpiderPiece)),
                ((-1, 2), typeof(SpiderPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (1, 2), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.WRONG_VALIDATOR_FOR_PIECE_TYPE, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_EmptySpaceMovementIsValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //      [WHT Q]
            //      [ 1,-1]
            //
            //  [WHT S]
            //  [ q, r]
            //
            //      [BLK S]
            //      [ 0, 1]
            //
            //          [BLK A]
            //          [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 2), typeof(AntPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (1, 0), (1, 1), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.NO_PIECE_TO_MOVE, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_MiddlePieceMovementIsValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //          [WHT B]
            //          [ 2,-2]
            //
            //      [WHT Q]
            //      [ 1,-1]
            //
            //  [WHT S]
            //  [ q, r]
            //
            //      [BLK S]
            //      [ 0, 1]
            //
            //  [BLK S] [BLK A]
            //  [-1, 2] [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 2,-2), typeof(BeetlePiece)),
                ((-1, 2), typeof(SpiderPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, 0), (2, -3), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.BREAKS_ONE_HIVE, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_SpiderOneSpaceMovementToUnconnectedSpaceIsValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //  [WHT S] [WHT Q]
            //  [ 0,-1] [ 1,-1]
            //
            //      [WHT S]
            //      [ q, r]
            //
            //          [BLK S]
            //          [ 0, 1]
            //
            //      [BLK S] [BLK A]
            //      [-1, 2] [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 0,-1), typeof(SpiderPiece)),
                ((-1, 2), typeof(SpiderPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (3, -2), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.BREAKS_ONE_HIVE, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_SpiderMovementWithSameCoordinatesIsValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //  [WHT S] [WHT Q]
            //  [ 0,-1] [ 1,-1]
            //
            //      [WHT S]
            //      [ q, r]
            //
            //          [BLK S]
            //          [ 0, 1]
            //
            //      [BLK S] [BLK A]
            //      [-1, 2] [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 0,-1), typeof(SpiderPiece)),
                ((-1, 2), typeof(SpiderPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (0, -1), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.START_AND_DESTINATION_CANNOT_BE_THE_SAME, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithPiecesInABigCShape_When_SpiderMovementToOtherSideIsValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            // Movement doesn't have continuous contact with the hive
            //
            //  	            [ 1,-2] [ 2,-2]
            //
            //      [-1,-1]                 [ 2,-1]
            //
            //  [-2, 0]                         [ 2, 0]
            //
            //      [-2, 1]                 [ 1, 1]
            //
            //          [-2, 2] [-1, 2] [ 0, 2]
            //
            // Spider is at [-1,-1] and it cannot move to [ 2,-3]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                ((-1,-1), typeof(SpiderPiece)),
                ((-2, 0), typeof(SpiderPiece)),
                ((-2, 1), typeof(QueenPiece)),
                ((-2, 2), typeof(QueenPiece)),
                ((-1, 2), typeof(AntPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 1, 1), typeof(AntPiece)),
                (( 2, 0), typeof(AntPiece)),
                (( 2,-1), typeof(BeetlePiece)),
                (( 2,-2), typeof(BeetlePiece)),
                (( 1,-2), typeof(BeetlePiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (-1, -1), (2, -3), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithPiecesInACShape_When_SpiderMovementToCenterOfCircleValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            // Spider cannot move/slide into the center
            //
            //                  [ 2,-2]
            //
            //      [ 0,-1] [ 1,-1]
            //
            //  [-1, 0]
            //
            //      [-1, 1] [ 0, 1]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 2,-2), typeof(SpiderPiece)),
                (( 0,-1), typeof(QueenPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                ((-1, 1), typeof(AntPiece)),
                ((-1, 0), typeof(AntPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (2, -2), (0, 0), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystemWithNoQueen_WhenSpiderMovementIsValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //      [WHT S]
            //      [ 1,-1]
            //
            //  [WHT S]
            //  [ q, r]
            //
            //      [BLK S]
            //      [ 0, 1]
            //
            //          [BLK A]
            //          [ 0, 2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                (( 1,-1), typeof(SpiderPiece)),
                (( 0, 2), typeof(AntPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (1, 2), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.CANNOT_MOVE_WITHOUT_QUEEN, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_WrongColoredSpiderMovementIsValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            //      [ 0,-1] [ 1,-1]
            //
            //  [-1, 0]         [ 1, 0]
            //
            //      [-1, 1] [ 0, 1]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                (( 0,-1), typeof(QueenPiece)),
                (( 1,-1), typeof(QueenPiece)),
                (( 1, 0), typeof(SpiderPiece)),
                (( 0, 1), typeof(SpiderPiece)),
                ((-1, 1), typeof(AntPiece)),
                ((-1, 0), typeof(AntPiece))
            ]);
            var spiderMovementRules = new SpiderMovementRules();

            // WHEN
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, 1), (-2, 1), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.WRONG_COLOR_MOVED, result);
        }
    }
}
