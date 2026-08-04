using Hive.Core.Models;
using Hive.Core.Rules;
using Hive.PlayerAgent;

var minimaxPlayer = new MinimaxPlayer();
var randomPlayer = new RandomPlayer();

var gameState = GameRules.CreateFreshGameState();

while (!GameRules.VerifyWhetherGameStateIsTerminal(gameState))
{
    var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);
    GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);

    if (GameRules.VerifyWhetherGameStateIsTerminal(gameState))
    {
        break;
    }

    suggestedAction = randomPlayer.SuggestNextPlayerAction(gameState);
    GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);

    PrintGameState(gameState);
    Console.WriteLine("-----------------------");
}

Console.WriteLine(GameRules.GetGameResult(gameState));

static void PrintGameState(GameState gameState)
{
    var allCoordinates = gameState.CoordinateSystem.GetAllCoordinates();
    foreach (var coordinate in allCoordinates.OrderBy(c => c.row))
    {
        gameState.CoordinateSystem.TryGetHexagon(coordinate, out Hexagon? retrievedHexagon);
        if (retrievedHexagon != null)
        {
            Console.WriteLine($"{retrievedHexagon.GetColor()} hexagon at ({coordinate.column}, {coordinate.row}) has {retrievedHexagon.GetPieceCount()} piece(s).");
            foreach (var piece in retrievedHexagon.GetAllPieces())
            {
                Console.WriteLine($"{piece.Color} {piece.GetPieceName()}");

            }
        }
    }
}