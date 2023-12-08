using YellowSquad.HexMath;
using YellowSquad.Anthill.Core.HexMap;

namespace YellowSquad.Anthill.Core.AStarPathfinding.Adapters
{
    public class MapMovePolicy : IMovePolicy
    {
        private readonly IHexMap _hexMap;

        public MapMovePolicy(IHexMap hexMap)
        {
            _hexMap = hexMap;
        }
        
        public bool CanMove(AxialCoordinate axialCoordinate)
        {
            return _hexMap.HasPosition(axialCoordinate) && 
                   _hexMap.HasObstacleIn(axialCoordinate) == false;
        }
    }
}
