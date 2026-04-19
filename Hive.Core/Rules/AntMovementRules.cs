namespace Hive.Core.Rules
{
    public class AntMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement((int column, int row) startCoordinate, (int column, int row) endCoordinate)
        {
            throw new NotImplementedException();
        }
    }
}
