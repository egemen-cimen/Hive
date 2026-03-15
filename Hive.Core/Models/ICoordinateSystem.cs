namespace Hive.Core.Models
{
    public interface ICoordinateSystem
    {
        public void AddHexagonToCoordinate(Hexagon hexagon, int column, int row);

        public Hexagon? GetHexagonAtCoordinate(int column, int row);

        public void RemoveHexagonFromCoordinate(int column, int row);
    }
}
