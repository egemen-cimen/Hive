namespace Hive.Core.Models
{
    public class AxialCoordinateSystem : ICoordinateSystem
    {
        private readonly Dictionary<(int column, int row), Hexagon> hexagonalGrid;

        public AxialCoordinateSystem() => hexagonalGrid = [];

        public void AddHexagonToCoordinate(Hexagon hexagon, int column, int row) => hexagonalGrid.Add((column, row), hexagon);

        public Hexagon? GetHexagonAtCoordinate(int column, int row)
        {
            hexagonalGrid.TryGetValue((column, row), out Hexagon? hexagon);

            return hexagon;
        }

        public void RemoveHexagonFromCoordinate(int column, int row)
        {
            var isRemoved = hexagonalGrid.Remove((column, row));

            if (!isRemoved)
            {
                throw new ArgumentException("There is no hexagon at the given coordinates.");
            }
        }

        public List<(int column, int row)> GetListOfCoordinates()
        {
            return [.. hexagonalGrid.Keys];
        }
    }
}
