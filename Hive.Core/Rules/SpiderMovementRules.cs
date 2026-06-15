using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class SpiderMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate,
            PlayerColor playerTurnColor,
            int turnNumber
            )
        {
            var currentCoordinate = startCoordinate;
            var visitedCoordinates = new HashSet<(int column, int row)>();
            var validDestinationCoordinates = new HashSet<(int column, int row)>();

            // ---

            for (var step = 1; step <= 3; step++)
            {
                visitedCoordinates.Add(currentCoordinate);
                var directContactNeighborHexagons = coordinateSystem.GetPopulatedNeighborCoordinates(currentCoordinate);
                directContactNeighborHexagons.ExceptWith(visitedCoordinates);
                var sharedFreeCoordinates = GetSharedFreeAdjacentsWithNeighbors(coordinateSystem, currentCoordinate, directContactNeighborHexagons);
                var nextStepDestinationCoordinate = sharedFreeCoordinates.First(); // TODO: also branch to other option
                currentCoordinate = nextStepDestinationCoordinate;

                if (step == 3)
                {
                    validDestinationCoordinates.Add(currentCoordinate);
                }
            }
 
            if (validDestinationCoordinates.Contains(destinationCoordinate))
            {
                return MovementValidationResult.VALID; 
            }

            return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION; 

            static HashSet<(int column, int row)> GetSharedFreeAdjacentsWithNeighbors(ICoordinateSystem coordinateSystem, (int column, int row) currentCoordinate, HashSet<(int column, int row)> directContactNeighborHexagons)
            {
                var sharedFreeCoordinates = new HashSet<(int column, int row)>();
                foreach (var directContactNeighbor in directContactNeighborHexagons)
                {
                    var freeAdjacents = coordinateSystem.GetSharedFreeAdjacentCoordinates(currentCoordinate, directContactNeighbor);

                    // TODO: a better way to add all
                    foreach (var freeAdjacent in freeAdjacents)
                    {
                        sharedFreeCoordinates.Add(freeAdjacent);
                    }
                }

                return sharedFreeCoordinates;
            }
        }
    }
}
