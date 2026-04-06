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

        public void AddHexagonToCoordinate(Hexagon hexagon, int column, int row) => _hexagonalGrid.Add((column, row), hexagon);

        public Hexagon? GetHexagonAtCoordinate(int column, int row)
        {
            _hexagonalGrid.TryGetValue((column, row), out Hexagon? hexagon);

            return hexagon;
        }

        public List<Hexagon?> GetPopulatedNeighborsForCoordinate(int column, int row)
        {
            var neighbors = new List<Hexagon?>();
            foreach (var direction in _adjacentDirections)
            {
                var neighborHexagon = GetHexagonAtCoordinate(direction.column + column, direction.row + row);
                if (neighborHexagon != null)
                {
                    neighbors.Add(neighborHexagon);
                }
            }

            return neighbors;
        }

        public List<(int column, int row)> GetAdjacentCoordinates(int column, int row)
        {
            var adjacentCoordinates = new List<(int column, int row)>();
            foreach (var direction in _adjacentDirections)
            {
                adjacentCoordinates.Add((direction.column + column, direction.row + row));
            }

            return adjacentCoordinates;
        }

        public void RemoveHexagonFromCoordinate(int column, int row)
        {
            var isRemoved = _hexagonalGrid.Remove((column, row));

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
