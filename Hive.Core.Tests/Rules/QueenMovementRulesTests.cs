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
        public void Given_PopulatedCoordinateSystem_When_QueenOneSpaceMovementIsValidated_Then_ReturnsValid(int endColumn, int endRow)
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (endColumn, endRow), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.VALID, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_AllValidQueenMovementIsRetrieved_Then_ReturnsAllValid()
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
            var queenMovementRules = new QueenMovementRules();
            var startCoordinate = (1, -1);

            // WHEN
            var allAvailableMovements = queenMovementRules.GetAllAvailablePieceMovements(coordinateSystem, startCoordinate, PlayerColor.WHITE);

            // THEN
            Assert.HasCount(2, allAvailableMovements);
            foreach (var availableMovement in allAvailableMovements)
            {
                Assert.AreEqual(MovementValidationResult.VALID, queenMovementRules.ValidatePieceMovement(coordinateSystem, startCoordinate, availableMovement, PlayerColor.WHITE));
            }
        }

        [TestMethod]
        [DataRow(-1, 0)]
        [DataRow(-1, 1)]
        [DataRow(-1, 2)]
        [DataRow(1, 1)]
        public void Given_PopulatedCoordinateSystem_When_QueenTwoSpaceMovementIsValidated_Then_ReturnsValidationFail(int endColumn, int endRow)
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (endColumn, endRow), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_QueenOneSpaceMovementToOccupiedSpaceIsValidated_Then_ReturnsValidationFail()
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (0, 0), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.DESTINATION_IS_NOT_EMPTY, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_AnotherTypeIsValidatedWithQueenValidator_Then_ReturnsValidationFail()
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
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (-1, 0), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.WRONG_VALIDATOR_FOR_PIECE_TYPE, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_QueenMovementIsRetrievedForAnotherType_Then_ReturnsEmpty()
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
            var allAvailableMovements = queenMovementRules.GetAllAvailablePieceMovements(coordinateSystem, (0, -1), PlayerColor.WHITE);

            // THEN
            Assert.HasCount(0, allAvailableMovements);
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, 0), (1, 1), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.NO_PIECE_TO_MOVE, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_AllValidQueenMovementForEmptySpaceIsRetrieved_Then_ReturnsEmpty()
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var allAvailableMovements = queenMovementRules.GetAllAvailablePieceMovements(coordinateSystem, (1, 0), PlayerColor.WHITE);

            // THEN
            Assert.HasCount(0, allAvailableMovements);
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (1, 0), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.BREAKS_ONE_HIVE, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_AllValidQueenMovementForMiddlePieceIsRetrieved_Then_ReturnsEmpty()
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var allAvailableMovements = queenMovementRules.GetAllAvailablePieceMovements(coordinateSystem, (1, -1), PlayerColor.WHITE);

            // THEN
            Assert.HasCount(0, allAvailableMovements);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_QueenOneSpaceMovementToUnconnectedSpaceIsValidated_Then_ReturnsValidationFail()
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (2, -1), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.BREAKS_ONE_HIVE, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_QueenMovementWithSameCoordinatesIsValidated_Then_ReturnsValidationFail()
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (1, -1), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.START_AND_DESTINATION_CANNOT_BE_THE_SAME, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithPiecesInABigCShape_When_QueenMovementToOtherSideIsValidated_Then_ReturnsValidationFail()
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
            // Queen is at [-1,-1] and it cannot move to [ 0,-2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                ((-1,-1), typeof(QueenPiece)),
                ((-2, 0), typeof(QueenPiece)),
                ((-2, 1), typeof(SpiderPiece)),
                ((-2, 2), typeof(SpiderPiece)),
                ((-1, 2), typeof(AntPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 1, 1), typeof(AntPiece)),
                (( 2, 0), typeof(AntPiece)),
                (( 2,-1), typeof(BeetlePiece)),
                (( 2,-2), typeof(BeetlePiece)),
                (( 1,-2), typeof(BeetlePiece))
            ]);
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (-1, -1), (0, -2), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithPiecesInABigCShape_When_AllValidQueenMovementIsRetrieved_Then_ReturnsAllValid()
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
            // Queen is at [-1,-1] and it cannot move to [ 0,-2]
            var coordinateSystem = CoordinateSystemPopulationHelper.CreatePopulatedCoordinateSystem(
            [
                ((-1,-1), typeof(QueenPiece)),
                ((-2, 0), typeof(QueenPiece)),
                ((-2, 1), typeof(SpiderPiece)),
                ((-2, 2), typeof(SpiderPiece)),
                ((-1, 2), typeof(AntPiece)),
                (( 0, 2), typeof(AntPiece)),
                (( 1, 1), typeof(AntPiece)),
                (( 2, 0), typeof(AntPiece)),
                (( 2,-1), typeof(BeetlePiece)),
                (( 2,-2), typeof(BeetlePiece)),
                (( 1,-2), typeof(BeetlePiece))
            ]);
            var queenMovementRules = new QueenMovementRules();
            var startCoordinate = (-1, -1);

            // WHEN
            var allAvailableMovements = queenMovementRules.GetAllAvailablePieceMovements(coordinateSystem, startCoordinate, PlayerColor.WHITE);

            // THEN
            Assert.HasCount(2, allAvailableMovements);
            foreach (var availableMovement in allAvailableMovements)
            {
                Assert.AreEqual(MovementValidationResult.VALID, queenMovementRules.ValidatePieceMovement(coordinateSystem, startCoordinate, availableMovement, PlayerColor.WHITE));
            }
        }

        [TestMethod]
        public void Given_CoordinateSystemWithPiecesInACircle_When_QueenMovementToCenterOfCircleValidated_Then_ReturnsValidationFail()
        {
            // GIVEN

            // Queen cannot move/slide into the center
            //
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (0, -1), (0, 0), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION, result);
        }

        [TestMethod]
        public void Given_CoordinateSystemWithPiecesInACircle_When_AllValidQueenMovementIsRetrieved_Then_ReturnsAllValid()
        {
            // GIVEN

            // Queen cannot move/slide into the center
            //
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
            var queenMovementRules = new QueenMovementRules();
            var startCoordinate = (0, -1);

            // WHEN
            var allAvailableMovements = queenMovementRules.GetAllAvailablePieceMovements(coordinateSystem, startCoordinate, PlayerColor.WHITE);

            // THEN
            Assert.HasCount(2, allAvailableMovements);
            foreach (var availableMovement in allAvailableMovements)
            {
                Assert.AreEqual(MovementValidationResult.VALID, queenMovementRules.ValidatePieceMovement(coordinateSystem, startCoordinate, availableMovement, PlayerColor.WHITE));
            }
        }

        [TestMethod]
        public void Given_CoordinateSystem_When_WrongColoredQueenMovementValidated_Then_ReturnsValidationFail()
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var result = queenMovementRules.ValidatePieceMovement(coordinateSystem, (1, -1), (1, -2), PlayerColor.WHITE);

            // THEN
            Assert.AreEqual(MovementValidationResult.WRONG_COLOR_MOVED, result);
        }

        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_AllValidQueenMovementForWrongColorIsRetrieved_Then_ReturnsEmpty()
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
            var queenMovementRules = new QueenMovementRules();

            // WHEN
            var allAvailableMovements = queenMovementRules.GetAllAvailablePieceMovements(coordinateSystem, (1, -1), PlayerColor.WHITE);

            // THEN
            Assert.HasCount(0, allAvailableMovements);
        }
    }
}
