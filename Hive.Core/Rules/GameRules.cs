using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public static class GameRules
    {
        private static readonly Dictionary<Type, IMovementRules> _movementRules = new()
        {
            { typeof(AntPiece), new AntMovementRules() },
            { typeof(BeetlePiece), new BeetleMovementRules() },
            { typeof(GrasshopperPiece), new GrasshopperMovementRules() },
            { typeof(QueenPiece), new QueenMovementRules() },
            { typeof(SpiderPiece), new SpiderMovementRules() }
        };

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
            if (VerifyWhetherGameStateIsTerminal(gameState))
            {
                return [];
            }

            var allAvailableActions = new List<IPlayerAction>();

            List<IPiece> allPossibleSpawnPieces = [new AntPiece(gameState.CurrentPlayerTurnColor),
                new BeetlePiece(gameState.CurrentPlayerTurnColor),
                new GrasshopperPiece(gameState.CurrentPlayerTurnColor),
                new QueenPiece(gameState.CurrentPlayerTurnColor),
                new SpiderPiece(gameState.CurrentPlayerTurnColor)
                ];

            var possibleSpawnCoordinates = gameState.CoordinateSystem.GetAllFreeAdjacentCoordinates();
            foreach (var spawnCoordinate in possibleSpawnCoordinates)
            {
                AddValidSpawnPlayerActions(gameState, allAvailableActions, allPossibleSpawnPieces, spawnCoordinate);
            }

            var allCoordinates = gameState.CoordinateSystem.GetAllCoordinates();
            foreach (var coordinate in allCoordinates)
            {
                gameState.CoordinateSystem.TryGetHexagon(coordinate, out var populatedHexagon);
                var piece = populatedHexagon!.PeekPiece();
                IEnumerable<(int column, int row)> possibleDestinations;

                // TODO: Fix inefficencies by implementing "GetAllAvailableMovements" method in *MovementRules.
                if (piece.GetType() == typeof(BeetlePiece))
                {
                    var possibleFirstLevelDestinations = gameState.CoordinateSystem.GetAllFreeAdjacentCoordinates();
                    var possibleStackingDestinations = gameState.CoordinateSystem.GetAllCoordinates();
                    possibleDestinations = possibleFirstLevelDestinations.Concat(possibleStackingDestinations);
                }
                else
                {
                    possibleDestinations = gameState.CoordinateSystem.GetAllFreeAdjacentCoordinates();
                }

                _movementRules.TryGetValue(piece.GetType(), out var movementRules);
                foreach (var destination in possibleDestinations)
                {
                    var validationResult = movementRules!.ValidatePieceMovement(gameState.CoordinateSystem, coordinate, destination, gameState.CurrentPlayerTurnColor);
                    if (validationResult == MovementValidationResult.VALID)
                    {
                        allAvailableActions.Add(new PlayerMovementAction(coordinate, destination));
                    }
                }
            }

            if (allAvailableActions.Count == 0)
            {
                allAvailableActions.Add(new PlayerUnableToPlayAction());
            }

            return allAvailableActions;
        }

        private static void AddValidSpawnPlayerActions(GameState gameState,
            List<IPlayerAction> allAvailableActions,
            List<IPiece> allPossibleSpawnPieces,
            (int, int) spawnCoordinate
            )
        {
            foreach (var piece in allPossibleSpawnPieces)
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

        /// <summary>
        /// Returns the result of the game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public static GameResult GetGameResult(GameState gameState)
        {
            var (whiteQueenExists, whiteQueenCoordinate) = RulesHelper.VerifyWhetherQueenIsSpawned(gameState.CoordinateSystem, PlayerColor.WHITE);
            var (blackQueenExists, blackQueenCoordinate) = RulesHelper.VerifyWhetherQueenIsSpawned(gameState.CoordinateSystem, PlayerColor.BLACK);

            if (!whiteQueenExists || !blackQueenExists)
            {
                return GameResult.ONGOING;
            }

            var numberOfPiecesAroundWhiteQueen = gameState.CoordinateSystem.GetPopulatedNeighborHexagons(whiteQueenCoordinate!.Value).Count;
            var numberOfPiecesAroundBlackQueen = gameState.CoordinateSystem.GetPopulatedNeighborHexagons(blackQueenCoordinate!.Value).Count;

            if (numberOfPiecesAroundWhiteQueen == 6 && numberOfPiecesAroundBlackQueen == 6)
            {
                return GameResult.DRAW;
            }
            else if (numberOfPiecesAroundWhiteQueen == 6)
            {
                return GameResult.BLACK_WON;
            }
            else if (numberOfPiecesAroundBlackQueen == 6)
            {
                return GameResult.WHITE_WON;
            }

            return GameResult.ONGOING;
        }

        /// <summary>
        /// Checks whether the game has concluded or is still ongoing.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns></returns>
        public static bool VerifyWhetherGameStateIsTerminal(GameState gameState)
        {
            var gameResult = GetGameResult(gameState);
            return gameResult != GameResult.ONGOING;
        }

        /// <summary>
        /// Applies the player action to the given game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerAction"></param>
        /// <returns>A new game state but reuses the coordinate system and past move stack from the original.</returns>
        public static GameState ApplyPlayerActionToGameState(GameState gameState, IPlayerAction playerAction)
        {
            if (!VerifyWhetherActionIsLegal(gameState, playerAction))
            {
                throw new Exception("Player action cannot be applied to the game state.");
            }

            if (playerAction.GetType() == typeof(PlayerSpawnAction))
            {
                var spawnAction = (PlayerSpawnAction)playerAction;
                var hexagonToBeAdded = new Hexagon();
                hexagonToBeAdded.PushPiece(spawnAction.PieceToSpawn);
                gameState.CoordinateSystem.AddHexagon(hexagonToBeAdded, spawnAction.DestinationCoordinate);

                var (nextPlayerColor, nextTurnNumber) = IncrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                gameState.PastPlayerActions.Push(playerAction);

                // TODO: Consider keeping only one copy of a game state rather than creating new ones. Do this for all occurrences in this class.
                return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, nextPlayerColor, nextTurnNumber);

            }
            else if (playerAction.GetType() == typeof(PlayerMovementAction))
            {
                var movementAction = (PlayerMovementAction)playerAction;

                MovePiece(gameState.CoordinateSystem, movementAction.StartCoordinate, movementAction.DestinationCoordinate);

                var (nextPlayerColor, nextTurnNumber) = IncrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                gameState.PastPlayerActions.Push(playerAction);

                return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, nextPlayerColor, nextTurnNumber);
            }
            else // Player unable to play
            {
                var (nextPlayerColor, nextTurnNumber) = IncrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                gameState.PastPlayerActions.Push(playerAction);

                return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, nextPlayerColor, nextTurnNumber);
            }
        }

        /// <summary>
        /// Checks the given player action against the game state to determine if the action is valid.
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="playerAction"></param>
        /// <returns></returns>
        private static bool VerifyWhetherActionIsLegal(GameState gameState, IPlayerAction playerAction)
        {
            if (VerifyWhetherGameStateIsTerminal(gameState))
            {
                return false;
            }

            if (playerAction.GetType() == typeof(PlayerSpawnAction))
            {
                var spawnAction = (PlayerSpawnAction)playerAction;
                var validationResult = SpawnRules.ValidatePieceSpawn(spawnAction.PieceToSpawn,
                    gameState.CoordinateSystem,
                    spawnAction.DestinationCoordinate,
                    gameState.CurrentPlayerTurnColor,
                    gameState.TurnNumber
                    );

                return validationResult == SpawnValidationResult.VALID;
            }
            else if (playerAction.GetType() == typeof(PlayerMovementAction))
            {
                var movementAction = (PlayerMovementAction)playerAction;
                gameState.CoordinateSystem.TryGetHexagon(movementAction.StartCoordinate, out var startHexagon);
                var pieceToMove = startHexagon!.PeekPiece();

                _movementRules.TryGetValue(pieceToMove.GetType(), out var movementRules);

                var validationResult = movementRules!.ValidatePieceMovement(gameState.CoordinateSystem,
                    movementAction.StartCoordinate,
                    movementAction.DestinationCoordinate,
                    gameState.CurrentPlayerTurnColor
                    );

                return validationResult == MovementValidationResult.VALID;
            }
            else // Player unable to play
            {
                var allAvailablePlayerActions = GetAllAvailablePlayerActions(gameState);
                if (allAvailablePlayerActions.Count > 1 && allAvailablePlayerActions[0].GetType() != typeof(PlayerUnableToPlayAction))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Reverts the last player action from the given game state.
        /// </summary>
        /// <param name="gameState"></param>
        /// <returns>A new game state but reuses the coordinate system and past move stack from the original.</returns>
        public static GameState UndoLastMoveFromGameState(GameState gameState)
        {
            var playerAction = gameState.PastPlayerActions.Pop();

            if (playerAction.GetType() == typeof(PlayerSpawnAction))
            {
                var spawnAction = (PlayerSpawnAction)playerAction;

                gameState.CoordinateSystem.TryGetHexagon(spawnAction.DestinationCoordinate, out var hexagonToBeRemoved);
                hexagonToBeRemoved!.PopPiece();
                gameState.CoordinateSystem.RemoveHexagon(spawnAction.DestinationCoordinate);

                var (previousPlayerColor, previousTurnNumber) = DecrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, previousPlayerColor, previousTurnNumber);
            }
            else if (playerAction.GetType() == typeof(PlayerMovementAction))
            {
                var movementAction = (PlayerMovementAction)playerAction;

                MovePiece(gameState.CoordinateSystem, movementAction.DestinationCoordinate, movementAction.StartCoordinate);

                var (previousPlayerColor, previousTurnNumber) = DecrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, previousPlayerColor, previousTurnNumber);
            }
            else // Player unable to play
            {
                var (previousPlayerColor, previousTurnNumber) = DecrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, previousPlayerColor, previousTurnNumber);
            }
        }

        private static void MovePiece(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate
            )
        {
            coordinateSystem.TryGetHexagon(startCoordinate, out var startHexagon);

            var pieceToMove = startHexagon!.PeekPiece();
            if (!coordinateSystem.TryGetHexagon(destinationCoordinate, out var destinationHexagon))
            {
                destinationHexagon = new Hexagon();
                coordinateSystem.AddHexagon(destinationHexagon, destinationCoordinate);
            }

            destinationHexagon!.PushPiece(pieceToMove);
            startHexagon.PopPiece();
            if (startHexagon.GetPieceCount() == 0)
            {
                coordinateSystem.RemoveHexagon(startCoordinate);
            }
        }

        private static (PlayerColor playerColor, int turnNumber) IncrementTurnCounter(PlayerColor currentPlayerTurnColor, int currentTurnNumber)
        {
            var nextTurnNumber = currentTurnNumber;

            var nextPlayerColor = currentPlayerTurnColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;

            if (nextPlayerColor == PlayerColor.WHITE)
            {
                nextTurnNumber++;
            }

            return (nextPlayerColor, nextTurnNumber);
        }

        private static (PlayerColor playerColor, int turnNumber) DecrementTurnCounter(PlayerColor currentPlayerTurnColor, int currentTurnNumber)
        {
            var nextTurnNumber = currentTurnNumber;

            var nextPlayerColor = currentPlayerTurnColor == PlayerColor.WHITE ? PlayerColor.BLACK : PlayerColor.WHITE;

            if (nextPlayerColor == PlayerColor.BLACK)
            {
                nextTurnNumber--;
            }

            return (nextPlayerColor, nextTurnNumber);
        }
    }
}
