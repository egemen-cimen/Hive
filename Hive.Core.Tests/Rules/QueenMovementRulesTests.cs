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
    }
}
