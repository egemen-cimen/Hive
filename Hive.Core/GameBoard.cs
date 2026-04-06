using Hive.Core.Models;

namespace Hive.Core
{
    public class GameBoard(ICoordinateSystem coordinateSystem)
    {
        private readonly ICoordinateSystem _coordinateSystem = coordinateSystem;

        public bool SpawnPiece((int column, int row) coordinate, IPiece piece)
        {
            var hexagonExists = _coordinateSystem.TryGetHexagonAtCoordinate(coordinate, out Hexagon? hexagonAtDestination);
            if (hexagonExists)
            {
                return false;
            }

            // TODO: rules

            var hexagon = new Hexagon();
            hexagon.PushPiece(piece);
            _coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);

            return true;
        }

        public bool MovePiece((int column, int row) startCoordinate, (int column, int row) endCoordinate)
        {
            // ...
            throw new NotImplementedException();

            return true;
        }

        public List<(int column, int row)> GetEmptySpacesOnTheBoard()
        {
            var emptySpaces = new List<(int column, int row)>();

            var listOfCoordinates = _coordinateSystem.GetListOfCoordinates();

            if (listOfCoordinates.Count == 0)
            {
                return [(0, 0)];
            }

            // TODO: refactor
            if (listOfCoordinates.Count == 1)
            {

            }

            return emptySpaces;
        }


    }
}
