using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class QueenMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate,
            PlayerColor playerTurnColor,
            int turnNumber
            )
        {
            var isStartHexagonExists = coordinateSystem.TryGetHexagonAtCoordinate(startCoordinate, out var hexagonAtStart);
            if (!isStartHexagonExists)
            {
                return MovementValidationResult.NO_PIECE_TO_MOVE;
            }

            if (hexagonAtStart!.PeekPiece() is not QueenPiece)
            {
                return MovementValidationResult.WRONG_VALIDATOR_FOR_PIECE_TYPE;
            }

            var adjacentCoordinates = coordinateSystem.GetAdjacentCoordinatesForCoordinate(startCoordinate);
            if (!adjacentCoordinates.Contains(destinationCoordinate))
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }

            if (coordinateSystem.TryGetHexagonAtCoordinate(destinationCoordinate, out _))
            {
                return MovementValidationResult.DESTINATION_IS_NOT_EMPTY;
            }

            if (!coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(startCoordinate))
            {
                return MovementValidationResult.BREAKS_ONE_HIVE;
            }

            var allFreeAdjacentCoordinatesWithout = coordinateSystem.GetAllFreeAdjacentCoordinatesWithoutHexagon(startCoordinate);
            if (!allFreeAdjacentCoordinatesWithout.Contains(destinationCoordinate))
            {
                return MovementValidationResult.BREAKS_ONE_HIVE;
            }

            return MovementValidationResult.VALID;
        }
    }
}
