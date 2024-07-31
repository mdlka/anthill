﻿using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class DefaultStorage : ITaskStorage
    {
        private readonly HashSet<ITaskGroup> _taskGroups = new();
        private readonly HashSet<AxialCoordinate> _activeTaskGroupsPositions = new();

        public bool HasFreeTaskGroup => _taskGroups.Any(group => group.HasFreeTask);

        public bool HasTaskGroupIn(AxialCoordinate position)
        {
            return _taskGroups.Any(taskGroup => taskGroup.AllTaskCompleted == false && taskGroup.TargetCellPosition == position);
        }

        public void AddTaskGroup(ITaskGroup taskGroup)
        {
            if (HasTaskGroupIn(taskGroup.TargetCellPosition))
                throw new InvalidOperationException();
            
            _taskGroups.Add(taskGroup);
            _activeTaskGroupsPositions.Add(taskGroup.TargetCellPosition);
        }

        public void CancelTaskGroup(AxialCoordinate position)
        {
            if (HasTaskGroupIn(position) == false)
                throw new InvalidOperationException();
            
            RemoveCompletedTasks();

            var targetTaskGroup = _taskGroups.First(taskGroup => taskGroup.TargetCellPosition == position);
            targetTaskGroup.Cancel();
            
            _taskGroups.Remove(targetTaskGroup);
            _activeTaskGroupsPositions.Remove(position);
        }

        public ITaskGroup FindTaskGroup()
        {
            if (HasFreeTaskGroup == false)
                throw new InvalidOperationException();

            RemoveCompletedTasks();
            return _taskGroups.First(taskGroup => taskGroup.HasFreeTask);
        }
        
        public void Visualize(ITasksProgressView view)
        {
            RemoveCompletedTasks();
            view.Render(_activeTaskGroupsPositions);
        }

        private void RemoveCompletedTasks()
        {
            int tasksBeforeRemove = _taskGroups.Count;
            _taskGroups.RemoveWhere(taskGroup => taskGroup.AllTaskCompleted);

            if (tasksBeforeRemove == _taskGroups.Count) 
                return;
            
            _activeTaskGroupsPositions.Clear();

            foreach (var taskGroup in _taskGroups)
                _activeTaskGroupsPositions.Add(taskGroup.TargetCellPosition);
        }
    }
}