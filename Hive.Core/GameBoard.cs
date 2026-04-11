using Hive.Core.Models;

namespace Hive.Core
{
    public class GameBoard(ICoordinateSystem coordinateSystem)
    {
        private readonly ICoordinateSystem _coordinateSystem = coordinateSystem;

        public bool TrySpawnPiece((int column, int row) coordinate, IPiece piece)
        {
            var allFreeAdjacentCoordinates = _coordinateSystem.GetAllFreeAdjacentCoordinates();

            var isSpaceAvailable = allFreeAdjacentCoordinates.Contains(coordinate);
            if (!isSpaceAvailable)
            {
                return false;
            }

            // TODO: spawn rules

            var hexagon = new Hexagon();
            hexagon.PushPiece(piece);
            _coordinateSystem.AddHexagonToCoordinate(hexagon, coordinate);

            return true;
        }

        public List<(int column, int row)> GetValidSpawnPoints(PlayerColor playerColor)
        {
            throw new NotImplementedException();

            // TODO: spawn rules
        }

        public bool TryMovePiece((int column, int row) startCoordinate, (int column, int row) endCoordinate)
        {
            throw new NotImplementedException();

            // TODO: move rules
        }

        public List<(int column, int row)> GetValidMovePoints((int column, int row) pieceCoordinate)
        {
            throw new NotImplementedException();

            // TODO: move rules
        }
    }
}
