using Hive.Core.Models;

namespace Hive.Core.Rules
{
    public static class CommonMovementRules
    {
        public static MovementValidationResult ValidateCommonMovementRules<T>(ICoordinateSystem coordinateSystem,
            (int column, int row) startCoordinate,
            (int column, int row) destinationCoordinate,
            PlayerColor playerTurnColor
            )
        {
            if (startCoordinate == destinationCoordinate)
            {
                return MovementValidationResult.START_AND_DESTINATION_CANNOT_BE_THE_SAME;
            }

            var isStartHexagonExists = coordinateSystem.TryGetHexagon(startCoordinate, out var hexagonAtStart);
            if (!isStartHexagonExists)
            {
                return MovementValidationResult.NO_PIECE_TO_MOVE;
            }

            var topPiece = hexagonAtStart!.PeekPiece();
            if (topPiece is not T)
            {
                return MovementValidationResult.WRONG_VALIDATOR_FOR_PIECE_TYPE;
            }

            if (topPiece.Color != playerTurnColor)
            {
                return MovementValidationResult.WRONG_COLOR_MOVED;
            }

            // Validation for non-beetle pieces
            if (typeof(T) != typeof(BeetlePiece))
            {
                if (coordinateSystem.TryGetHexagon(destinationCoordinate, out _))
                {
                    return MovementValidationResult.DESTINATION_IS_NOT_EMPTY;
                }

                if (!coordinateSystem.VerifyWhetherAllHexagonsConnectedWithoutHexagon(startCoordinate))
                {
                    return MovementValidationResult.BREAKS_ONE_HIVE;
                }

                var allFreeAdjacentCoordinatesWithout = coordinateSystem.GetAllFreeAdjacentCoordinatesWithoutHexagon(startCoordinate);
                if (!allFreeAdjacentCoordinatesWithout.Contains(destinationCoordinate))
                {
                    return MovementValidationResult.BREAKS_ONE_HIVE;
                }
            }

            // Validation for non-queen pieces
            if (typeof(T) != typeof(QueenPiece))
            {
                var queenExists = RulesHelper.VerifyWhetherQueenIsSpawned(coordinateSystem, playerTurnColor);
                if (!queenExists)
                {
                    return MovementValidationResult.CANNOT_MOVE_WITHOUT_QUEEN;
                }
            }

            return MovementValidationResult.VALID;
        }
    }
}
