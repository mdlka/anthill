using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultAntFactory : IAntFactory
    {
        private readonly IPath _path;
        private readonly float _moveDelay;
        private readonly int _stepsToGoal;

        public DefaultAntFactory(IPath path, float moveDelay, int stepsToGoal)
        {
            _path = path;
            _moveDelay = moveDelay;
            _stepsToGoal = stepsToGoal;
        }
        
        public IAnt CreateDigger(IHome home, AxialCoordinate startPosition)
        {
            return new Ant(home, new DefaultMovement(_moveDelay, _stepsToGoal, _path, startPosition));
        }

        public IAnt CreateLoader(IHome home, AxialCoordinate startPosition)
        {
            return new Ant(home, new DefaultMovement(_moveDelay, _stepsToGoal, _path, startPosition));
        }
    }
}