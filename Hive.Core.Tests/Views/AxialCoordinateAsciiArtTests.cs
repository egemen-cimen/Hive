using Hive.Core.Models;
using Hive.Core.Views;

namespace Hive.Core.Tests.Views
{
    [TestClass]
    public class AxialCoordinateAsciiArtTests
    {
        [TestMethod]
        public void Given_PopulatedCoordinateSystem_When_GenerateAsciiArtCalled_Then_ReturnsAsciiArtOfCoordinateSystem()
        {
            // GIVEN
            var coordinateSystem = new AxialCoordinateSystem();
            var hexagon1 = new Hexagon();
            var hexagon2 = new Hexagon();
            var hexagon3 = new Hexagon();
            var hexagon4 = new Hexagon();
            var hexagon5 = new Hexagon();

            coordinateSystem.AddHexagon(hexagon1, (0, -2));
            coordinateSystem.AddHexagon(hexagon2, (0, -1));
            coordinateSystem.AddHexagon(hexagon3, (0, 0));
            coordinateSystem.AddHexagon(hexagon4, (-1, 1));
            coordinateSystem.AddHexagon(hexagon5, (-2, 2));

            // WHEN
            var asciiArt = AxialCoordinateAsciiArt.GenerateAsciiArt(coordinateSystem);

            // THEN
            // TODO: also implement printing pieces on top of the hexagons
            var expectedAsciiArt = @"
[   0,  -2]

      [   0,  -1]

            [   0,   0]

      [  -1,   1]

[  -2,   2]
";
            Assert.AreEqual(expectedAsciiArt, asciiArt);
        }
    }
}
