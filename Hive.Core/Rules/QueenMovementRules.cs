using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class QueenMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) endCoordinate,
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
            if (!adjacentCoordinates.Contains(endCoordinate))
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }


            if (coordinateSystem.TryGetHexagonAtCoordinate(endCoordinate, out _))
            {
                return MovementValidationResult.DESTINATION_IS_NOT_EMPTY;
            }

            return MovementValidationResult.VALID;
        }
    }
}
