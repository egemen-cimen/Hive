using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public static class RulesHelper
    {
        public static (bool queenExists, (int column, int row)? coordinate) VerifyWhetherQueenIsSpawned(ICoordinateSystem coordinateSystem, PlayerColor playerTurnColor)
        {
            var allCoordinates = coordinateSystem.GetAllCoordinates();
            foreach (var populatedCoordinate in allCoordinates)
            {
                coordinateSystem.TryGetHexagon(populatedCoordinate, out var hexagon);
                if (hexagon!.GetAllPieces().Any(p => p.Color == playerTurnColor && p is QueenPiece))
                {
                    return (true, populatedCoordinate);
                }
            }

            return (false, null);
        }
    }
}
