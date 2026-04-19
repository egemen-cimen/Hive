namespace Hive.Core.Rules
{
    public class SpiderMovementRules : IMovementRules
    {
        public MovementValidationResult ValidatePieceMovement((int column, int row) startCoordinate, (int column, int row) endCoordinate)
        {
            throw new NotImplementedException();
        }
    }
}
