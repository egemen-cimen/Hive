namespace Hive.Core.Models
{
    public interface ICoordinateSystem
    {
        /// <summary>
        /// Adds a hexagon to the given coordinate.
        /// </summary>
        /// <param name="hexagon"></param>
        /// <param name="coordinate"></param>
        public void AddHexagon(Hexagon hexagon, (int column, int row) coordinate);

        /// <summary>
        /// Gets the hexagon located on the given coordinate.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="hexagon"></param>
        /// <returns>true if the coordinate contains a hexagon; otherwise, false</returns>
        public bool TryGetHexagon((int column, int row) coordinate, out Hexagon? hexagon);

        /// <summary>
        /// Gets only the neighbor hexagons of the given coordinate.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>List of populated neighbor hexagons</returns>
        public List<Hexagon> GetPopulatedNeighborHexagons((int column, int row) coordinate);

        /// <summary>
        /// Gets only the coordinates for the neighbor hexagons of the given coordinate.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>HashSet of populated neighbor coordinates</returns>
        public HashSet<(int column, int row)> GetPopulatedNeighborCoordinates((int column, int row) coordinate);

        public List<Hexagon> GetSharedPopulatedNeighborHexagons((int column, int row) coordinate1, (int column, int row) coordinate2);

        public HashSet<(int column, int row)> GetAdjacentCoordinates((int column, int row) coordinate);

        public HashSet<(int column, int row)> GetSharedFreeAdjacentCoordinates((int column, int row) coordinate1, (int column, int row) coordinate2);

        public void RemoveHexagon((int column, int row) coordinate);

        public HashSet<(int column, int row)> GetAllCoordinates();

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinates();

        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinatesWithoutHexagon((int column, int row) coordinate);

        public bool VerifyWhetherAllHexagonsConnectedWithoutHexagon((int column, int row) coordinate);

    }
}
