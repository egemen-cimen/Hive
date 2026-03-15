namespace Hive.Core.Models
{
    public class AxialCoordinateSystem : ICoordinateSystem
    {
        private readonly Dictionary<Tuple<int, int>, Hexagon> hexagonalGrid;

        public AxialCoordinateSystem() => hexagonalGrid = [];

        public void AddHexagonToCoordinate(Hexagon hexagon, int column, int row) => hexagonalGrid.Add(new Tuple<int, int>(column, row), hexagon);
        
        public Hexagon? GetHexagonAtCoordinate(int column, int row)
        {
            hexagonalGrid.TryGetValue(new Tuple<int, int>(column, row), out Hexagon? hexagon);

            return hexagon;
        }

        public void RemoveHexagonFromCoordinate(int column, int row)
        {
            var isRemoved = hexagonalGrid.Remove(new Tuple<int, int>(column, row));

            if (!isRemoved)
            {
                throw new ArgumentException("There is no hexagon at the given coordinates.");
            }
        }
    }
}
