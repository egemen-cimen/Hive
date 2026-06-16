using Hive.Core.Models;

namespace Hive.Core.Tests.Rules
{
    internal class CoordinateSystemPopulationHelper
    {
        internal static AxialCoordinateSystem CreatePopulatedCoordinateSystem(List<((int column, int row) coordinate, Type pieceType)> exampleMoves)
        {
            var coordinateSystem = new AxialCoordinateSystem();
            var currentColor = PlayerColor.WHITE;

            foreach (var (coordinate, pieceType) in exampleMoves)
            {
                SpawnPiece(coordinateSystem, coordinate, pieceType, currentColor);
                currentColor = currentColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;
            }

            return coordinateSystem;
        }

        internal static void SpawnPiece(AxialCoordinateSystem coordinateSystem, (int column, int row) coordinate, Type pieceType, PlayerColor pieceColor)
        {
            var hexagon = new Hexagon();

            if (pieceType == typeof(AntPiece))
            {
                hexagon.PushPiece(new AntPiece(pieceColor));
            }
            else if (pieceType == typeof(BeetlePiece))
            {
                hexagon.PushPiece(new BeetlePiece(pieceColor));
            }
            else if (pieceType == typeof(GrasshopperPiece))
            {
                hexagon.PushPiece(new GrasshopperPiece(pieceColor));
            }
            else if (pieceType == typeof(QueenPiece))
            {
                hexagon.PushPiece(new QueenPiece(pieceColor));
            }
            else if (pieceType == typeof(SpiderPiece))
            {
                hexagon.PushPiece(new SpiderPiece(pieceColor));
            }

            coordinateSystem.AddHexagon(hexagon, coordinate);
        }
    }
}
