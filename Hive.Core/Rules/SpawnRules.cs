using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class SpawnRules
    {
        public static SpawnValidationResult ValidatePieceSpawn(IPiece piece,
            ICoordinateSystem coordinateSystem,
            (int column, int row) spawnCoordinate
            )
        {
            var coordinates = coordinateSystem.GetAllCoordinates();

            // White is the starting color
            var playerTurnColor = coordinates.Count % 2 == 0 ? PlayerColor.WHITE : PlayerColor.BLACK;

            int turnNumber = coordinates.Count / 2 + 1;

            if (piece.Color != playerTurnColor)
            {
                return SpawnValidationResult.WRONG_COLOR_PLAYED;
            }

            if (coordinates.Contains(spawnCoordinate))
            {
                return SpawnValidationResult.ANOTHER_PIECE_AT_DESTINATION;
            }

            if (turnNumber == 4)
            {
                bool isQueenPlayedForColor = FindQueenForPlayerColor(coordinateSystem, playerTurnColor);
                if (!isQueenPlayedForColor)
                {
                    return SpawnValidationResult.QUEEN_SHOULD_BE_PLAYED;
                }
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

        private static bool FindQueenForPlayerColor(ICoordinateSystem coordinateSystem, PlayerColor playerTurnColor)
        {
            var coordinates = coordinateSystem.GetAllCoordinates();
            var isQueenPlayedForColor = false;

            foreach (var coordinate in coordinates)
            {
                coordinateSystem.TryGetHexagonAtCoordinate(coordinate, out var hexagon);

                if (hexagon == null)
                {
                    throw new Exception("Null hexagon encountered!");
                }

                if (hexagon.GetAllPieces().Any(p => p.Color == playerTurnColor && p.GetPieceName() == QueenPiece.Name))
                {
                    isQueenPlayedForColor = true;
                    break;
                }
            }

            return isQueenPlayedForColor;
        }
    }
}
