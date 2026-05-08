using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public interface IMovementRules
    {
        MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem, 
            (int column, int row) startCoordinate, (int column, int row) destinationCoordinate, PlayerColor playerTurnColor, int turnNumber);
    }
}
