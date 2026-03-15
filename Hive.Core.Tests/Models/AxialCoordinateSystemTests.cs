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
    }
}
