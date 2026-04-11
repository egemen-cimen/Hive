namespace Hive.Core.Models
{
    public interface ICoordinateSystem
    {
        public void AddHexagonToCoordinate(Hexagon hexagon, (int column, int row) coordinate);

        public bool TryGetHexagonAtCoordinate((int column, int row) coordinate, out Hexagon? hexagon);

        public List<Hexagon> GetPopulatedNeighborHexagonsForCoordinate((int column, int row) coordinate);

        public List<(int column, int row)> GetAdjacentCoordinatesForCoordinate((int column, int row) coordinate);

        public void RemoveHexagonFromCoordinate((int column, int row) coordinate);

        public HashSet<(int column, int row)> GetListOfCoordinates();

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinates();

        public bool VerifyWhetherAllHexagonsConnectedWithoutHexagon((int column, int row) coordinate);

    }
}
