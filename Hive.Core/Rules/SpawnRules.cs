using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class SpawnRules
    {
        public static SpawnValidationResult ValidatePieceSpawn(IPiece piece,
            ICoordinateSystem coordinateSystem,
            (int column, int row) spawnCoordinate,
            PlayerColor playerTurnColor,
            int turnNumber
            )
        {
            var coordinates = coordinateSystem.GetAllCoordinates();

            if (piece.Color != playerTurnColor)
            {
                return SpawnValidationResult.WRONG_COLOR_PLAYED;
            }

            if (coordinates.Contains(spawnCoordinate))
            {
                return SpawnValidationResult.ANOTHER_PIECE_AT_DESTINATION;
            }

            if (turnNumber == 4 && !RulesHelper.FindQueenForPlayerColor(coordinateSystem, playerTurnColor))
            {
                return SpawnValidationResult.QUEEN_SHOULD_BE_PLAYED;
            }

            var allAdjacentCoordinates = coordinateSystem.GetAllFreeAdjacentCoordinates();
            if (!allAdjacentCoordinates.Any(c => c == spawnCoordinate))
            {
                return SpawnValidationResult.PIECE_MUST_TOUCH_THE_HIVE;
            }

            var allNeighborsForSpawnCoordinate = coordinateSystem.GetPopulatedNeighborHexagonsForCoordinate(spawnCoordinate);
            if (turnNumber > 1 && allNeighborsForSpawnCoordinate.Any(n => n.GetColor() != playerTurnColor))
            {
                return SpawnValidationResult.PIECE_MUST_ONLY_TOUCH_FRIENDLY_PIECES;
            }

            return SpawnValidationResult.VALID;
        }
    }
}
