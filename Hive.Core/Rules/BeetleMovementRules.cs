using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class BeetleMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) endCoordinate,
            PlayerColor playerTurnColor,
            int turnNumber
            )
        {
            throw new NotImplementedException();


        }
    }
}
