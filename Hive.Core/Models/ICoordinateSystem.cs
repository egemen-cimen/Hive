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
        /// <returns>Collection of populated neighbor hexagons</returns>
        public List<Hexagon> GetPopulatedNeighborHexagons((int column, int row) coordinate);

        /// <summary>
        /// Gets only the coordinates for the neighbor hexagons of the given coordinate.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>Collection of populated neighbor coordinates</returns>
        public HashSet<(int column, int row)> GetPopulatedNeighborCoordinates((int column, int row) coordinate);

        public HashSet<(int column, int row)> GetPopulatedNeighborCoordinatesWithoutHexagon((int column, int row) coordinate, (int column, int row) hexagonCoordinate);

        /// <summary>
        /// Gets only the neighbor hexagons common for given coordinates.
        /// </summary>
        /// <param name="coordinate1"></param>
        /// <param name="coordinate2"></param>
        /// <returns>Collection of common populated neighbor hexagons</returns>
        public List<Hexagon> GetSharedPopulatedNeighborHexagons((int column, int row) coordinate1, (int column, int row) coordinate2);

        /// <summary>
        /// Gets all possible adjacent coordinates of the given coordinate. Does not check if it's populated or not.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>Collection of all adjacent coordinates</returns>
        public HashSet<(int column, int row)> GetAdjacentCoordinates((int column, int row) coordinate);

        /// <summary>
        /// Gets only the free adjacent coordinates common for given coordinates. Given coordinates can be populated or free.
        /// </summary>
        /// <param name="coordinate1"></param>
        /// <param name="coordinate2"></param>
        /// <returns>Collection of free coordinates</returns>
        public HashSet<(int column, int row)> GetSharedFreeAdjacentCoordinates((int column, int row) coordinate1, (int column, int row) coordinate2);

        /// <summary>
        /// Removes hexagon from given coordinate.
        /// </summary>
        /// <param name="coordinate"></param>
        public void RemoveHexagon((int column, int row) coordinate);

        /// <summary>
        /// Gets all the coordinates from the coordinate system.
        /// </summary>
        /// <returns>Collection of all coordinates</returns>
        public HashSet<(int column, int row)> GetAllCoordinates();

        /// <summary>
        /// Gets all free adjacent coordinates for the entire coordinate system.
        /// </summary>
        /// <returns>Collection of free coordinates</returns>
        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinates();

        /// <summary>
        /// Gets all free adjacent coordinates for the entire coordinate system if the hexagon at the coordinate was free.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>Collection of free coordinates</returns>
        public HashSet<(int column, int row)> GetAllFreeAdjacentCoordinatesWithoutHexagon((int column, int row) coordinate);

        /// <summary>
        /// Verifies whether the all hexagons would still be connected if the hexagon at the coordinate was free.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>true if all the hexagons would be connected; otherwise, false</returns>
        public bool VerifyWhetherAllHexagonsConnectedWithoutHexagon((int column, int row) coordinate);

    }
}
