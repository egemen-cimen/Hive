using Hive.Core.Models;
using NSubstitute;

namespace Hive.Core.Tests.Models
{
    [TestClass]
    public class GameBoardTests
    {
        [TestMethod]
        public void Given_GameBoardWithNoPieces_When_SpawnedPiece_Then_AddsHexagon()
        {
            // GIVEN
            var coordinateSystem = Substitute.For<ICoordinateSystem>();
            (int column, int row) coordinate = (0, 0);
            coordinateSystem.TryGetHexagonAtCoordinate(coordinate, out var _).Returns(false);
            var gameBoard = new GameBoard(coordinateSystem);

            // WHEN
            var result = gameBoard.SpawnPiece(coordinate, new SpiderPiece(PlayerColor.BLACK));

            // THEN
            Assert.IsTrue(result);
            coordinateSystem.Received().AddHexagonToCoordinate(Arg.Any<Hexagon>(), coordinate);
        }
    }
}
