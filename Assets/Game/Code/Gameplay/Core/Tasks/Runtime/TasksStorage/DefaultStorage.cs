using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using YellowSquad.GamePlatformSdk;
using YellowSquad.HexMath;
using YellowSquad.Utils;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class DefaultStorage : ITaskStorage
    {
        private readonly ISave _save;
        private readonly string _saveKey;
        private readonly bool _needSave;
        
        private readonly OrderedSet<ITaskGroup> _taskGroups = new();
        private readonly Dictionary<AxialCoordinate, ITaskGroup> _activeTaskGroups = new();

        public DefaultStorage(ISave save, string saveKey, bool needSave)
        {
            _save = save;
            _saveKey = saveKey;
            _needSave = needSave;
        }

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
            
            if (_activeTaskGroups.ContainsKey(taskGroup.TargetCellPosition) == false)
                _activeTaskGroups.Add(taskGroup.TargetCellPosition, taskGroup);

            Save();
        }

        public void CancelTaskGroup(AxialCoordinate position)
        {
            if (HasTaskGroupIn(position) == false)
                throw new InvalidOperationException();
            
            RemoveCompletedTasks();

            var targetTaskGroup = _taskGroups.First(taskGroup => taskGroup.TargetCellPosition == position);
            targetTaskGroup.Cancel();
            
            _taskGroups.Remove(targetTaskGroup);
            _activeTaskGroups.Remove(position);

            Save();
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
            view.Render(_activeTaskGroups);
        }

        private void RemoveCompletedTasks()
        {
            int tasksBeforeRemove = _taskGroups.Count;
            _taskGroups.RemoveWhere(taskGroup => taskGroup.AllTaskCompleted);

            if (tasksBeforeRemove == _taskGroups.Count) 
                return;
            
            _activeTaskGroups.Clear();

            foreach (var taskGroup in _taskGroups)
                _activeTaskGroups.Add(taskGroup.TargetCellPosition, taskGroup);
        }
        
        private void Save()
        {
            if (_needSave == false)
                return;
            
            var saveData = new TaskStorageSave();

            foreach (var pair in _activeTaskGroups)
                if (pair.Value.AllTaskCompleted == false)
                    saveData.ActiveTasks.Add(pair.Key);
            
            _save.SetString(_saveKey, JsonConvert.SerializeObject(saveData));
        }
    }

    [Serializable]
    public class TaskStorageSave
    {
        [JsonProperty] public HashSet<AxialCoordinateSave> ActiveTasks = new();
    }
}