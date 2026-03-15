using Hive.Core.Models;
using NSubstitute;

namespace Hive.Core.Tests.Models
{
    [TestClass]
    public class HexagonTests
    {
        [TestMethod]
        public void Given_AddedPiece_When_PieceRetrieved_Then_ReturnsSamePiece()
        {
            // GIVEN
            var hexagon = new Hexagon();
            var piece = Substitute.For<IPiece>();

            hexagon.PushPiece(piece);

            // WHEN
            var retrievedPiece = hexagon.PopPiece();

            // THEN
            Assert.IsNotNull(retrievedPiece);
            Assert.AreEqual(piece, retrievedPiece);
        }

        [TestMethod]
        public void Given_AddedPiece_When_AddedAnotherPiece_Then_RetrievalReturnsLastPiece()
        {
            // GIVEN
            var hexagon = new Hexagon();
            var piece1 = Substitute.For<IPiece>();
            var piece2 = Substitute.For<IPiece>();

            hexagon.PushPiece(piece1);
            hexagon.PushPiece(piece2);

            // WHEN
            var retrievedPiece = hexagon.PopPiece();

            // THEN
            Assert.IsNotNull(retrievedPiece);
            Assert.AreEqual(piece2, retrievedPiece);
            Assert.AreNotEqual(piece1, retrievedPiece);
        }

        [TestMethod]
        public void Given_AddedPiece_When_AddedOtherColoredPiece_Then_RetrievalReturnsLastColor()
        {
            // GIVEN
            var hexagon = new Hexagon();
            var piece1 = Substitute.For<IPiece>();
            piece1.Color.ReturnsForAnyArgs(PlayerColor.BLACK);
            var piece2 = Substitute.For<IPiece>();
            piece2.Color.ReturnsForAnyArgs(PlayerColor.WHITE);

            hexagon.PushPiece(piece1);
            hexagon.PushPiece(piece2);

            // WHEN
            var retrievedColor = hexagon.GetColor();

            // THEN
            Assert.AreEqual(PlayerColor.WHITE, retrievedColor);
        }

        [TestMethod]
        public void Given_NoPiece_When_PieceRetrieved_Then_ThrowsException()
        {
            // GIVEN
            var hexagon = new Hexagon();

            // WHEN & THEN
            Assert.Throws<InvalidOperationException>(hexagon.PopPiece);
        }

        [TestMethod]
        public void Given_NoPiece_When_ColorRetrieved_Then_ThrowsException()
        {
            // GIVEN
            var hexagon = new Hexagon();

            // WHEN & THEN
            Assert.Throws<InvalidOperationException>(() => hexagon.GetColor());
        }
    }
}
