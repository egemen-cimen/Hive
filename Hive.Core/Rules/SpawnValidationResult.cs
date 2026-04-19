namespace Hive.Core.Rules
{
    public enum SpawnValidationResult
    {
        VALID,
        WRONG_COLOR_PLAYED,
        QUEEN_SHOULD_BE_PLAYED,
        ANOTHER_PIECE_AT_DESTINATION,
        PIECE_MUST_TOUCH_THE_HIVE,
        PIECE_MUST_ONLY_TOUCH_FRIENDLY_PIECES
    }
}
