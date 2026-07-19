using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public static class RulesHelper
    {
        public static bool VerifyWhetherQueenIsSpawned(ICoordinateSystem coordinateSystem, PlayerColor playerTurnColor)
        {
            var queenExists = false;
            var allCoordinates = coordinateSystem.GetAllCoordinates();
            foreach (var populatedCoordinate in allCoordinates)
            {
                coordinateSystem.TryGetHexagon(populatedCoordinate, out var hexagon);
                queenExists = hexagon!.GetAllPieces().Any(p => p.Color == playerTurnColor && p is QueenPiece);
                if (queenExists)
                {
                    break;
                }
            }

            return queenExists;
        }
    }
}
