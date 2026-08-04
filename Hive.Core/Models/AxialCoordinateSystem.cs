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

        public void AddHexagon(Hexagon hexagon, (int column, int row) coordinate)
            => _hexagonalGrid.Add(coordinate, hexagon);

        public bool TryGetHexagon((int column, int row) coordinate, out Hexagon? hexagon)
            => _hexagonalGrid.TryGetValue(coordinate, out hexagon);

        public Hexagon GetHexagonAtCoordinate((int column, int row) coordinate)
        {
            var hexagon = _hexagonalGrid.GetValueOrDefault(coordinate)
                ?? throw new ArgumentException("There is no hexagon at the given coordinates.");

            return hexagon;
        }

        public List<Hexagon> GetPopulatedNeighborHexagons((int column, int row) coordinate)
        {
            var neighbors = new List<Hexagon>();
            foreach (var direction in _adjacentDirections)
            {
                TryGetHexagon(SumTwoCoordinates(direction, coordinate), out Hexagon? neighborHexagon);
                if (neighborHexagon != null)
                {
                    neighbors.Add(neighborHexagon);
                }
            }

            return neighbors;
        }

        public HashSet<(int column, int row)> GetPopulatedNeighborCoordinates((int column, int row) coordinate)
        {
            var populatedNeighborCoordinates = new HashSet<(int column, int row)>();

            foreach (var direction in _adjacentDirections)
            {
                var coordinateSum = SumTwoCoordinates(direction, coordinate);
                TryGetHexagon(coordinateSum, out Hexagon? neighborHexagon);
                if (neighborHexagon != null)
                {
                    populatedNeighborCoordinates.Add(coordinateSum);
                }
            }

            return populatedNeighborCoordinates;
        }

        public List<Hexagon> GetSharedPopulatedNeighborHexagons((int column, int row) coordinate1, (int column, int row) coordinate2)
        {
            var sharedNeighbors = new List<Hexagon>();
            var allAdjacentCoordinatesForCoordinate1 = GetAdjacentCoordinates(coordinate1);
            var allAdjacentCoordinatesForCoordinate2 = GetAdjacentCoordinates(coordinate2);

            foreach (var commonAdjacentCoordinate in allAdjacentCoordinatesForCoordinate1.Intersect(allAdjacentCoordinatesForCoordinate2))
            {
                if (TryGetHexagon(commonAdjacentCoordinate, out var sharedNeighbor))
                {
                    sharedNeighbors.Add(sharedNeighbor!);
                }
            }

            return sharedNeighbors;
        }

        public HashSet<(int column, int row)> GetAdjacentCoordinates((int column, int row) coordinate)
        {
            var adjacentCoordinates = new HashSet<(int column, int row)>();
            foreach (var direction in _adjacentDirections)
            {
                adjacentCoordinates.Add(SumTwoCoordinates(direction, coordinate));
            }

            return adjacentCoordinates;
        }

        public HashSet<(int column, int row)> GetSharedFreeAdjacentCoordinates((int column, int row) coordinate1, (int column, int row) coordinate2)
        {
            var freeAdjacentCoordinates1 = GetFreeAdjacentCoordinates(coordinate1);
            var freeAdjacentCoordinates2 = GetFreeAdjacentCoordinates(coordinate2);

            return [.. freeAdjacentCoordinates1.Intersect(freeAdjacentCoordinates2)];
        }

        private HashSet<(int column, int row)> GetFreeAdjacentCoordinates((int column, int row) coordinate)
        {
            var adjacentCoordinates = new HashSet<(int column, int row)>();

            foreach (var direction in _adjacentDirections)
            {
                var adjacentCoordinate = SumTwoCoordinates(direction, coordinate);

                if (!TryGetHexagon(adjacentCoordinate, out _))
                {
                    adjacentCoordinates.Add(adjacentCoordinate);
                }
            }

            return adjacentCoordinates;
        }

        public void RemoveHexagon((int column, int row) coordinate)
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
                    if (TryGetHexagon(coordinateSummation, out _))
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

            RemoveHexagon(coordinate);

            var result = GetAllFreeAdjacentCoordinates();

            AddHexagon(hexagon, coordinate);

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
                    if (TryGetHexagon(coordinateSummation, out _)
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

            RemoveHexagon(coordinate);

            var result = VerifyWhetherAllHexagonsConnected();

            AddHexagon(hexagon, coordinate);

            return result;
        }

        public HashSet<(int column, int row)> GetPopulatedNeighborCoordinatesWithoutHexagon((int column, int row) coordinate, (int column, int row) hexagonCoordinate)
        {
            var hexagon = GetHexagonAtCoordinate(hexagonCoordinate);

            RemoveHexagon(hexagonCoordinate);

            var result = GetPopulatedNeighborCoordinates(coordinate);

            AddHexagon(hexagon, hexagonCoordinate);

            return result;
        }
    }
}
