namespace Hive.Core.Models
{
    public class AxialCoordinateSystem : ICoordinateSystem
    {
        private readonly Dictionary<(int column, int row), Hexagon> _hexagonalGrid;
        private static readonly List<(int column, int row)> _adjacentDirections = [
            ( 0, -1),
            ( 1, -1),
            (-1,  0),
            ( 1,  0),
            (-1,  1),
            ( 0,  1)
            ];

        public AxialCoordinateSystem() => _hexagonalGrid = [];

        public void AddHexagonToCoordinate(Hexagon hexagon, (int column, int row) coordinate) => _hexagonalGrid.Add(coordinate, hexagon);

        public bool TryGetHexagonAtCoordinate((int column, int row) coordinate, out Hexagon? hexagon)
        {
            var hexagonExists = _hexagonalGrid.TryGetValue(coordinate, out hexagon);

            return hexagonExists;
        }

        public List<Hexagon> GetPopulatedNeighborsForCoordinate((int column, int row) coordinate)
        {
            var neighbors = new List<Hexagon>();
            foreach (var (directionColumn, directionRow) in _adjacentDirections)
            {
                TryGetHexagonAtCoordinate((directionColumn + coordinate.column, directionRow + coordinate.row), out Hexagon? neighborHexagon);
                if (neighborHexagon != null)
                {
                    neighbors.Add(neighborHexagon);
                }
            }

            return neighbors;
        }

        public List<(int column, int row)> GetAdjacentCoordinates((int column, int row) coordinate)
        {
            var adjacentCoordinates = new List<(int column, int row)>();
            foreach (var (directionColumn, directionRow) in _adjacentDirections)
            {
                adjacentCoordinates.Add((directionColumn + coordinate.column, directionRow + coordinate.row));
            }

            return adjacentCoordinates;
        }

        public void RemoveHexagonFromCoordinate((int column, int row) coordinate)
        {
            var isRemoved = _hexagonalGrid.Remove(coordinate);

            if (!isRemoved)
            {
                throw new ArgumentException("There is no hexagon at the given coordinates.");
            }
        }

        public List<(int column, int row)> GetListOfCoordinates()
        {
            return [.. _hexagonalGrid.Keys];
        }
    }
}
