using Hive.Core.Models;

var axialCoordinateSystem = new AxialCoordinateSystem();

var hexagon1 = new Hexagon();
hexagon1.PushPiece(new SpiderPiece(PlayerColor.BLACK));
var hexagon2 = new Hexagon();
hexagon2.PushPiece(new SpiderPiece(PlayerColor.WHITE));

axialCoordinateSystem.AddHexagonToCoordinate(hexagon1, 0, 0);
axialCoordinateSystem.AddHexagonToCoordinate(hexagon2, 0, 1);

var allCoordinates = axialCoordinateSystem.GetListOfCoordinates();
foreach (var coordinate in allCoordinates)
{
    var retrievedHexagon = axialCoordinateSystem.GetHexagonAtCoordinate(coordinate.Item1, coordinate.Item2);
    if (retrievedHexagon != null)
    {
        Console.WriteLine($"{retrievedHexagon.GetColor()} hexagon at ({coordinate.Item1}, {coordinate.Item2}) has {retrievedHexagon.GetPieceCount()} piece(s).");
        foreach (var piece in retrievedHexagon.GetAllPieces())
        {
            Console.WriteLine($"{piece.Color} {piece.GetPieceName()}");

        }
    }
}