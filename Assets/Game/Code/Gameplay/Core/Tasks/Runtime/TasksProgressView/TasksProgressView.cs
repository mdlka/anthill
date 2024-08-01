using System.Collections.Generic;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class TasksProgressView : MonoBehaviour, ITasksProgressView
    {
        private readonly Dictionary<AxialCoordinate, TaskProgressView> _views = new();

        [SerializeField] private TaskProgressView _taskProgressTemplate;
        [SerializeField] private Transform _viewsContent;
        [SerializeField] private Vector3 _viewsOffset;

        private IHexMap _map;
        
        public void Initialize(IHexMap map)
        {
            _map = map;
        }

        public void Render(Dictionary<AxialCoordinate, ITaskGroup> activeTaskGroups)
        {
            DestroyExtraViews(activeTaskGroups);
            
            foreach (var taskGroup in activeTaskGroups)
            {
                if (_views.ContainsKey(taskGroup.Key))
                {
                    _views[taskGroup.Key].Render(taskGroup.Value.Progress);
                    continue;
                }

                var viewInstance = Instantiate(_taskProgressTemplate, taskGroup.Key.ToVector3(_map.Scale) + _viewsOffset,
                    _taskProgressTemplate.transform.rotation, _viewsContent);
                viewInstance.Render(taskGroup.Value.Progress);
                
                _views.Add(taskGroup.Key, viewInstance);
            }
        }

        private void DestroyExtraViews(Dictionary<AxialCoordinate, ITaskGroup> activeTaskGroups)
        {
            int extraPositionsCount = _views.Count - activeTaskGroups.Count;

            if (extraPositionsCount <= 0)
                return;
            
            AxialCoordinate[] extraPositions = new AxialCoordinate[extraPositionsCount];
            int index = 0;
            
            foreach (var pair in _views)
                if (activeTaskGroups.ContainsKey(pair.Key) == false)
                    extraPositions[index++] = pair.Key;

            foreach (var position in extraPositions)
            {
                Destroy(_views[position].gameObject);
                _views.Remove(position);
            }
        }
    }
}
