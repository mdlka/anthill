using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.Anthill.Core.GameTime;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class TaskGroup : ITaskGroup
    {
        private readonly HashSet<ITask> _tasks;
        private readonly HashSet<ITask> _tookTasks = new();
        private readonly float _delayBetweenTasks;
        private readonly int _tasksCount;

        private readonly IStopwatch _stopwatch;
        private readonly int _stopwatchIndex;

        private float _lastFindTaskTime;

        public TaskGroup(AxialCoordinate targetCellPosition, HashSet<ITask> tasks, IStopwatch stopwatch, float delayBetweenTasks)
        {
            TargetCellPosition = targetCellPosition;
            _tasks = tasks;
            _delayBetweenTasks = delayBetweenTasks;
            _tasksCount = _tasks.Count;

            _stopwatch = stopwatch;
            _stopwatchIndex = _stopwatch.Create();
        }
        
        public AxialCoordinate TargetCellPosition { get; }
        public bool AllTaskCompleted => _tookTasks.Count == _tasksCount && _tookTasks.All(task => task.Completed);
        public bool HasFreeTask => _tasks.Count > 0 && _stopwatch.ElapsedTime(_stopwatchIndex) >= _delayBetweenTasks;
        public float Progress => 1.0f * _tookTasks.Count(task => task.Completed) / _tasksCount;
        public bool Cancelled { get; private set; }

        public ITask TakeClosestTask(FracAxialCoordinate position)
        {
            if (Cancelled || HasFreeTask == false)
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
            _stopwatch.Restart(_stopwatchIndex);
            
            return task;
        }

        public void Cancel()
        {
            if (Cancelled)
                throw new InvalidOperationException();

            foreach (var task in _tasks.Where(task => task.Cancelled == false))
                task.Cancel();

            foreach (var task in _tookTasks.Where(task => task.Cancelled == false))
                task.Cancel();
            
            Cancelled = true;
        }
    }
}