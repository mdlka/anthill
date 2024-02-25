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

        public void Render(HashSet<AxialCoordinate> activeTaskGroupsPositions)
        {
            DestroyExtraViews(activeTaskGroupsPositions);
            
            foreach (var taskGroupPosition in activeTaskGroupsPositions)
            {
                if (_views.ContainsKey(taskGroupPosition))
                    continue;
                
                _views.Add(taskGroupPosition, 
                    Instantiate(_taskProgressTemplate, taskGroupPosition.ToVector3(_map.Scale) + _viewsOffset, 
                        _taskProgressTemplate.transform.rotation, _viewsContent));
            }
        }

        private void DestroyExtraViews(HashSet<AxialCoordinate> activeTaskGroupsPositions)
        {
            int extraPositionsCount = _views.Count - activeTaskGroupsPositions.Count;

            if (extraPositionsCount <= 0)
                return;
            
            AxialCoordinate[] extraPositions = new AxialCoordinate[extraPositionsCount];
            int index = 0;
            
            foreach (var pair in _views)
                if (activeTaskGroupsPositions.Contains(pair.Key) == false)
                    extraPositions[index++] = pair.Key;

            foreach (var position in extraPositions)
            {
                Destroy(_views[position].gameObject);
                _views.Remove(position);
            }
        }
    }
}
