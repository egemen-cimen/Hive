using Hive.Core.Models;
using Hive.Core.Rules;

namespace Hive.PlayerAgent
{
    public class MinimaxPlayer : IPlayerAgent
    {
        private const int WINNER_VALUATION = 100;
        private const int DRAW_VALUATION = 0;
        private const int QUEEN_NOT_PLAYED_PENALTY = -10;
        private const int PIECE_NEXT_TO_QUEEN_PENALY = -10;
        private const int QUEEN_CANNOT_MOVE_PENALTY = -10;
        private static readonly QueenMovementRules _queenMovementRules = new();
        private const int MAX_TREE_DEPTH = 2;
        private int _gameStatesEvaluated = 0;

        public IPlayerAction SuggestNextPlayerAction(GameState gameState)
        {
            _gameStatesEvaluated = 0;
            var allAvailablePlayerActions = GameRules.GetAllAvailablePlayerActions(gameState);
            PriorityQueue<IPlayerAction, int> playerActionsAndUtilityValues;
            if (CurrentPlayerIsMaximizer(gameState))
            {
                playerActionsAndUtilityValues = new PriorityQueue<IPlayerAction, int>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
            }
            else
            {
                playerActionsAndUtilityValues = new PriorityQueue<IPlayerAction, int>();
            }

            foreach (var playerAction in allAvailablePlayerActions)
            {
                GameRules.ApplyPlayerActionToGameState(gameState, playerAction);

                var value = Minimax(gameState, MAX_TREE_DEPTH);
                playerActionsAndUtilityValues.Enqueue(playerAction, value);

                GameRules.UndoLastMoveFromGameState(gameState);
            }

            return playerActionsAndUtilityValues.Dequeue();
        }

        /// <summary>
        /// Retrieves the number of game states evaluated in the last run
        /// </summary>
        /// <returns>Number of game states evaluated</returns>
        public int GetEvaluationCount()
        {
            return _gameStatesEvaluated;
        }

        private static bool CurrentPlayerIsMaximizer(GameState gameState)
        {
            return gameState.CurrentPlayerTurnColor == PlayerColor.WHITE;
        }

        private int Minimax(GameState gameState, int maxTreeDepth)
        {
            if (maxTreeDepth == 0 || GameRules.VerifyWhetherGameStateIsTerminal(gameState))
            {
                return EvaluateValueOfGameState(gameState);
            }

            if (CurrentPlayerIsMaximizer(gameState))
            {
                var value = int.MinValue;
                foreach (var playerAction in GameRules.GetAllAvailablePlayerActions(gameState))
                {
                    GameRules.ApplyPlayerActionToGameState(gameState, playerAction);
                    value = int.Max(value, Minimax(gameState, maxTreeDepth - 1));
                    GameRules.UndoLastMoveFromGameState(gameState);
                }

                return value;
            }
            else
            {
                var value = int.MaxValue;
                foreach (var playerAction in GameRules.GetAllAvailablePlayerActions(gameState))
                {
                    GameRules.ApplyPlayerActionToGameState(gameState, playerAction);
                    value = int.Min(value, Minimax(gameState, maxTreeDepth - 1));
                    GameRules.UndoLastMoveFromGameState(gameState);
                }

                return value;
            }
        }

        private int EvaluateValueOfGameState(GameState gameState)
        {
            _gameStatesEvaluated++;
            var gameResult = GameRules.GetGameResult(gameState);
            switch (gameResult)
            {
                case GameResult.WHITE_WON:
                    return WINNER_VALUATION;
                case GameResult.BLACK_WON:
                    return -WINNER_VALUATION;
                case GameResult.DRAW:
                    return DRAW_VALUATION;
            }

            var valuation = 0;
            var (whiteQueenExists, whiteQueenCoordinate) = RulesHelper.VerifyWhetherQueenIsSpawned(gameState.CoordinateSystem, PlayerColor.WHITE);
            var (blackQueenExists, blackQueenCoordinate) = RulesHelper.VerifyWhetherQueenIsSpawned(gameState.CoordinateSystem, PlayerColor.BLACK);

            if (!whiteQueenExists)
            {
                valuation += QUEEN_NOT_PLAYED_PENALTY;
            }
            else
            {
                var numberOfPiecesAroundWhiteQueen = gameState.CoordinateSystem.GetPopulatedNeighborHexagons(whiteQueenCoordinate!.Value).Count;
                valuation += numberOfPiecesAroundWhiteQueen * PIECE_NEXT_TO_QUEEN_PENALY;

                if (!VerifyWhetherQueenCanMove(gameState, whiteQueenCoordinate.Value, PlayerColor.WHITE))
                {
                    valuation += QUEEN_CANNOT_MOVE_PENALTY;
                }
            }

            if (!blackQueenExists)
            {
                valuation -= QUEEN_NOT_PLAYED_PENALTY;
            }
            else
            {
                var numberOfPiecesAroundBlackQueen = gameState.CoordinateSystem.GetPopulatedNeighborHexagons(blackQueenCoordinate!.Value).Count;
                valuation -= numberOfPiecesAroundBlackQueen * PIECE_NEXT_TO_QUEEN_PENALY;

                if (!VerifyWhetherQueenCanMove(gameState, blackQueenCoordinate.Value, PlayerColor.WHITE))
                {
                    valuation -= QUEEN_CANNOT_MOVE_PENALTY;
                }
            }

            return valuation;

            static bool VerifyWhetherQueenCanMove(GameState gameState, (int column, int row) queenCoordinate, PlayerColor queenColor)
            {
                var queenCanMove = false;
                foreach (var adjacentHexagon in gameState.CoordinateSystem.GetAdjacentCoordinates(queenCoordinate))
                {
                    if (_queenMovementRules.ValidatePieceMovement(gameState.CoordinateSystem,
                        queenCoordinate,
                        adjacentHexagon,
                        queenColor) == MovementValidationResult.VALID
                        )
                    {
                        queenCanMove = true;
                        break;
                    }
                }

                return queenCanMove;
            }
        }
    }
}
