using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public class RulesHelper
    {
        public static bool FindQueenForPlayerColor(ICoordinateSystem coordinateSystem, PlayerColor playerTurnColor)
        {
            var coordinates = coordinateSystem.GetAllCoordinates();
            var isQueenPlayedForColor = false;

            foreach (var coordinate in coordinates)
            {
                coordinateSystem.TryGetHexagonAtCoordinate(coordinate, out var hexagon);

                if (hexagon!.GetAllPieces().Any(p => p.Color == playerTurnColor && p is QueenPiece))
                {
                    isQueenPlayedForColor = true;
                    break;
                }
            }

            return isQueenPlayedForColor;
        }
    }
}
