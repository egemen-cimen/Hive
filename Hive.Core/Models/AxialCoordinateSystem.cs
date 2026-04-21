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
            ( 0,  1)];

        public AxialCoordinateSystem() => _hexagonalGrid = [];

        public void AddHexagonToCoordinate(Hexagon hexagon, (int column, int row) coordinate)
            => _hexagonalGrid.Add(coordinate, hexagon);

        public bool TryGetHexagonAtCoordinate((int column, int row) coordinate, out Hexagon? hexagon)
            => _hexagonalGrid.TryGetValue(coordinate, out hexagon);

        public Hexagon GetHexagonAtCoordinate((int column, int row) coordinate)
        {
            var hexagon = _hexagonalGrid.GetValueOrDefault(coordinate)
                ?? throw new ArgumentException("There is no hexagon at the given coordinates.");

            return hexagon;
        }

        public List<Hexagon> GetPopulatedNeighborHexagonsForCoordinate((int column, int row) coordinate)
        {
            var neighbors = new List<Hexagon>();
            foreach (var direction in _adjacentDirections)
            {
                TryGetHexagonAtCoordinate(SumTwoCoordinates(direction, coordinate), out Hexagon? neighborHexagon);
                if (neighborHexagon != null)
                {
                    neighbors.Add(neighborHexagon);
                }
            }

            return neighbors;
        }

        public List<Hexagon> GetSharedPopulatedNeighborHexagonsForCoordinates((int column, int row) coordinate1, (int column, int row) coordinate2)
        {
            var sharedNeighbors = new List<Hexagon>();
            var allAdjacentCoordinatesForCoordinate1 = GetAdjacentCoordinatesForCoordinate(coordinate1);
            var allAdjacentCoordinatesForCoordinate2 = GetAdjacentCoordinatesForCoordinate(coordinate2);

            foreach (var commonAdjacentCoordinate in allAdjacentCoordinatesForCoordinate1.Intersect(allAdjacentCoordinatesForCoordinate2))
            {
                if (TryGetHexagonAtCoordinate(commonAdjacentCoordinate, out var sharedNeighbor))
                {
                    sharedNeighbors.Add(sharedNeighbor!);
                }
            }

            return sharedNeighbors;
        }

        public List<(int column, int row)> GetAdjacentCoordinatesForCoordinate((int column, int row) coordinate)
        {
            var adjacentCoordinates = new List<(int column, int row)>();
            foreach (var direction in _adjacentDirections)
            {
                adjacentCoordinates.Add(SumTwoCoordinates(direction, coordinate));
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

        public HashSet<(int column, int row)> GetAllCoordinates() => [.. _hexagonalGrid.Keys];

        private static (int column, int row) SumTwoCoordinates((int column, int row) coordinate1,
                                                               (int column, int row) coordinate2)
            => (coordinate1.column + coordinate2.column, coordinate1.row + coordinate2.row);

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinates()
        {
            if (_hexagonalGrid.Count == 0)
            {
                return [(0, 0)];
            }

            var freeAdjacentCoordinates = new HashSet<(int column, int row)>();

            var visitedCoordinates = new HashSet<(int column, int row)>();
            var coordinatesToVisit = new Stack<(int column, int row)>();

            coordinatesToVisit.Push(_hexagonalGrid.First().Key);

            while (coordinatesToVisit.Count > 0)
            {
                var currentCoordinate = coordinatesToVisit.Pop();
                visitedCoordinates.Add(currentCoordinate);

                foreach (var direction in _adjacentDirections)
                {
                    var coordinateSummation = SumTwoCoordinates(direction, currentCoordinate);
                    if (TryGetHexagonAtCoordinate(coordinateSummation, out _))
                    {
                        if (!visitedCoordinates.TryGetValue(coordinateSummation, out _))
                        {
                            coordinatesToVisit.Push(coordinateSummation);
                        }
                    }
                    else
                    {
                        freeAdjacentCoordinates.Add(coordinateSummation);
                    }
                }
            }

            if (visitedCoordinates.Count != _hexagonalGrid.Count)
            {
                throw new InvalidOperationException(System.Reflection.MethodBase.GetCurrentMethod() +
                    "was unable to reach all coordinates.");
            }

            return freeAdjacentCoordinates;
        }

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinatesWithoutHexagon((int column, int row) coordinate)
        {
            var hexagon = GetHexagonAtCoordinate(coordinate);

            RemoveHexagonFromCoordinate(coordinate);

            var result = GetAllFreeAdjacentCoordinates();

            AddHexagonToCoordinate(hexagon, coordinate);

            return result;
        }

        private bool VerifyWhetherAllHexagonsConnected()
        {
            if (_hexagonalGrid.Count == 0)
            {
                return true;
            }

            var visitedCoordinates = new HashSet<(int column, int row)>();
            var coordinatesToVisit = new Stack<(int column, int row)>();

            coordinatesToVisit.Push(_hexagonalGrid.First().Key);

            while (coordinatesToVisit.Count > 0)
            {
                var currentCoordinate = coordinatesToVisit.Pop();
                visitedCoordinates.Add(currentCoordinate);

                foreach (var direction in _adjacentDirections)
                {
                    var coordinateSummation = SumTwoCoordinates(direction, currentCoordinate);
                    if (TryGetHexagonAtCoordinate(coordinateSummation, out _)
                        && !visitedCoordinates.TryGetValue(coordinateSummation, out _))
                    {
                        coordinatesToVisit.Push(coordinateSummation);
                    }
                }
            }

            if (visitedCoordinates.Count != _hexagonalGrid.Count)
            {
                return false;
            }

            return true;
        }

        public bool VerifyWhetherAllHexagonsConnectedWithoutHexagon((int column, int row) coordinate)
        {
            var hexagon = GetHexagonAtCoordinate(coordinate);

            RemoveHexagonFromCoordinate(coordinate);

            var result = VerifyWhetherAllHexagonsConnected();

            AddHexagonToCoordinate(hexagon, coordinate);

            return result;
        }
    }
}
