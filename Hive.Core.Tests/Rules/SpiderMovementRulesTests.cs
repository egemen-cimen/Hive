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
        public void Given_PopulatedCoordinateSystem_When_SpiderThreeSpaceMomementIsValidated_Then_ReturnsValid(int endColumn, int endRow)
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
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (endColumn, endRow), PlayerColor.WHITE, 3);

            // THEN
            Assert.AreEqual(MovementValidationResult.VALID, result);
        }

        [TestMethod]
        [DataRow(1, -2)]
        [DataRow(2, -2)]
        [DataRow(-1, 0)]
        [DataRow(-1, 1)]
        public void Given_PopulatedCoordinateSystem_When_SpiderLessNotThreeSpaceMomementIsValidated_Then_ReturnsValidationFail(int endColumn, int endRow)
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
            var result = spiderMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (endColumn, endRow), PlayerColor.WHITE, 4);

            // THEN
            Assert.AreEqual(MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION, result);
        }
    }
}
