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

                var sharedFreeCoordinates = GetSharedFreeAdjacentsWithNeighbors(coordinateSystem, currentCoordinate, directContactNeighborHexagons);
                sharedFreeCoordinates.ExceptWith(visitedCoordinates);

                foreach (var sharedFreeCoordinate in sharedFreeCoordinates)
                {
                    nextStepAndCoordinates.Push((step + 1, sharedFreeCoordinate)); 
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
