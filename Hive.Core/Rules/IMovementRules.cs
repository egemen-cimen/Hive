namespace Hive.Core.Rules
{
    public interface IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement((int column, int row) startCoordinate, (int column, int row) endCoordinate);
    }
}
