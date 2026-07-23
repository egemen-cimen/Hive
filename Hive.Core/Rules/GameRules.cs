using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public static class GameRules
    {
        /// <summary>
        /// Returns a game state reset to beginning.
        /// </summary>
        /// <returns></returns>
        public static GameState CreateFreshGameState()
        {
            var freshGameState = new GameState(new AxialCoordinateSystem(), new Stack<IPlayerAction>(), PlayerColor.WHITE, 1);
            return freshGameState;
        }

        /// <summary>
        /// Returns all valid actions from a given game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public static List<IPlayerAction> GetAllAvailablePlayerActions(GameState gameState)
        {
            var allCoordinates = gameState.CoordinateSystem.GetAllCoordinates();
            var allAvailableActions = new List<IPlayerAction>();
            if (allCoordinates.Count == 0)
            {
                List<IPiece> allPossiblePieces = [new AntPiece(PlayerColor.WHITE),
                    new BeetlePiece(PlayerColor.WHITE),
                    new GrasshopperPiece(PlayerColor.WHITE),
                    new QueenPiece(PlayerColor.WHITE),
                    new SpiderPiece(PlayerColor.WHITE)
                    ];

                var spawnCoordinate = (0, 0);

                foreach (var piece in allPossiblePieces)
                {
                    var validationResult = SpawnRules.ValidatePieceSpawn(piece,
                        gameState.CoordinateSystem,
                        spawnCoordinate,
                        gameState.CurrentPlayerTurnColor,
                        gameState.TurnNumber
                        );

                    if (validationResult == SpawnValidationResult.VALID)
                    {
                        allAvailableActions.Add(new PlayerSpawnAction(piece, spawnCoordinate));
                    }
                }
            }

            return allAvailableActions;
        }

        /// <summary>
        /// Checks the given player action against the game state to determine if the action is valid.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerAction"></param>
        /// <returns></returns>
        public static bool VerifyWhetherActionIsLegal(GameState gameState, IPlayerAction playerAction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the result of the game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public static GameResult GetGameResult(GameState gameState)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether the game has concluded or is still ongoing.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public static bool VerifyWhetherGameStateIsTerminal(GameState gameState)
        {
            //var gameResult = GetGameResult(gameState);
            //return gameResult == GameResult.ONGOING;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies the player action to the given game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerAction"></param>
        /// <returns>A new game state but reuses the coordinate system and past move stack from the original.</returns>
        public static GameState ApplyPlayerActionToGameState(GameState gameState, IPlayerAction playerAction)
        {
            if (playerAction.GetType() == typeof(PlayerSpawnAction))
            {
                var spawnAction = (PlayerSpawnAction)playerAction;
                var validationResult = SpawnRules.ValidatePieceSpawn(spawnAction.PieceToSpawn,
                    gameState.CoordinateSystem,
                    spawnAction.DestinationCoordinate,
                    gameState.CurrentPlayerTurnColor,
                    gameState.TurnNumber
                    );

                if (validationResult == SpawnValidationResult.VALID)
                {
                    var hexagonToBeAdded = new Hexagon();
                    hexagonToBeAdded.PushPiece(spawnAction.PieceToSpawn);
                    gameState.CoordinateSystem.AddHexagon(hexagonToBeAdded, spawnAction.DestinationCoordinate);

                    var (nextPlayerColor, nextTurnNumber) = IncrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                    gameState.PastPlayerActions.Push(playerAction);

                    // TODO: consider keeping only one copy of a game state rather than creating new ones.
                    return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, nextPlayerColor, nextTurnNumber);
                }
                else
                {
                    throw new Exception("Player action cannot be applied to the game state.");
                }

            }
            else if (playerAction.GetType() == typeof(PlayerMovementAction))
            {
                throw new NotImplementedException();
            }
            else if (playerAction.GetType() == typeof(PlayerUnableToPlayAction))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception($"Unknown player action {playerAction.GetType()} encountered.");
            }
        }

        /// <summary>
        /// Reverts the last player action from the given game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerAction"></param>
        /// <returns>A new game state but reuses the coordinate system and past move stack from the original.</returns>
        public static GameState UndoLastMoveFromGameState(GameState gameState)
        {
            var playerAction = gameState.PastPlayerActions.Pop(); // TODO: write test case for it

            if (playerAction.GetType() == typeof(PlayerSpawnAction))
            {
                var spawnAction = (PlayerSpawnAction)playerAction;

                gameState.CoordinateSystem.TryGetHexagon(spawnAction.DestinationCoordinate, out var hexagonToBeRemoved);
                hexagonToBeRemoved!.PopPiece();
                gameState.CoordinateSystem.RemoveHexagon(spawnAction.DestinationCoordinate);

                var (previousPlayerColor, previousTurnNumber) = DecrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                // TODO: consider keeping only one copy of a game state rather than creating new ones.
                return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, previousPlayerColor, previousTurnNumber);

            }
            else if (playerAction.GetType() == typeof(PlayerMovementAction))
            {
                throw new NotImplementedException();
            }
            else if (playerAction.GetType() == typeof(PlayerUnableToPlayAction))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception($"Unknown player action {playerAction.GetType()} encountered.");
            }
        }

        private static (PlayerColor playerColor, int turnNumber) IncrementTurnCounter(PlayerColor currentPlayerTurnColor, int currentTurnNumber)
        {
            var nextTurnNumber = currentTurnNumber;
            var nextPlayerColor = currentPlayerTurnColor + 1 % 1;
            if (nextPlayerColor == PlayerColor.WHITE)
            {
                nextTurnNumber++;
            }

            return (nextPlayerColor, nextTurnNumber);
        }

        private static (PlayerColor playerColor, int turnNumber) DecrementTurnCounter(PlayerColor currentPlayerTurnColor, int currentTurnNumber)
        {
            var nextTurnNumber = currentTurnNumber;
            var nextPlayerColor = currentPlayerTurnColor + 1 % 1;
            if (nextPlayerColor == PlayerColor.WHITE)
            {
                nextTurnNumber--;
            }

            return (nextPlayerColor, nextTurnNumber);
        }
    }
}
