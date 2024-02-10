using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultAntFactory : IAntFactory
    {
        private readonly MovementPath _path;
        private readonly MovementSettings _movementSettings;
        private readonly ITaskStore _taskStore;

        public DefaultAntFactory(MovementPath path, MovementSettings movementSettings, ITaskStore taskStore)
        {
            _path = path;
            _movementSettings = movementSettings;
            _taskStore = taskStore;
        }
        
        public IAnt CreateDigger(IHome home, AxialCoordinate startPosition)
        {
            return new Ant(home, new DefaultMovement(_path, _movementSettings, startPosition), _taskStore);
        }

        public IAnt CreateLoader(IHome home, AxialCoordinate startPosition)
        {
            return new Ant(home, new DefaultMovement(_path, _movementSettings, startPosition), _taskStore);
        }
    }
}