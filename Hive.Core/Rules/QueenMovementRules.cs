using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class QueenMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate,
            PlayerColor playerTurnColor
            )
        {
            var commonMovementValidation = CommonMovementRules.ValidateCommonMovementRules<QueenPiece>(coordinateSystem,
                startCoordinate,
                destinationCoordinate,
                playerTurnColor
                );

            if (commonMovementValidation != MovementValidationResult.VALID)
            {
                return commonMovementValidation;
            }

            var allAdjacentCoordinatesForStart = coordinateSystem.GetAdjacentCoordinates(startCoordinate);
            if (!allAdjacentCoordinatesForStart.Contains(destinationCoordinate))
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
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

        public HashSet<(int column, int row)> GetAllAvailablePieceMovements(ICoordinateSystem coordinateSystem, (int column, int row) startCoordinate, PlayerColor playerTurnColor)
        {
            var allAdjacentCoordinates = coordinateSystem.GetAdjacentCoordinates(startCoordinate);
            var result = new HashSet<(int column, int row)>();
            foreach(var adjacentCoordinate in allAdjacentCoordinates)
            {
                if (ValidatePieceMovement(coordinateSystem, startCoordinate, adjacentCoordinate, playerTurnColor) == MovementValidationResult.VALID)
                {
                    result.Add(adjacentCoordinate);
                }
            }

            return result;
        }
    }
}
