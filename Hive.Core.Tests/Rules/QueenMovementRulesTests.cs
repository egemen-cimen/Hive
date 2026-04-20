using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.Core.Tests.Rules
{
    [TestClass]
    public class QueenMovementRulesTests
    {
        [TestMethod]
        [DataRow(0, -1)]
        [DataRow(1, 0)]
        public void Given_CoordinateSystemWithThreePieces_When_QueenOneSpaceMomementIsValidated_Then_ReturnsValid(int endColumn, int endRow)
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
                (( 0,  0), typeof(SpiderPiece)),
                (( 0,  1), typeof(SpiderPiece)),
                (( 1, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(AntPiece))
            ]);
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (endColumn, endRow), PlayerColor.WHITE, 3);

            // THEN
            Assert.AreEqual(MovementValidationResult.VALID, result);
        }

        [TestMethod]
        [DataRow(-1, 0)]
        [DataRow(-1, 1)]
        [DataRow(-1, 2)]
        [DataRow(1, 1)]
        public void Given_CoordinateSystemWithThreePieces_When_QueenTwoSpaceMomementIsValidated_Then_ReturnsValidationFail(int endColumn, int endRow)
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
                (( 0,  0), typeof(SpiderPiece)),
                (( 0,  1), typeof(SpiderPiece)),
                (( 1, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(AntPiece))
            ]);
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (endColumn, endRow), PlayerColor.WHITE, 3);

            // THEN
            Assert.AreEqual(MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithThreePieces_When_QueenOneSpaceMomementToOccupiedSpaceIsValidated_Then_ReturnsValidationFail()
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
                (( 0,  0), typeof(SpiderPiece)),
                (( 0,  1), typeof(SpiderPiece)),
                (( 1, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(AntPiece))
            ]);
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (0, 0), PlayerColor.WHITE, 3);

            // THEN
            Assert.AreEqual(MovementValidationResult.DESTINATION_IS_NOT_EMPTY, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithThreePieces_When_AnotherTypeIsValidatedWithQueenValidator_Then_ReturnsValidationFail()
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (-1, 0), PlayerColor.WHITE, 4);

            // THEN
            Assert.AreEqual(MovementValidationResult.WRONG_VALIDATOR_FOR_PIECE_TYPE, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithPieces_When_EmptySpaceMomementIsValidated_Then_ReturnsValidationFail()
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
                (( 0,  0), typeof(SpiderPiece)),
                (( 0,  1), typeof(SpiderPiece)),
                (( 1, -1), typeof(QueenPiece)),
                (( 0,  2), typeof(AntPiece))
            ]);
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, 0), (1, 1), PlayerColor.WHITE, 3);

            // THEN
            Assert.AreEqual(MovementValidationResult.NO_PIECE_TO_MOVE, result);
        }
    }
}
