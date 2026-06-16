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
            if (startCoordinate == destinationCoordinate)
            {
                return MovementValidationResult.START_AND_DESTINATION_CANNOT_BE_THE_SAME;
            }

            var isStartHexagonExists = coordinateSystem.TryGetHexagon(startCoordinate, out var hexagonAtStart);
            if (!isStartHexagonExists)
            {
                return MovementValidationResult.NO_PIECE_TO_MOVE;
            }

            var topPiece = hexagonAtStart!.PeekPiece();
            if (topPiece is not QueenPiece)
            {
                return MovementValidationResult.WRONG_VALIDATOR_FOR_PIECE_TYPE;
            }

            if (topPiece.Color != playerTurnColor)
            {
                return MovementValidationResult.WRONG_COLORED_PIECE;
            }

            var allAdjacentCoordinatesForStart = coordinateSystem.GetAdjacentCoordinates(startCoordinate);
            if (!allAdjacentCoordinatesForStart.Contains(destinationCoordinate))
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }

            if (coordinateSystem.TryGetHexagon(destinationCoordinate, out _))
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

            var sharedPopulatedNeighborHexagons = coordinateSystem.GetSharedPopulatedNeighborHexagons(startCoordinate, destinationCoordinate);
            if (sharedPopulatedNeighborHexagons.Count == 0)
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }

            // Two shared populated neighbors mean that the piece cannot slide into the space.
            if (sharedPopulatedNeighborHexagons.Count == 2)
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }

            return MovementValidationResult.VALID;
        }
    }
}
