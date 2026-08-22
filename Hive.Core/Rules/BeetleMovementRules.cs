using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class BeetleMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate,
            PlayerColor playerTurnColor
            )
        {
            var commonMovementValidation = CommonMovementRules.ValidateCommonMovementRules<BeetlePiece>(coordinateSystem,
                startCoordinate,
                destinationCoordinate,
                playerTurnColor
                );

            if (commonMovementValidation != MovementValidationResult.VALID)
            {
                return commonMovementValidation;
            }

            coordinateSystem.TryGetHexagon(startCoordinate, out var startHexagon);
            // Subtract beetle's height for the start height.
            var startHeight = startHexagon!.GetPieceCount() - 1;

            var destinationHeight = 0;
            var destinationHexagonExists = false;
            if (coordinateSystem.TryGetHexagon(destinationCoordinate, out var destinationHexagon))
            {
                destinationHexagonExists = true;
                destinationHeight = destinationHexagon!.GetPieceCount();
            }

            // One-hive rule is only broken if the beetle is on the first floor.
            if (startHeight == 0)
            {
                if (!coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(startCoordinate))
                {
                    return MovementValidationResult.BREAKS_ONE_HIVE;
                }

                var allFreeAdjacentCoordinatesWithout = coordinateSystem.GetAllFreeAdjacentCoordinatesWithoutHexagon(startCoordinate);
                if (!destinationHexagonExists && !allFreeAdjacentCoordinatesWithout.Contains(destinationCoordinate))
                {
                    return MovementValidationResult.BREAKS_ONE_HIVE;
                }
            }

            var allAdjacentCoordinatesForStart = coordinateSystem.GetAdjacentCoordinates(startCoordinate);
            if (!allAdjacentCoordinatesForStart.Contains(destinationCoordinate))
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }

            var firstNeighborHeight = 0;
            var secondNeighborHeight = 0;
            var sharedPopulatedNeighborHexagons = coordinateSystem.GetSharedPopulatedNeighborHexagons(startCoordinate, destinationCoordinate);
            if (sharedPopulatedNeighborHexagons.Count == 1)
            {
                firstNeighborHeight = sharedPopulatedNeighborHexagons[0].GetPieceCount();
            }
            else if (sharedPopulatedNeighborHexagons.Count == 2)
            {
                firstNeighborHeight = sharedPopulatedNeighborHexagons[0].GetPieceCount();
                secondNeighborHeight = sharedPopulatedNeighborHexagons[1].GetPieceCount();
            }

            if (startHeight == 0 &&
                destinationHeight == 0 &&
                firstNeighborHeight == 0 &&
                secondNeighborHeight == 0
                )
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }

            if (firstNeighborHeight > startHeight &&
                secondNeighborHeight > startHeight &&
                firstNeighborHeight > destinationHeight &&
                secondNeighborHeight > destinationHeight
                )
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
