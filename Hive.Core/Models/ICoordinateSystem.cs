namespace Hive.Core.Models
{
    public interface ICoordinateSystem
    {
        public void AddHexagonToCoordinate(Hexagon hexagon, (int column, int row) coordinate);

        public bool TryGetHexagonAtCoordinate((int column, int row) coordinate, out Hexagon? hexagon);

        public List<Hexagon> GetPopulatedNeighborsForCoordinate((int column, int row) coordinate);

        public List<(int column, int row)> GetAdjacentCoordinates((int column, int row) coordinate);

        public void RemoveHexagonFromCoordinate((int column, int row) coordinate);

        public List<(int column, int row)> GetListOfCoordinates();
    }
}
