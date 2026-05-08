using Hive.Core.Models;

var axialCoordinateSystem = new AxialCoordinateSystem();

var hexagon1 = new Hexagon();
hexagon1.PushPiece(new SpiderPiece(PlayerColor.BLACK));
var hexagon2 = new Hexagon();
hexagon2.PushPiece(new SpiderPiece(PlayerColor.WHITE));
hexagon2.PushPiece(new BeetlePiece(PlayerColor.BLACK));

axialCoordinateSystem.AddHexagon(hexagon1, (0, 0));
axialCoordinateSystem.AddHexagon(hexagon2, (0, 1));

var allCoordinates = axialCoordinateSystem.GetAllCoordinates();
foreach (var coordinate in allCoordinates)
{
    axialCoordinateSystem.TryGetHexagon(coordinate, out Hexagon? retrievedHexagon);
    if (retrievedHexagon != null)
    {
        Console.WriteLine($"{retrievedHexagon.GetColor()} hexagon at ({coordinate.column}, {coordinate.row}) has {retrievedHexagon.GetPieceCount()} piece(s).");
        foreach (var piece in retrievedHexagon.GetAllPieces())
        {
            Console.WriteLine($"{piece.Color} {piece.GetPieceName()}");

        }
    }
}