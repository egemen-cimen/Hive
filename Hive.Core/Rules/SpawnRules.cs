using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class SpawnRules
    {
        private static readonly Dictionary<Type, int> AVAILABLE_PIECE_COUNTS = new()
        {
            [typeof(QueenPiece)] = 1,
            [typeof(SpiderPiece)] = 2,
            [typeof(BeetlePiece)] = 2,
            [typeof(GrasshopperPiece)] = 3,
            [typeof(AntPiece)] = 3,
        };

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

            if (turnNumber == 4 && !RulesHelper.VerifyWhetherQueenIsSpawned(coordinateSystem, playerTurnColor))
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

            // Check whether the player has that piece in their inventory.
            var allPlayerPieces = CountSpawnedPlayerPieces(coordinateSystem, playerTurnColor);
            if (allPlayerPieces.TryGetValue(piece.GetType(), out int pieceCount))
            {
                if (pieceCount + 1 > AVAILABLE_PIECE_COUNTS[piece.GetType()])
                {
                    return SpawnValidationResult.MORE_THAN_AVAILABLE_PIECES_SPAWNED;
                }
            }

            return SpawnValidationResult.VALID;

            static Dictionary<Type, int> CountSpawnedPlayerPieces(ICoordinateSystem coordinateSystem, PlayerColor playerTurnColor)
            {
                var allPlayerPieces = new Dictionary<Type, int>();

                var allCoordinates = coordinateSystem.GetAllCoordinates();
                foreach (var populatedCoordinate in allCoordinates)
                {
                    coordinateSystem.TryGetHexagon(populatedCoordinate, out var hexagon);

                    var allHexagonPieces = hexagon!.GetAllPieces();
                    foreach (var piece in allHexagonPieces)
                    {
                        if (piece.Color != playerTurnColor)
                        {
                            continue;
                        }

                        if (allPlayerPieces.TryGetValue(piece.GetType(), out var count))
                        {
                            allPlayerPieces[piece.GetType()] = count + 1;
                        }
                        else
                        {
                            allPlayerPieces[piece.GetType()] = 1;
                        }
                    }
                }

                return allPlayerPieces;
            }
        }
    }
}
