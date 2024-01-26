using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class Ant : IAnt
    {
        private const float Delay = 0.5f;
        
        private readonly IHome _home;
        private readonly IMovement _movement;

        private ITask _currentTask;
        private float _reachedTaskPositionTime = -1f;

        public Ant(IHome home, IMovement movement) : this(home, movement, new AlwaysCompletedTask()) { }

        private Ant(IHome home, IMovement movement, ITask startTask)
        {
            _home = home;
            _movement = movement;
            _currentTask = startTask;
        }

        public FracAxialCoordinate CurrentPosition => _movement.CurrentPosition;
        public bool Moving => _movement.ReachedTargetPosition == false;

        public void Update(float deltaTime)
        {
            if (_currentTask.Completed)
            {
                if (_movement.ReachedTargetPosition)
                {
                    if (_home.HasTask == false)
                    {
                        if (CurrentPosition.AxialRound() == _home.Position)
                            return;
                        
                        _movement.MoveTo(_home.Position);
                        return;
                    }

                    _currentTask = _home.FindTask();
                    _movement.MoveTo(_currentTask.TargetCellPosition);
                }
            }
            else
            {
                if (_movement.ReachedTargetPosition)
                {
                    if (_reachedTaskPositionTime < 0)
                        _reachedTaskPositionTime = Time.realtimeSinceStartup;

                    if (Time.realtimeSinceStartup - _reachedTaskPositionTime >= Delay)
                    {
                        _currentTask.Complete(CurrentPosition);
                        _movement.MoveTo(_home.Position);
                        _reachedTaskPositionTime = -1;
                    }
                }
            }
            
            _movement.Update(deltaTime);
        }
    }
}
