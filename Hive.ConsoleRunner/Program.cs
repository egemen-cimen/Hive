using Hive.Core.Models;
using Hive.Core.Rules;
using Hive.PlayerAgent;

var minimaxPlayer = new MinimaxPlayer();

var gameState = GameRules.CreateFreshGameState();

var playerStartsFirst = true;
Console.WriteLine("Do you want to start first? Y/n");
var input = Console.ReadLine()!.Trim();
if (input == "n" || input == "N")
{
    playerStartsFirst = false;
    Console.WriteLine("Computer starts first.");
}
else
{
    Console.WriteLine("You start first.");
}


while (!GameRules.VerifyWhetherGameStateIsTerminal(gameState))
{
    AllowPlayerMove(minimaxPlayer, gameState, playerStartsFirst);
    PrintGameState(gameState);

    if (GameRules.VerifyWhetherGameStateIsTerminal(gameState))
    {
        break;
    }

    AllowPlayerMove(minimaxPlayer, gameState, !playerStartsFirst);
    PrintGameState(gameState);
    Console.WriteLine("-----------------------");
}

Console.WriteLine(GameRules.GetGameResult(gameState));

static IPlayerAction GetPlayerActionFromConsole(GameState gameState)
{
    var allPossibleActions = GameRules.GetAllAvailablePlayerActions(gameState).ToArray();

    Console.WriteLine("Please select your next move: player action index - player action");

    for (int i = 0; i < allPossibleActions.Length; i++)
    {
        Console.WriteLine($"{i,5} - {GetPlayerActionString(allPossibleActions[i], gameState)}");
    }

    var validSelection = false;
    var selection = -1;

    while (!validSelection)
    {
        Console.WriteLine("Your selection:");
        var input = Console.ReadLine();
        if (int.TryParse(input, out int actionIndex) && actionIndex >= 0 && actionIndex < allPossibleActions.Length)
        {
            Console.WriteLine($"Chose {actionIndex} - {GetPlayerActionString(allPossibleActions[actionIndex], gameState)}");
            validSelection = true;
            selection = actionIndex;
        }
        else
        {
            Console.WriteLine("Invalid player action. Please enter a valid action index.");
        }
    }

    return allPossibleActions[selection];
}

static string GetPlayerActionString(IPlayerAction playerAction, GameState gameState)
{
    if (playerAction.GetType() == typeof(PlayerSpawnAction))
    {
        var spawnAction = (PlayerSpawnAction)playerAction;
        return $"{spawnAction.PieceToSpawn.Color} " +
            $"{spawnAction.PieceToSpawn.GetPieceName()} is spawned at " +
            $"{spawnAction.DestinationCoordinate}.";
    }
    else if (playerAction.GetType() == typeof(PlayerMovementAction))
    {
        var movementAction = (PlayerMovementAction)playerAction;
        var pieceToMove = gameState.CoordinateSystem.GetHexagonAtCoordinate(movementAction.StartCoordinate).PeekPiece();
        return $"{pieceToMove.Color} " +
            $"{pieceToMove.GetPieceName()} is moved from " +
            $"{movementAction.StartCoordinate} to " +
            $"{movementAction.DestinationCoordinate}.";
    }

    // Player unable to play
    return $"{gameState.CurrentPlayerTurnColor} player is unable to play.";
}

static void PrintPlayerAction(IPlayerAction playerAction, GameState gameState)
{
    Console.WriteLine(GetPlayerActionString(playerAction, gameState));
    Console.WriteLine();
}

static void PrintGameState(GameState gameState)
{
    Console.WriteLine("vvvvvvvvvvvvvvvvvvvvvvv");
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

    Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^");
    Console.WriteLine();
}

static void AllowPlayerMove(MinimaxPlayer minimaxPlayer, GameState gameState, bool playerStartsFirst)
{
    if (playerStartsFirst)
    {
        //var randomPlayer = new RandomPlayer();
        //var suggestedAction = randomPlayer.SuggestNextPlayerAction(gameState);
        var suggestedAction = GetPlayerActionFromConsole(gameState);
        PrintPlayerAction(suggestedAction, gameState);
        GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);
    }
    else
    {
        var suggestedAction = minimaxPlayer.SuggestNextPlayerAction(gameState);
        PrintPlayerAction(suggestedAction, gameState);
        GameRules.ApplyPlayerActionToGameState(gameState, suggestedAction);
    }
}