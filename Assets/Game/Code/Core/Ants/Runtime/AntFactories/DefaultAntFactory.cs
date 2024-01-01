using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultAntFactory : IAntFactory
    {
        private readonly IPath _path;
        private readonly float _moveDelay;

        public DefaultAntFactory(IPath path, float moveDelay)
        {
            _path = path;
            _moveDelay = moveDelay;
        }
        
        public IAnt CreateDigger(IHome home, AxialCoordinate startPosition)
        {
            return new Ant(home, new RoughMovement(_moveDelay, _path, startPosition));
        }

        public IAnt CreateLoader(IHome home, AxialCoordinate startPosition)
        {
            return new Ant(home, new RoughMovement(_moveDelay, _path, startPosition));
        }
    }
}