using Hive.Core.Models;
using NSubstitute;

namespace Hive.Core.Tests.Models
{
    [TestClass]
    public class GameBoardTests
    {
        [TestMethod]
        public void Given_EmptyGameBoard_When_EmptySpacesRetrieved_Then_ReturnsSingleSpace()
        {
            // GIVEN
            var coordinateSystem = Substitute.For<ICoordinateSystem>();
            coordinateSystem.GetListOfCoordinates().Returns([]);
            var gameBoard = new GameBoard(coordinateSystem);

            // WHEN
            var emptySpaces = gameBoard.GetEmptySpacesOnTheBoard();

            // THEN
            Assert.HasCount(1, emptySpaces);
        }

        [TestMethod]
        public void Given_GameBoardWithNoPieces_When_SpawnedPiece_Then_AddsHexagon()
        {
            // GIVEN
            var coordinateSystem = Substitute.For<ICoordinateSystem>();
            Hexagon? nullHexagonAtDestiantion = null;
            coordinateSystem.GetHexagonAtCoordinate(0, 0).Returns(nullHexagonAtDestiantion);
            var gameBoard = new GameBoard(coordinateSystem);

            // WHEN
            var result = gameBoard.SpawnPiece(0, 0, new SpiderPiece(PlayerColor.BLACK));

            // THEN
            Assert.IsTrue(result);
            coordinateSystem.Received().AddHexagonToCoordinate(Arg.Any<Hexagon>(), 0, 0);
        }
    }
}
