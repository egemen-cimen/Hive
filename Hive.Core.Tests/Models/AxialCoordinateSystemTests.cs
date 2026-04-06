using Hive.Core.Models;

namespace Hive.Core.Tests.Models
{
    [TestClass]
    public class AxialCoordinateSystemTests
    {
        [TestMethod]
        public void Given_AddedHexagon_When_Retrieved_Then_ReturnsSameHexagon()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            var column = 0;
            var row = 0;

            coordinateSystem.AddHexagonToCoordinate(hexagon, column, row);

            // WHEN
            var retrievedHexagon = coordinateSystem.GetHexagonAtCoordinate(column, row);

            // THEN
            Assert.IsNotNull(retrievedHexagon);
            Assert.AreEqual(hexagon, retrievedHexagon);
        }

        [TestMethod]

        public void Given_AddedHexagon_When_AddedToSameCoordinate_Then_ThrowsException()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            var anotherHexagon = new Hexagon();
            var column = 0;
            var row = 0;

            coordinateSystem.AddHexagonToCoordinate(hexagon, column, row);

            // WHEN & THEN
            Assert.Throws<ArgumentException>(() => coordinateSystem.AddHexagonToCoordinate(anotherHexagon, column, row));
        }

        [TestMethod]

        public void Given_AddedHexagon_When_AddedToAnotherCoordinate_Then_RetrievalReturnsSecondHexagon()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            var column = 0;
            var row = 0;
            var anotherHexagon = new Hexagon();
            var anotherColumn = 1;
            var anotherRow = -1;

            coordinateSystem.AddHexagonToCoordinate(hexagon, column, row);

            // WHEN
            coordinateSystem.AddHexagonToCoordinate(anotherHexagon, anotherColumn, anotherRow);

            // THEN
            var retrievedHexagon = coordinateSystem.GetHexagonAtCoordinate(anotherColumn, anotherRow);
            Assert.IsNotNull(retrievedHexagon);
            Assert.AreNotEqual(hexagon, retrievedHexagon);
        }

        [TestMethod]
        public void Given_AddedHexagon_When_Removed_Then_RetrievalReturnsNull()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            var column = 0;
            var row = 0;

            coordinateSystem.AddHexagonToCoordinate(hexagon, column, row);

            // WHEN
            coordinateSystem.RemoveHexagonFromCoordinate(column, row);

            // THEN
            var retrievedHexagon = coordinateSystem.GetHexagonAtCoordinate(column, row);
            Assert.IsNull(retrievedHexagon);
        }

        [TestMethod]

        public void Given_NoHexagon_When_Removed_Then_ThrowsException()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var column = 0;
            var row = 0;

            // WHEN & THEN
            Assert.Throws<ArgumentException>(() => coordinateSystem.RemoveHexagonFromCoordinate(column, row));
        }

        [TestMethod]
        public void Given_AddedHexagons_When_PopulatedNeighborsRetrieved_Then_RetrievalReturnsOnlyNeighbors()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            var neighborHexagon1 = new Hexagon();
            var neighborHexagon2 = new Hexagon();
            var notNeighborHexagon = new Hexagon();

            coordinateSystem.AddHexagonToCoordinate(hexagon, 0, 0);
            coordinateSystem.AddHexagonToCoordinate(neighborHexagon1, 0, -1);
            coordinateSystem.AddHexagonToCoordinate(neighborHexagon2, 1, 0);
            coordinateSystem.AddHexagonToCoordinate(notNeighborHexagon, 2, 0);

            // WHEN
            var neigborHexagons = coordinateSystem.GetPopulatedNeighborsForCoordinate(0, 0);

            Assert.HasCount(2, neigborHexagons);
            CollectionAssert.AreEquivalent(new List<Hexagon?>() { neighborHexagon1, neighborHexagon2 }, neigborHexagons);
        }

        [TestMethod]
        public void Given_CoordinateSystem_When_AdjacentCoordinatesRetrieved_Then_RetrivalReturnsAllSix()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var column = 0;
            var row = 0;

            // WHEN
            var adjacentCoordinates = coordinateSystem.GetAdjacentCoordinates(column, row);

            // THEN
            Assert.HasCount(6, adjacentCoordinates);
            var expectedCoordinates = new List<(int column, int row)>() {
                (0, -1),
                (1, -1),
                (-1, 0),
                (1, 0),
                (-1, 1),
                (0, 1)
            };

            CollectionAssert.AreEquivalent(expectedCoordinates, adjacentCoordinates);
        }
    }
}
