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
                return SpawnValidationResult.WRONG_COLOR_PLACED;
            }

            if (coordinates.Contains(spawnCoordinate))
            {
                return SpawnValidationResult.ANOTHER_PIECE_AT_DESTINATION;
            }

            if (turnNumber == 1 && piece is QueenPiece)
            {
                return SpawnValidationResult.QUEEN_CANNOT_BE_PLACED_ON_FIRST_TURN;
            }

            if (turnNumber == 4 && !RulesHelper.FindQueenForPlayerColor(coordinateSystem, playerTurnColor))
            {
                return SpawnValidationResult.QUEEN_WAS_NOT_PLACED_UNTIL_FOURTH_TURN;
            }

            var allAdjacentCoordinates = coordinateSystem.GetAllFreeAdjacentCoordinates();
            if (!allAdjacentCoordinates.Any(c => c == spawnCoordinate))
            {
                return SpawnValidationResult.PIECE_DID_NOT_TOUCH_THE_HIVE;
            }

            var allNeighborsForSpawnCoordinate = coordinateSystem.GetPopulatedNeighborHexagons(spawnCoordinate);
            if (turnNumber > 1 && allNeighborsForSpawnCoordinate.Any(n => n.GetColor() != playerTurnColor))
            {
                return SpawnValidationResult.PIECE_TOUCHED_ENEMY_PIECE;
            }

            // TODO: check whether the player has that piece in their inventory

            return SpawnValidationResult.VALID;
        }
    }
}
