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
            (int column, int row) coordinate = (0, 0);

            coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);

            // WHEN
            var hexagonExists = coordinateSystem.TryGetHexagonAtCoordinate(coordinate, out Hexagon? retrievedHexagon);

            // THEN
            Assert.IsTrue(hexagonExists);
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
            (int column, int row) coordinate = (0, 0);

            coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);

            // WHEN & THEN
            Assert.Throws<ArgumentException>(() => coordinateSystem.AddHexagonToCoordinate(anotherHexagon, coordinate));
        }

        [TestMethod]

        public void Given_AddedHexagon_When_AddedToAnotherCoordinate_Then_RetrievalReturnsSecondHexagon()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, 0);

            var anotherHexagon = new Hexagon();
            (int column, int row) anotherCoordinate = (1, -1);

            coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);

            // WHEN
            coordinateSystem.AddHexagonToCoordinate(anotherHexagon, anotherCoordinate);

            // THEN
            var hexagonExists = coordinateSystem.TryGetHexagonAtCoordinate(anotherCoordinate, out Hexagon? retrievedHexagon);
            Assert.IsTrue(hexagonExists);
            Assert.IsNotNull(retrievedHexagon);
            Assert.AreNotEqual(hexagon, retrievedHexagon);
        }

        [TestMethod]
        public void Given_AddedHexagon_When_Removed_Then_RetrievalReturnsNull()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, 0);

            coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);

            // WHEN
            coordinateSystem.RemoveHexagonFromCoordinate(coordinate);

            // THEN
            var hexagonExists = coordinateSystem.TryGetHexagonAtCoordinate(coordinate, out Hexagon? retrievedHexagon);
            Assert.IsFalse(hexagonExists);
            Assert.IsNull(retrievedHexagon);
        }

        [TestMethod]

        public void Given_NoHexagon_When_Removed_Then_ThrowsException()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            (int column, int row) coordinate = (0, 0);

            // WHEN & THEN
            Assert.Throws<ArgumentException>(() => coordinateSystem.RemoveHexagonFromCoordinate(coordinate));
        }

        [TestMethod]
        public void Given_AddedHexagons_When_PopulatedNeighborsRetrieved_Then_RetrievalReturnsOnlyNeighbors()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, 0);

            var neighborHexagon1 = new Hexagon();
            var neighborHexagon2 = new Hexagon();
            var notNeighborHexagon = new Hexagon();

            coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);
            coordinateSystem.AddHexagonToCoordinate(neighborHexagon1, (0, -1));
            coordinateSystem.AddHexagonToCoordinate(neighborHexagon2, (1, 0));
            coordinateSystem.AddHexagonToCoordinate(notNeighborHexagon, (2, 0));

            // WHEN
            var neigborHexagons = coordinateSystem.GetPopulatedNeighborsForCoordinate(coordinate);

            Assert.HasCount(2, neigborHexagons);
            CollectionAssert.AreEquivalent(new List<Hexagon>() { neighborHexagon1, neighborHexagon2 }, neigborHexagons);
        }

        [TestMethod]
        public void Given_CoordinateSystem_When_AdjacentCoordinatesRetrieved_Then_RetrivalReturnsAllSix()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            (int column, int row) coordinate = (0, 0);

            // WHEN
            var adjacentCoordinates = coordinateSystem.GetAdjacentCoordinates(coordinate);

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
