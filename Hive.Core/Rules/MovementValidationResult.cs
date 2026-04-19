namespace Hive.Core.Rules
{
    public enum MovementValidationResult
    {
        VALID,
        THERE_MUST_BE_A_PIECE_TO_MOVE,
        WRONG_VALIDATOR_FOR_PIECE_TYPE,
        PIECE_CANNOT_REACH_DESTINATION,
        DESTINATION_MUST_BE_EMPTY
    }
}
