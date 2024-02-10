using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class TaskGroup : ITaskGroup
    {
        private readonly HashSet<ITask> _tasks;
        private readonly HashSet<ITask> _tookTasks = new();
        private readonly int _tasksCount;

        public TaskGroup(AxialCoordinate targetCellPosition, params ITask[] tasks) 
            : this(targetCellPosition, new HashSet<ITask>(tasks)) { }

        public TaskGroup(AxialCoordinate targetCellPosition, HashSet<ITask> tasks)
        {
            TargetCellPosition = targetCellPosition;
            _tasks = tasks;
            _tasksCount = _tasks.Count;
        }
        
        public AxialCoordinate TargetCellPosition { get; }
        public bool AllTaskCompleted => _tookTasks.Count == _tasksCount && _tookTasks.All(task => task.State == TaskState.Complete);
        public bool HasFreeTask => _tasks.Count > 0;
        
        public ITask ClosestTask(FracAxialCoordinate position)
        {
            if (HasFreeTask == false)
                throw new InvalidOperationException();

            var task = _tasks.Aggregate((task1, task2) =>
                HMath.Distance(position, task1.TargetPosition) >
                HMath.Distance(position, task2.TargetPosition)
                    ? task2
                    : task1);

            int count = _tasks.Count;
            
            _tasks.Remove(task);
            
            if (count == _tasks.Count)
                throw new InvalidOperationException();
            
            _tookTasks.Add(task);
            
            return task;
        }
    }
}