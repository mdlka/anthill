using System;
using System.Collections.Generic;
using System.Linq;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultStorage : ITaskStorage
    {
        private readonly Queue<ITaskGroup> _taskGroups = new();

        public bool HasFreeTaskGroup => _taskGroups.Any(group => group.HasFreeTask);

        public void AddTaskGroup(ITaskGroup taskGroup)
        {
            if (_taskGroups.Contains(taskGroup))
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

                _taskGroups.Dequeue();
            }

            throw new InvalidOperationException();
        }
    }
}