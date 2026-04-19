namespace Hive.Core.Rules
{
    public class QueenMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement((int column, int row) startCoordinate, (int column, int row) endCoordinate)
        {
            throw new NotImplementedException();
        }
    }
}
