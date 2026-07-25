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

                if (!_movementRules.TryGetValue(piece.GetType(), out var movementRules))
                {
                    throw new Exception("Unsupported piece type encountered.");
                }

                // TODO: fix inefficencies by implementing "GetAllAvailableMovements" method in *MovementRules
                if (piece.GetType() == typeof(AntPiece))
                {
                    possibleDestinations = gameState.CoordinateSystem.GetAllFreeAdjacentCoordinates();
                }
                else if (piece.GetType() == typeof(BeetlePiece))
                {
                    var possibleFirstLevelDestinations = gameState.CoordinateSystem.GetAllFreeAdjacentCoordinates();
                    var possibleStackingDestinations = gameState.CoordinateSystem.GetAllCoordinates();
                    possibleDestinations = possibleFirstLevelDestinations.Concat(possibleStackingDestinations);
                }
                else if (piece.GetType() == typeof(GrasshopperPiece))
                {
                    possibleDestinations = gameState.CoordinateSystem.GetAllFreeAdjacentCoordinates();
                }
                else if (piece.GetType() == typeof(QueenPiece))
                {
                    possibleDestinations = gameState.CoordinateSystem.GetAllFreeAdjacentCoordinates();
                }
                else
                {
                    possibleDestinations = gameState.CoordinateSystem.GetAllFreeAdjacentCoordinates();
                }

                foreach (var destination in possibleDestinations)
                {
                    var validationResult = movementRules.ValidatePieceMovement(gameState.CoordinateSystem, coordinate, destination, gameState.CurrentPlayerTurnColor);
                    if (validationResult == MovementValidationResult.VALID)
                    {
                        allAvailableActions.Add(new PlayerMovementAction(coordinate, destination));
                    }
                }

            }

            // TODO: return no valid player move if allAvailableActions is empty

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
                var movementAction = (PlayerMovementAction)playerAction;
                if (!gameState.CoordinateSystem.TryGetHexagon(movementAction.StartCoordinate, out var startHexagon))
                {
                    throw new Exception("Piece selected for movement is not found.");
                }
                var pieceToMove = startHexagon!.PeekPiece();

                if (!_movementRules.TryGetValue(pieceToMove.GetType(), out var movementRules))
                {
                    throw new Exception("Unsupported piece type encountered.");
                }

                var validationResult = movementRules.ValidatePieceMovement(gameState.CoordinateSystem,
                    movementAction.StartCoordinate,
                    movementAction.DestinationCoordinate,
                    gameState.CurrentPlayerTurnColor
                    );

                if (validationResult == MovementValidationResult.VALID)
                {
                    MovePiece(gameState.CoordinateSystem, movementAction.StartCoordinate, movementAction.DestinationCoordinate);

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
                var movementAction = (PlayerMovementAction)playerAction;

                MovePiece(gameState.CoordinateSystem, movementAction.DestinationCoordinate, movementAction.StartCoordinate);

                var (previousPlayerColor, previousTurnNumber) = DecrementTurnCounter(gameState.CurrentPlayerTurnColor, gameState.TurnNumber);

                // TODO: consider keeping only one copy of a game state rather than creating new ones.
                return new GameState(gameState.CoordinateSystem, gameState.PastPlayerActions, previousPlayerColor, previousTurnNumber);
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

        private static void MovePiece(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate
            )
        {
            if (!coordinateSystem.TryGetHexagon(startCoordinate, out var startHexagon))
            {
                throw new Exception("Piece selected for movement is not found.");
            }

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
