using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class SpiderMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate,
            PlayerColor playerTurnColor
            )
        {
            var commonMovementValidation = CommonMovementRules.ValidateCommonMovementRules<SpiderPiece>(coordinateSystem,
                startCoordinate,
                destinationCoordinate,
                playerTurnColor
                );

            if (commonMovementValidation != MovementValidationResult.VALID)
            {
                return commonMovementValidation;
            }

            var validDestinationCoordinates = GetAllAvailablePieceMovements(coordinateSystem, startCoordinate);
            if (!validDestinationCoordinates.Contains(destinationCoordinate))
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }

            return MovementValidationResult.VALID;
        }

        public HashSet<(int column, int row)> GetAllAvailablePieceMovements(ICoordinateSystem coordinateSystem, (int column, int row) startCoordinate)
        {
            var visitedCoordinates = new HashSet<(int column, int row)>();
            var validDestinationCoordinates = new HashSet<(int column, int row)>();
            var nextStepAndCoordinates = new Stack<(int step, (int column, int row))>();
            nextStepAndCoordinates.Push((0, startCoordinate));

            while (nextStepAndCoordinates.Count > 0)
            {
                var (step, currentCoordinate) = nextStepAndCoordinates.Pop();
                if (step == 3)
                {
                    validDestinationCoordinates.Add(currentCoordinate);
                    continue;
                }

                visitedCoordinates.Add(currentCoordinate);

                var directContactNeighborHexagons = coordinateSystem.GetPopulatedNeighborCoordinatesWithoutHexagon(currentCoordinate, startCoordinate);

                var sharedFreeCoordinates = GetSharedFreeAdjacentCoordinatesWithNeighbors(coordinateSystem, currentCoordinate, directContactNeighborHexagons);
                sharedFreeCoordinates.ExceptWith(visitedCoordinates);

                foreach (var sharedFreeCoordinate in sharedFreeCoordinates)
                {
                    var sharedPopulatedNeighborHexagons = coordinateSystem.GetSharedPopulatedNeighborHexagonsWithoutHexagon(sharedFreeCoordinate, currentCoordinate, startCoordinate);

                    // Two shared populated neighbors mean that the piece cannot slide into the space.
                    if (sharedPopulatedNeighborHexagons.Count != 2)
                    {
                        nextStepAndCoordinates.Push((step + 1, sharedFreeCoordinate));
                    }
                }
            }

            return validDestinationCoordinates;

            static HashSet<(int column, int row)> GetSharedFreeAdjacentCoordinatesWithNeighbors(ICoordinateSystem coordinateSystem, (int column, int row) currentCoordinate, HashSet<(int column, int row)> directContactNeighborHexagons)
            {
                var sharedFreeCoordinates = new HashSet<(int column, int row)>();
                foreach (var directContactNeighbor in directContactNeighborHexagons)
                {
                    var freeAdjacentCoordinates = coordinateSystem.GetSharedFreeAdjacentCoordinates(currentCoordinate, directContactNeighbor);

                    foreach (var freeAdjacent in freeAdjacentCoordinates)
                    {
                        sharedFreeCoordinates.Add(freeAdjacent);
                    }
                }

                return sharedFreeCoordinates;
            }
        }
    }
}
