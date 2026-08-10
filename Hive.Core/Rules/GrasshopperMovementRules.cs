using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class GrasshopperMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate,
            PlayerColor playerTurnColor
            )
        {
            var commonMovementValidation = CommonMovementRules.ValidateCommonMovementRules<GrasshopperPiece>(coordinateSystem,
                startCoordinate,
                destinationCoordinate,
                playerTurnColor
                );

            if (commonMovementValidation != MovementValidationResult.VALID)
            {
                return commonMovementValidation;
            }

            var validDestinationCoordinated = GetAllAvailablePieceMovements(coordinateSystem, startCoordinate);

            if (!validDestinationCoordinated.Contains(destinationCoordinate))
            {
                return MovementValidationResult.PIECE_CANNOT_REACH_DESTINATION;
            }

            return MovementValidationResult.VALID;
        }

        public HashSet<(int column, int row)> GetAllAvailablePieceMovements(ICoordinateSystem coordinateSystem, (int column, int row) startCoordinate)
        {
            var populatedNeighborCoordinates = coordinateSystem.GetPopulatedNeighborCoordinates(startCoordinate);
            var validDestinationCoordinates = new HashSet<(int column, int row)>();

            foreach (var populatedNeighborCoordinate in populatedNeighborCoordinates)
            {
                var direction = SubtractCoordinates(populatedNeighborCoordinate, startCoordinate);

                var nextCoordinate = SumCoordinates(startCoordinate, direction);

                while (coordinateSystem.TryGetHexagon(nextCoordinate, out _))
                {
                    nextCoordinate = SumCoordinates(nextCoordinate, direction);
                }

                validDestinationCoordinates.Add(nextCoordinate);
            }

            return validDestinationCoordinates;
        }

        static (int column, int row) SumCoordinates((int column, int row) coordinate1, (int column, int row) coordinate2)
        {
            return (coordinate1.column + coordinate2.column, coordinate1.row + coordinate2.row);
        }

        static (int column, int row) SubtractCoordinates((int column, int row) coordinate1, (int column, int row) coordinate2)
        {
            return (coordinate1.column - coordinate2.column, coordinate1.row - coordinate2.row);
        }
    }
}
