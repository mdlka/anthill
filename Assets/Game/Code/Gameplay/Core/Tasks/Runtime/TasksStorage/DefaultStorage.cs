using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class DefaultStorage : ITaskStorage
    {
        private readonly Queue<ITaskGroup> _taskGroups = new();
        private readonly HashSet<ITaskGroup> _almostCompletedTaskGroups = new();

        public bool HasFreeTaskGroup => _taskGroups.Any(group => group.HasFreeTask);

        public bool HasTaskGroupIn(AxialCoordinate position)
        {
            if (_taskGroups.Any(taskGroup => taskGroup.HasFreeTask && taskGroup.TargetCellPosition == position))
                return true;

            if (_almostCompletedTaskGroups.Count == 0)
                return false;
            
            _almostCompletedTaskGroups.RemoveWhere(group => group.AllTaskCompleted);
            return _almostCompletedTaskGroups.Any(taskGroup => taskGroup.TargetCellPosition == position);
        }

        public void AddTaskGroup(ITaskGroup taskGroup)
        {
            if (HasTaskGroupIn(taskGroup.TargetCellPosition))
                throw new InvalidOperationException();
            
            _taskGroups.Enqueue(taskGroup);
        }

        public ITaskGroup FindTaskGroup()
        {
            if (HasFreeTaskGroup == false)
                throw new InvalidOperationException();

            while (_taskGroups.Count > 0)
            {
                if (_taskGroups.Peek().HasFreeTask)
                    return _taskGroups.Peek();
                
                _almostCompletedTaskGroups.Add(_taskGroups.Dequeue());
            }

            throw new InvalidOperationException();
        }
    }
}