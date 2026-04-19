namespace Hive.Core.Rules
{
    public enum SpawnValidationResult
    {
        VALID,
        WRONG_COLOR_PLACED,
        QUEEN_MUST_BE_PLACED_UNTIL_FOURTH_TURN,
        QUEEN_CANNOT_BE_PLACED_ON_FIRST_TURN,
        ANOTHER_PIECE_AT_DESTINATION,
        PIECE_MUST_TOUCH_THE_HIVE,
        PIECE_MUST_ONLY_TOUCH_FRIENDLY_PIECES
    }
}
