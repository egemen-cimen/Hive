namespace Hive.Core.Rules
{
    public enum MovementValidationResult
    {
        VALID,
        NO_PIECE_TO_MOVE,
        WRONG_VALIDATOR_FOR_PIECE_TYPE,
        PIECE_CANNOT_REACH_DESTINATION,
        DESTINATION_IS_NOT_EMPTY,
        BREAKS_ONE_HIVE,
        START_AND_DESTINATION_CANNOT_BE_THE_SAME,
        WRONG_COLORED_PIECE,
        CANNOT_MOVE_WITHOUT_QUEEN
    }
}
