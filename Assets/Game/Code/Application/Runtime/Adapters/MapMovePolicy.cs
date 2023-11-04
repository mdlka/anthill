using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Application
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
