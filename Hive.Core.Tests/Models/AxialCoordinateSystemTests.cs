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

            coordinateSystem.AddHexagon(hexagon, coordinate);

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

            coordinateSystem.AddHexagon(hexagon, coordinate);

            // WHEN & THEN
            Assert.Throws<ArgumentException>(() => coordinateSystem.AddHexagon(anotherHexagon, coordinate));
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

            coordinateSystem.AddHexagon(hexagon, coordinate);

            // WHEN
            coordinateSystem.AddHexagon(anotherHexagon, anotherCoordinate);

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

            coordinateSystem.AddHexagon(hexagon, coordinate);

            // WHEN
            coordinateSystem.RemoveHexagon(coordinate);

            // THEN
            var hexagonExists = coordinateSystem.TryGetHexagon(coordinate, out Hexagon? retrievedHexagon);
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
            Assert.Throws<ArgumentException>(() => coordinateSystem.RemoveHexagon(coordinate));
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

            coordinateSystem.AddHexagon(hexagon, coordinate);
            coordinateSystem.AddHexagon(neighborHexagon1, (0, -1));
            coordinateSystem.AddHexagon(neighborHexagon2, (1, 0));
            coordinateSystem.AddHexagon(notNeighborHexagon, (2, 0));

            // WHEN
            var neigborHexagons = coordinateSystem.GetPopulatedNeighborHexagons(coordinate);

            Assert.HasCount(2, neigborHexagons);
            CollectionAssert.AreEquivalent(new List<Hexagon>() { neighborHexagon1, neighborHexagon2 }, neigborHexagons);
        }

        [TestMethod]
        public void Given_AddedHexagons_When_PopulatedNeighborsCoordinatesRetrieved_Then_RetrievalReturnsOnlyNeighborCoordinates()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, -1);

            var neighborHexagon1 = new Hexagon();
            var neighborHexagon2 = new Hexagon();
            var notNeighborHexagon = new Hexagon();

            coordinateSystem.AddHexagon(hexagon, coordinate);
            coordinateSystem.AddHexagon(neighborHexagon1, (1, -1));
            coordinateSystem.AddHexagon(neighborHexagon2, (0, 0));
            coordinateSystem.AddHexagon(notNeighborHexagon, (0, 1));

            // WHEN
            var neigborHexagons = coordinateSystem.GetPopulatedNeighborCoordinates(coordinate);

            Assert.HasCount(2, neigborHexagons);
            Assert.IsTrue(new HashSet<(int column, int row)>() { (0, 0), (1, -1) }.SetEquals(neigborHexagons));
        }

        [TestMethod]
        [DataRow(1, -1, 1, -2)]
        [DataRow(0, 0, -1, 0)]
        public void Given_AddedHexagons_When_SharedFreeAdjacentCoordinatesRetrieved_Then_RetrievalReturnsOnlyFreeAjacentCoordinates(int neighborColumn, int neighborRow, int freeAdjacentColumn, int freeAdjacentRow)
        {
            // GIVEN

            // Three hexagons in a line.
            //
            //              [ 1,-2]
            //
            //          [ 0,-1] [ 1,-1]
            //
            //      [-1, 0] [ q, r]
            //
            //                  [ 0, 1]
            //
            // Where q is column and r is row.
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, -1);

            var neighborHexagon1 = new Hexagon();
            var neighborHexagon2 = new Hexagon();
            var notNeighborHexagon = new Hexagon();

            coordinateSystem.AddHexagon(hexagon, coordinate);
            coordinateSystem.AddHexagon(neighborHexagon1, (1, -1));
            coordinateSystem.AddHexagon(neighborHexagon2, (0, 0));
            coordinateSystem.AddHexagon(notNeighborHexagon, (0, 1));

            // WHEN
            var neigborHexagons = coordinateSystem.GetSharedFreeAdjacentCoordinates(coordinate, (neighborColumn, neighborRow));

            Assert.HasCount(1, neigborHexagons);
            Assert.IsTrue(new HashSet<(int column, int row)>() { (freeAdjacentColumn, freeAdjacentRow) }.SetEquals(neigborHexagons));
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
            var expectedCoordinates = new HashSet<(int column, int row)>() {
                (0, -1),
                (1, -1),
                (-1, 0),
                (1, 0),
                (-1, 1),
                (0, 1)
            };

            Assert.IsTrue(expectedCoordinates.SetEquals(adjacentCoordinates));
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
            var expectedCoordinates = new HashSet<(int column, int row)>() {
                (0, 0)
            };

            Assert.IsTrue(expectedCoordinates.SetEquals(freeAdjacents));
        }

        [TestMethod]
        public void Given_AddedHexagon_When_RetrievedAllFreeAdjacents_Then_ReturnsSixCoordinates()
        {
            // GIVEN

            // Single hexagon in the middle should return all the spaces around it.
            //
            //      [ 0,-1] [ 1,-1]
            //
            //  [-1, 0] [ q, r] [ 1, 0]
            //
            //      [-1, 1] [ 0, 1]
            //
            // Where q is column and r is row.
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, 0);

            coordinateSystem.AddHexagon(hexagon, coordinate);

            // WHEN
            var freeAdjacents = coordinateSystem.GetAllFreeAdjacentCoordinates();

            // THEN
            Assert.HasCount(6, freeAdjacents);
            var expectedCoordinates = new HashSet<(int column, int row)>() {
                ( 0,-1),
                ( 1,-1),
                (-1, 0),
                ( 1, 0),
                (-1, 1),
                ( 0, 1)
            };

            Assert.IsTrue(expectedCoordinates.SetEquals(freeAdjacents));
        }

        [TestMethod]
        public void Given_AddedHexagons_When_RetrievedAllFreeAdjacents_Then_ReturnsAllFreeCoordinates()
        {
            // GIVEN

            // Two hexagons in should return all the empty spaces around it.
            //
            //      [ 0,-2] [ 1,-2]
            //
            //  [-1,-1] [ 0,-1] [ 1,-1]
            //
            //      [-1, 0] [ q, r] [ 1, 0]
            //
            //          [-1, 1] [ 0, 1]
            //
            // Where q is column and r is row.
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, 0);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, -1);

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);

            // WHEN
            var freeAdjacents = coordinateSystem.GetAllFreeAdjacentCoordinates();

            // THEN
            Assert.HasCount(8, freeAdjacents);
            var expectedCoordinates = new HashSet<(int column, int row)>() {
                ( 0,-2),
                ( 1,-2),
                (-1,-1),
                ( 1,-1),
                (-1, 0),
                ( 1, 0),
                (-1, 1),
                ( 0, 1),
            };

            Assert.IsTrue(expectedCoordinates.SetEquals(freeAdjacents));
        }

        [TestMethod]
        public void Given_AddedHexagonsInACycle_When_RetrievedAllFreeAdjacents_Then_ReturnsAllFreeCoordinates()
        {
            // GIVEN

            // A cycle with hexagons should return all the free spaces around and inside it.
            //
            //  	    [ 0,-2] [ 1,-2] [ 2,-2]
            //
            //      [-1,-1] [ 0,-1] [ 1,-1] [ 2,-1]
            //
            //  [-2, 0] [-1, 0] [ q, r] [ 1, 0] [ 2, 0]
            //
            //      [-2, 1] [-1, 1] [ 0, 1] [ 1, 1]
            //
            //          [-2, 2] [-1, 2] [ 0, 2]
            //
            // Where q is column and r is row.
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

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);
            coordinateSystem.AddHexagon(hexagon3, coordinate3);
            coordinateSystem.AddHexagon(hexagon4, coordinate4);
            coordinateSystem.AddHexagon(hexagon5, coordinate5);
            coordinateSystem.AddHexagon(hexagon6, coordinate6);

            // WHEN
            var freeAdjacents = coordinateSystem.GetAllFreeAdjacentCoordinates();

            // THEN
            Assert.HasCount(13, freeAdjacents);
            var expectedCoordinates = new HashSet<(int column, int row)>() {
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

            Assert.IsTrue(expectedCoordinates.SetEquals(freeAdjacents));
        }

        [TestMethod]
        public void Given_AddedUnreachableHexagons_When_RetrievedAllFreeAdjacents_Then_ThrowsException()
        {
            // GIVEN

            // Two hexagons are NOT connected.
            //
            //  [ 0,-2]
            //
            //
            //
            //          [ q, r]
            //
            // Where q is column and r is row.
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, 0);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, -2);

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);

            // WHEN & THEN
            Assert.Throws<InvalidOperationException>(coordinateSystem.GetAllFreeAdjacentCoordinates);
        }

        [TestMethod]
        public void Given_AddedTwoHexagons_When_RetrievedAllFreeAdjacentsWithoutOne_Then_ReturnsAllFreeCoordinates()
        {
            // GIVEN

            // Two hexagons without one should return all the empty spaces around the other.
            //
            //      [ 0,-2] [ 1,-2]
            //
            //  [-1,-1] [ 0,-1] [ 1,-1]
            //
            //      [-1, 0] [ q, r] [ 1, 0]
            //
            //          [-1, 1] [ 0, 1]
            //
            // Where q is column and r is row.
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, 0);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, -1);

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);

            // WHEN
            var freeAdjacents = coordinateSystem.GetAllFreeAdjacentCoordinatesWithoutHexagon(coordinate2);

            // THEN
            Assert.HasCount(6, freeAdjacents);
            var expectedCoordinates = new HashSet<(int column, int row)>() {
                ( 0,-1),
                ( 1,-1),
                (-1, 0),
                ( 1, 0),
                (-1, 1),
                ( 0, 1)
            };

            Assert.IsTrue(expectedCoordinates.SetEquals(freeAdjacents));
        }

        [TestMethod]
        public void Given_AddedHexagon_When_CheckedWhetherAllConnectedWithoutIt_Then_ReturnsTrue()
        {
            // GIVEN

            // Single hexagon in the middle.
            //
            //  [ q, r]
            //
            // Where q is column and r is row.
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon = new Hexagon();
            (int column, int row) coordinate = (0, 0);

            coordinateSystem.AddHexagon(hexagon, coordinate);

            // WHEN
            var isAllConnected = coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(coordinate);

            // THEN
            Assert.IsTrue(isAllConnected);
        }

        [TestMethod]
        public void Given_AddedHexagons_When_CheckedWhetherAllConnectedWithoutOne_Then_ReturnsTrue()
        {
            // GIVEN

            // Hexagon is connected after removing one of them.
            //
            //  [ 0,-1]
            //
            //      [ q, r]
            //
            // Where q is column and r is row.
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, -1);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, 0);

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);

            // WHEN
            var isAllConnected = coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(coordinate2);

            // THEN
            Assert.IsTrue(isAllConnected);
        }

        [TestMethod]
        public void Given_AddedHexagons_When_CheckedWhetherAllConnectedWithoutMiddleOne_Then_ReturnsFalse()
        {
            // GIVEN

            // Two hexagons are NOT connected after removing the middle one.
            //
            //  [ 0,-2]
            //
            //      [ 0,-1]
            //
            //          [ q, r]
            //
            // Where q is column and r is row.
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, -2);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, -1);
            var hexagon3 = new Hexagon();
            (int column, int row) coordinate3 = (0, 0);

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);
            coordinateSystem.AddHexagon(hexagon3, coordinate3);

            // WHEN
            var isAllConnected = coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(coordinate2);

            // THEN
            Assert.IsFalse(isAllConnected);
        }

        [TestMethod]
        public void Given_AddedHexagonsInACycle_When_CheckedWhetherAllConnectedWithoutOne_Then_ReturnsTrue()
        {
            // GIVEN

            // A cycle with hexagons are connected after removing one.
            //
            //      [ 0,-1] [ 1,-1]
            //
            //  [-1, 0] [ q, r] [ 1, 0]
            //
            //      [-1, 1] [ 0, 1]
            //
            // Where q is column and r is row.
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

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);
            coordinateSystem.AddHexagon(hexagon3, coordinate3);
            coordinateSystem.AddHexagon(hexagon4, coordinate4);
            coordinateSystem.AddHexagon(hexagon5, coordinate5);
            coordinateSystem.AddHexagon(hexagon6, coordinate6);

            // WHEN
            var isAllConnected = coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(coordinate2);

            // THEN
            Assert.IsTrue(isAllConnected);
        }

        [TestMethod]
        public void Given_CoordinateSystemRetrived_When_ChangesMadeToResult_Then_ChangesShouldNotAffectOriginal()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, -1);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (1, -1);

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);

            var retrievedCoordinateSystem = coordinateSystem.GetAllCoordinates();

            // WHEN
            retrievedCoordinateSystem.Add((-1, 0));
            var anotherRetrievedCoordinateSystem = coordinateSystem.GetAllCoordinates();

            // THEN
            Assert.HasCount(2, anotherRetrievedCoordinateSystem);
            Assert.IsTrue(anotherRetrievedCoordinateSystem.TryGetValue((0, -1), out _));
            Assert.IsTrue(anotherRetrievedCoordinateSystem.TryGetValue((1, -1), out _));
            Assert.IsFalse(anotherRetrievedCoordinateSystem.TryGetValue((-1, 0), out _));
        }

        [TestMethod]
        public void Given_AddedHexagonsInACycle_When_SharedPopulatedNeighborsRetrieved_Then_ReturnsCollection()
        {
            // GIVEN

            // A cycle with hexagons are connected and all have two neighbors.
            //
            //      [ 0,-1] [ 1,-1]
            //
            //  [-1, 0] [ q, r] [ 1, 0]
            //
            //      [-1, 1] [ 0, 1]
            //
            // Where q is column and r is row.
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

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);
            coordinateSystem.AddHexagon(hexagon3, coordinate3);
            coordinateSystem.AddHexagon(hexagon4, coordinate4);
            coordinateSystem.AddHexagon(hexagon5, coordinate5);
            coordinateSystem.AddHexagon(hexagon6, coordinate6);

            // WHEN
            var sharedNighbors = coordinateSystem.GetSharedPopulatedNeighborHexagons(coordinate3, coordinate2);

            // THEN
            Assert.HasCount(1, sharedNighbors);
            Assert.AreEqual(hexagon1, sharedNighbors[0]);
        }

        [TestMethod]
        public void Given_CoordinateSystem_When_ToStringCalled_Then_ReturnsAsciiArtOfCoordinateSystem()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            (int column, int row) coordinate1 = (0, -2);
            var hexagon2 = new Hexagon();
            (int column, int row) coordinate2 = (0, -1);
            var hexagon3 = new Hexagon();
            (int column, int row) coordinate3 = (0, 0);

            coordinateSystem.AddHexagon(hexagon1, coordinate1);
            coordinateSystem.AddHexagon(hexagon2, coordinate2);
            coordinateSystem.AddHexagon(hexagon3, coordinate3);

            // WHEN
            var asciiArt = coordinateSystem.ToString();

            // THEN
            var expectedAsciiArt = @"
[ 0,-2]

    [ 0,-1]

        [ q, r]
";
            Assert.AreEqual(expectedAsciiArt, asciiArt);
        }
    }
}
