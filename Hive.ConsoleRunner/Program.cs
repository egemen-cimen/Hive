using Hive.Core.Models;
using Hive.Core.Rules;
using Hive.PlayerAgent;

var minimaxPlayer = new MinimaxPlayer();
var randomPlayer = new RandomPlayer();

var gameState = GameRules.CreateFreshGameState();

while (!GameRules.VerifyWhetherGameStateIsTerminal(gameState))
{
    var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);
    PrintPlayerAction(suggestedAction, gameState);
    GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);

    if (GameRules.VerifyWhetherGameStateIsTerminal(gameState))
    {
        break;
    }

    suggestedAction = randomPlayer.SuggestNextPlayerAction(gameState);
    PrintPlayerAction(suggestedAction, gameState);
    GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);

    PrintGameState(gameState);
    Console.WriteLine("-----------------------");
}

Console.WriteLine(GameRules.GetGameResult(gameState));

static void PrintPlayerAction(IPlayerAction playerAction, GameState gameState)
{
    if (playerAction.GetType() == typeof(PlayerSpawnAction))
    {
        var spawnAction = (PlayerSpawnAction)playerAction;
        Console.WriteLine($"{spawnAction.PieceToSpawn.Color} " +
            $"{spawnAction.PieceToSpawn.GetPieceName()} is spawned at " +
            $"{spawnAction.DestinationCoordinate}.");
    }
    else if (playerAction.GetType() == typeof(PlayerMovementAction))
    {
        var movementAction = (PlayerMovementAction)playerAction;
        var pieceToMove = gameState.CoordinateSystem.GetHexagonAtCoordinate(movementAction.StartCoordinate).PeekPiece();
        Console.WriteLine($"{pieceToMove.Color} " +
            $"{pieceToMove.GetPieceName()} is moved from " +
            $"{movementAction.StartCoordinate} to " +
            $"{movementAction.DestinationCoordinate}.");
    }
    else // Player unable to play
    {
        Console.WriteLine($"{gameState.CurrentPlayerTurnColor} player is unable to play.");
    }

    Console.WriteLine();
}

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

    Console.WriteLine();
}