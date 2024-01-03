using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultAntFactory : IAntFactory
    {
        private readonly IPath _path;
        private readonly MovementSettings _movementSettings;

        public DefaultAntFactory(IPath path, MovementSettings movementSettings)
        {
            _path = path;
            _movementSettings = movementSettings;
        }
        
        public IAnt CreateDigger(IHome home, AxialCoordinate startPosition)
        {
            return new Ant(home, new DefaultMovement(_path, _movementSettings, startPosition));
        }

        public IAnt CreateLoader(IHome home, AxialCoordinate startPosition)
        {
            return new Ant(home, new DefaultMovement(_path, _movementSettings, startPosition));
        }
    }
}