using Hive.Core.Models;

namespace Hive.Core
{
    public class GameBoard(ICoordinateSystem coordinateSystem)
    {
        private readonly ICoordinateSystem CoordinateSystem = coordinateSystem;

        public bool SpawnPiece(int column, int row, IPiece piece)
        {
            var hexagonAtDestination = CoordinateSystem.GetHexagonAtCoordinate(column, row);
            if (hexagonAtDestination != null)
            {
                return false;
            }

            // TODO: rules

            var hexagon = new Hexagon();
            hexagon.PushPiece(piece);
            CoordinateSystem.AddHexagonToCoordinate(hexagon, column, row);

            return true;
        }

        public bool MovePiece(int startColumn, int startRow, int endColumn, int endRow)
        {
            // ...
            throw new NotImplementedException();

            return true;
        }

        public List<(int column, int row)> GetEmptySpacesOnTheBoard()
        {
            var emptySpaces = new List<(int column, int row)>();

            var listOfCoordinates = CoordinateSystem.GetListOfCoordinates();

            if (listOfCoordinates.Count == 0)
            {
                return [(0, 0)];
            }

            return emptySpaces;
        }


    }
}
