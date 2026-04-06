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
            var retrievedHexagon = coordinateSystem.GetHexagonAtCoordinate(coordinate);

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
            var retrievedHexagon = coordinateSystem.GetHexagonAtCoordinate(anotherCoordinate);
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
        public void Given_NoHexagon_When_Retrieved_Then_ThrowsException()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            (int column, int row) coordinate = (0, 0);

            // WHEN & THEN
            Assert.Throws<ArgumentException>(() => coordinateSystem.GetHexagonAtCoordinate(coordinate));
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
            var neigborHexagons = coordinateSystem.GetPopulatedNeighborHexagonsForCoordinate(coordinate);

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
            var adjacentCoordinates = coordinateSystem.GetAdjacentCoordinatesForCoordinate(coordinate);

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

        [TestMethod]
        public void Given_NoHexagon_When_RetrievedAllFreeAdjacents_Then_ReturnsSingleCoordinate()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();

            // WHEN
            var freeAdjacents = coordinateSystem.GetAllFreeAdjacentCoordinates();

            // THEN
            Assert.HasCount(1, freeAdjacents);
            var expectedCoordinates = new List<(int column, int row)>() {
                (0, 0)
            };

            CollectionAssert.AreEquivalent(expectedCoordinates, freeAdjacents);
        }

        /// <summary>
        /// Single hexagon in the middle should return all the spaces around it.
        /// 
        ///     [ 0,-1] [ 1,-1]
        /// 
        /// [-1, 0] [ q, r] [ 1, 0]
        /// 
        ///     [-1, 1] [ 0, 1]
        /// 
        /// Where q is column and r is row.
        /// </summary>
        [TestMethod]
        public void Given_AddedHexagon_When_RetrievedAllFreeAdjacents_Then_ReturnsSixCoordinates()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, 0);

            coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);

            // WHEN
            var freeAdjacents = coordinateSystem.GetAllFreeAdjacentCoordinates();

            // THEN
            Assert.HasCount(6, freeAdjacents);
            var expectedCoordinates = new List<(int column, int row)>() {
                ( 0,-1),
                ( 1,-1),
                (-1, 0),
                ( 1, 0),
                (-1, 1),
                ( 0, 1)
            };

            CollectionAssert.AreEquivalent(expectedCoordinates, freeAdjacents);
        }

        /// <summary>
        /// Two hexagons in should return all the empty spaces around it.
        /// 
        ///     [ 0,-2] [ 1,-2]
        /// 
        /// [-1,-1] [ 0,-1] [ 1,-1]
        /// 
        ///     [-1, 0] [ q, r] [ 1, 0]
        /// 
        ///         [-1, 1] [ 0, 1]
        /// 
        /// Where q is column and r is row.
        /// </summary>
        [TestMethod]
        public void Given_AddedHexagons_When_RetrievedAllFreeAdjacents_Then_ReturnsAllFreeCoordinates()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, 0);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, -1);

            coordinateSystem.AddHexagonToCoordinate(hexagon1, coordinate1);
            coordinateSystem.AddHexagonToCoordinate(hexagon2, coordinate2);

            // WHEN
            var freeAdjacents = coordinateSystem.GetAllFreeAdjacentCoordinates();

            // THEN
            Assert.HasCount(8, freeAdjacents);
            var expectedCoordinates = new List<(int column, int row)>() {
                ( 0,-2),
                ( 1,-2),
                (-1,-1),
                ( 1,-1),
                (-1, 0),
                ( 1, 0),
                (-1, 1),
                ( 0, 1),
            };

            CollectionAssert.AreEquivalent(expectedCoordinates, freeAdjacents);
        }

        /// <summary>
        /// A cycle with hexagons should return all the free spaces around and inside it.
        /// 
        /// 	    [ 0,-2] [ 1,-2] [ 2,-2]
        /// 
        ///     [-1,-1] [ 0,-1] [ 1,-1] [ 2,-1]
        /// 
        /// [-2, 0] [-1, 0] [ q, r] [ 1, 0] [ 2, 0]
        /// 
        ///     [-2, 1] [-1, 1] [ 0, 1] [ 1, 1]
        /// 
        ///         [-2, 2] [-1, 2] [ 0, 2]
        /// 
        /// Where q is column and r is row.
        /// </summary>
        [TestMethod]
        public void Given_AddedHexagonsInACycle_When_RetrievedAllFreeAdjacents_Then_ReturnsAllFreeCoordinates()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, -1);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (1, -1);
            var hexagon3 = new Hexagon();
            (int column, int row) coordinate3 = (-1, 0);
            var hexagon4 = new Hexagon();
            (int column, int row) coordinate4 = (1, 0);
            var hexagon5 = new Hexagon();
            (int column, int row) coordinate5 = (-1, 1);
            var hexagon6 = new Hexagon();
            (int column, int row) coordinate6 = (0, 1);

            coordinateSystem.AddHexagonToCoordinate(hexagon1, coordinate1);
            coordinateSystem.AddHexagonToCoordinate(hexagon2, coordinate2);
            coordinateSystem.AddHexagonToCoordinate(hexagon3, coordinate3);
            coordinateSystem.AddHexagonToCoordinate(hexagon4, coordinate4);
            coordinateSystem.AddHexagonToCoordinate(hexagon5, coordinate5);
            coordinateSystem.AddHexagonToCoordinate(hexagon6, coordinate6);

            // WHEN
            var freeAdjacents = coordinateSystem.GetAllFreeAdjacentCoordinates();

            // THEN
            Assert.HasCount(13, freeAdjacents);
            var expectedCoordinates = new List<(int column, int row)>() {
                ( 0,-2),
                ( 1,-2),
                ( 2,-2),
                (-1,-1),
                ( 2,-1),
                (-2, 0),
                ( 0, 0),
                ( 2, 0),
                (-2, 1),
                ( 1, 1),
                (-2, 2),
                (-1, 2),
                ( 0, 2)
            };

            CollectionAssert.AreEquivalent(expectedCoordinates, freeAdjacents);
        }

        /// <summary>
        /// Two hexagons are NOT connected.
        /// 
        /// [ 0,-2]
        /// 
        ///     [ 0,-1]
        /// 
        ///         [ q, r]
        /// 
        /// Where q is column and r is row.
        /// </summary>
        [TestMethod]
        public void Given_AddedUnreachableHexagons_When_RetrievedAllFreeAdjacents_Then_ThrowsException()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, 0);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, -2);

            coordinateSystem.AddHexagonToCoordinate(hexagon1, coordinate1);
            coordinateSystem.AddHexagonToCoordinate(hexagon2, coordinate2);

            // WHEN & THEN
            Assert.Throws<InvalidOperationException>(coordinateSystem.GetAllFreeAdjacentCoordinates);
        }

        /// <summary>
        /// Single hexagon in the middle.
        /// 
        /// [ q, r]
        /// 
        /// Where q is column and r is row.
        /// </summary>
        [TestMethod]
        public void Given_AddedHexagon_When_CheckedWhetherAllConnectedWithoutIt_Then_ReturnsTrue()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, 0);

            coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);

            // WHEN
            var isAllConnected = coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(coordinate);

            // THEN
            Assert.IsTrue(isAllConnected);
        }

        /// <summary>
        /// Hexagon is connected after removing one of them.
        /// 
        /// [ 0,-1]
        /// 
        ///     [ q, r]
        /// 
        /// Where q is column and r is row.
        [TestMethod]
        public void Given_AddedHexagons_When_CheckedWhetherAllConnectedWithoutOne_Then_ReturnsTrue()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, -1);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, 0);

            coordinateSystem.AddHexagonToCoordinate(hexagon1, coordinate1);
            coordinateSystem.AddHexagonToCoordinate(hexagon2, coordinate2);

            // WHEN
            var isAllConnected = coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(coordinate2);

            // THEN
            Assert.IsTrue(isAllConnected);
        }

        /// <summary>
        /// Two hexagons are NOT connected after removing the middle one.
        /// 
        /// [ 0,-2]
        /// 
        ///     [ 0,-1]
        /// 
        ///         [ q, r]
        /// 
        /// Where q is column and r is row.
        [TestMethod]
        public void Given_AddedHexagons_When_CheckedWhetherAllConnectedWithoutMiddleOne_Then_ReturnsFalse()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, -2);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, -1);
            var hexagon3 = new Hexagon();
            (int column, int row) coordinate3 = (0, 0);

            coordinateSystem.AddHexagonToCoordinate(hexagon1, coordinate1);
            coordinateSystem.AddHexagonToCoordinate(hexagon2, coordinate2);
            coordinateSystem.AddHexagonToCoordinate(hexagon3, coordinate3);

            // WHEN
            var isAllConnected = coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(coordinate2);

            // THEN
            Assert.IsFalse(isAllConnected);
        }


        /// <summary>
        /// A cycle with hexagons are connected after removing one.
        ///  
        ///     [ 0,-1] [ 1,-1]
        /// 
        /// [-1, 0] [ q, r] [ 1, 0]
        /// 
        ///     [-1, 1] [ 0, 1]
        /// 
        /// Where q is column and r is row.
        /// </summary>
        [TestMethod]
        public void Given_AddedHexagonsInACycle_When_CheckedWhetherAllConnectedWithoutOne_Then_ReturnsTrue()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, -1);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (1, -1);
            var hexagon3 = new Hexagon();
            (int column, int row) coordinate3 = (-1, 0);
            var hexagon4 = new Hexagon();
            (int column, int row) coordinate4 = (1, 0);
            var hexagon5 = new Hexagon();
            (int column, int row) coordinate5 = (-1, 1);
            var hexagon6 = new Hexagon();
            (int column, int row) coordinate6 = (0, 1);

            coordinateSystem.AddHexagonToCoordinate(hexagon1, coordinate1);
            coordinateSystem.AddHexagonToCoordinate(hexagon2, coordinate2);
            coordinateSystem.AddHexagonToCoordinate(hexagon3, coordinate3);
            coordinateSystem.AddHexagonToCoordinate(hexagon4, coordinate4);
            coordinateSystem.AddHexagonToCoordinate(hexagon5, coordinate5);
            coordinateSystem.AddHexagonToCoordinate(hexagon6, coordinate6);

            // WHEN
            var isAllConnected = coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(coordinate2);

            // THEN
            Assert.IsTrue(isAllConnected);
        }
    }
}
