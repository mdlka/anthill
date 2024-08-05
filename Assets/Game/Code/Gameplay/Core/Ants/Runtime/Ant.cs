using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class Ant : IAnt
    {
        private readonly AlwaysCompletedTask _alwaysCompletedTask = new();
        private readonly IHome _home;
        private readonly ITaskStore _taskStore;
        private readonly IMovement _movement;

        private ITask _currentTask;
        private bool _taskSold;
        private bool _lastTaskCancelled = true;

        public Ant(IHome home, IMovement movement, ITaskStore taskStore) : this(home, movement, taskStore, new AlwaysCompletedTask()) { }

        private Ant(IHome home, IMovement movement, ITaskStore taskStore, ITask startTask)
        {
            _home = home;
            _taskStore = taskStore;
            _movement = movement;
            _currentTask = startTask;
        }

        public bool HasPart { get; private set; }
        public FracAxialCoordinate CurrentPosition => _movement.CurrentPosition;
        public bool Moving => _movement.ReachedTargetPosition == false;
        public bool InHome => CurrentPosition.AxialRound() == _home.Position;

        public void Update(float deltaTime)
        {
            if (_currentTask.Completed)
            {
                if (_movement.ReachedTargetPosition)
                {
                    if (InHome == false)
                    {
                        _movement.MoveTo(_home.Position);
                        HasPart = !_lastTaskCancelled;
                        return;
                    }

                    if (_taskSold == false)
                    {
                        _taskStore.Sell(_currentTask, _home.Position);
                        _taskSold = true;
                        HasPart = false;
                    }
                    
                    if (_home.HasFreeTaskGroup == false)
                        return;

                    _taskSold = false;
                    
                    var taskGroup = _home.FindTaskGroup();
                    _movement.MoveTo(taskGroup.TargetCellPosition, position =>
                    {
                        _currentTask = taskGroup.TakeClosestTask(position);
                        return _currentTask.TargetPosition;
                    });
                }
            }
            else
            {
                if (_movement.ReachedTargetPosition)
                {
                    _lastTaskCancelled = _currentTask.Cancelled;
                    
                    if (_currentTask.Cancelled)
                        _currentTask = _alwaysCompletedTask;

                    if (_currentTask.Completed)
                    {
                        _movement.MoveTo(_home.Position);
                        HasPart = !_lastTaskCancelled;
                    }
                    else
                    {
                        _currentTask.UpdateProgress(deltaTime);
                    }
                }
            }
            
            _movement.Update(deltaTime);
        }
    }
}
