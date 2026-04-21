namespace Hive.Core.Models
{
    public interface ICoordinateSystem
    {
        public void AddHexagonToCoordinate(Hexagon hexagon, (int column, int row) coordinate);

        public bool TryGetHexagonAtCoordinate((int column, int row) coordinate, out Hexagon? hexagon);

        public List<Hexagon> GetPopulatedNeighborHexagonsForCoordinate((int column, int row) coordinate);

        public List<Hexagon> GetSharedPopulatedNeighborHexagonsForCoordinates((int column, int row) coordinate1, (int column, int row) coordinate2);

        public List<(int column, int row)> GetAdjacentCoordinatesForCoordinate((int column, int row) coordinate);

        public void RemoveHexagonFromCoordinate((int column, int row) coordinate);

        public HashSet<(int column, int row)> GetAllCoordinates();

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinates();

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinatesWithoutHexagon((int column, int row) coordinate);

        public bool VerifyWhetherAllHexagonsConnectedWithoutHexagon((int column, int row) coordinate);

    }
}
