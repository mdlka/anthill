using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class Ant : IAnt
    {
        private readonly IHome _home;
        private readonly IMovement _movement;

        private ITask _currentTask;

        public Ant(IHome home, IMovement movement) : this(home, movement, new AlwaysCompletedTask()) { }

        private Ant(IHome home, IMovement movement, ITask startTask)
        {
            _home = home;
            _movement = movement;
            _currentTask = startTask;
        }

        public AxialCoordinate CurrentPosition => _movement.CurrentPosition;
        public bool Moving => _movement.ReachedTargetPosition == false;

        public void Update(float deltaTime)
        {
            if (_currentTask.Completed)
            {
                if (_movement.ReachedTargetPosition)
                {
                    if (_home.HasTask == false)
                    {
                        if (CurrentPosition == _home.Position)
                            return;
                        
                        _movement.MoveTo(_home.Position);
                        return;
                    }

                    _currentTask = _home.FindTask();
                    _movement.MoveTo(_currentTask.TargetPosition);
                }

                _movement.Update(deltaTime);
            }
            else
            {
                if (_movement.ReachedTargetPosition)
                {
                    _currentTask.Complete();
                    _movement.MoveTo(_home.Position);
                }

                _movement.Update(deltaTime);
            }
        }
    }
}
