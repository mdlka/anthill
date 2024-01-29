using UnityEngine;
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
                    
                    if (_home.HasFreeTaskGroup == false)
                        return;

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
