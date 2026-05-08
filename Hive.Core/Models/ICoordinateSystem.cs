namespace Hive.Core.Models
{
    public interface ICoordinateSystem
    {
        public void AddHexagon(Hexagon hexagon, (int column, int row) coordinate);

        public bool TryGetHexagon((int column, int row) coordinate, out Hexagon? hexagon);

        public List<Hexagon> GetPopulatedNeighborHexagons((int column, int row) coordinate);

        public HashSet<(int column, int row)> GetPopulatedNeighborCoordinates((int column, int row) coordinate);

        public List<Hexagon> GetSharedPopulatedNeighborHexagons((int column, int row) coordinate1, (int column, int row) coordinate2);

        public List<(int column, int row)> GetAdjacentCoordinates((int column, int row) coordinate);

        public HashSet<(int column, int row)> GetSharedFreeAdjacentCoordinates((int column, int row) coordinate1, (int column, int row) coordinate2);

        public void RemoveHexagon((int column, int row) coordinate);

        public HashSet<(int column, int row)> GetAllCoordinates();

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinates();

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinatesWithoutHexagon((int column, int row) coordinate);

        public bool VerifyWhetherAllHexagonsConnectedWithoutHexagon((int column, int row) coordinate);

    }
}
