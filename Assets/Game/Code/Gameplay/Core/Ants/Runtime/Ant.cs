using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class Ant : IAnt
    {
        private readonly IHome _home;
        private readonly ITaskStore _taskStore;
        private readonly IMovement _movement;

        private ITask _currentTask;
        private bool _taskSold;

        public Ant(IHome home, IMovement movement, ITaskStore taskStore) : this(home, movement, taskStore, new AlwaysCompletedTask()) { }

        private Ant(IHome home, IMovement movement, ITaskStore taskStore, ITask startTask)
        {
            _home = home;
            _taskStore = taskStore;
            _movement = movement;
            _currentTask = startTask;
        }

        public FracAxialCoordinate CurrentPosition => _movement.CurrentPosition;
        public bool Moving => _movement.ReachedTargetPosition == false;
        private bool InHome => CurrentPosition.AxialRound() == _home.Position;

        public void Update(float deltaTime)
        {
            if (_currentTask.State == TaskState.Complete)
            {
                if (_movement.ReachedTargetPosition)
                {
                    if (InHome == false)
                    {
                        _movement.MoveTo(_home.Position);
                        return;
                    }

                    if (_taskSold == false)
                    {
                        _taskStore.Sell(_currentTask);
                        _taskSold = true;
                    }
                    
                    if (_home.HasFreeTaskGroup == false)
                        return;

                    _taskSold = false;
                    
                    var taskGroup = _home.FindTaskGroup();
                    _movement.MoveTo(taskGroup.TargetCellPosition, position =>
                    {
                        _currentTask = taskGroup.ClosestTask(position);
                        return _currentTask.TargetPosition;
                    });
                }
            }
            else
            {
                if (_movement.ReachedTargetPosition)
                {
                    if (_currentTask.State == TaskState.Idle)
                        _currentTask.Execute();

                    if (_currentTask.CanComplete)
                    {
                        _currentTask.Complete();
                        _movement.MoveTo(_home.Position);
                    }
                }
            }
            
            _movement.Update(deltaTime);
        }
    }
}
