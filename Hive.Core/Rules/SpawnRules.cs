using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class SpawnRules
    {
        private readonly PlayerColor _startingColor = PlayerColor.WHITE;

        public static SpawnValidationResult ValidatePieceSpawn(IPiece piece,
            ICoordinateSystem coordinateSystem,
            (int column, int row) spawnCoordinate
            )
        {
            var coordinates = coordinateSystem.GetAllCoordinates();
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
                var isQueenPlayedForColor = false;

                foreach (var coordinate in coordinates)
                {
                    coordinateSystem.TryGetHexagonAtCoordinate(coordinate, out var hexagon);

                    if (hexagon == null)
                    {
                        throw new Exception("Null hexagon encountered!");
                    }

                    if (hexagon.GetAllPieces().Any(p => p.Color == playerTurnColor && p.GetPieceName() == "Queen"))
                    {
                        isQueenPlayedForColor = true;
                        break;
                    }
                }

                if (!isQueenPlayedForColor)
                {
                    return SpawnValidationResult.QUEEN_SHOULD_BE_PLAYED;
                }
            }



            return SpawnValidationResult.VALID;
        }
    }
}
