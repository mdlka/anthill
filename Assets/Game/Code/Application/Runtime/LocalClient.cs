using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TNRD;
using UnityEngine;
using YellowSquad.HexMath;
using YellowSquad.Anthill.Application.Adapters;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.HexMap;

namespace YellowSquad.Anthill.Application
{
    public class LocalClient : MonoBehaviour
    {
        private readonly List<IAnt> _ants = new();

        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField] private SerializableInterface<IHexMapView> _hexMapView;
        [SerializeField] private AntView _diggerView;
        [SerializeField] private AntView _loaderView;

        private Camera _camera;
        private IHexMap _map;

        private IEnumerator Start()
        {
            _map = _mapFactory.Create();
            _map.Visualize(_hexMapView.Value);

            var path = new Path(new MapMovePolicy(_map));
            var taskStorage = new DefaultStorage();
            
            _diggerView.Initialize(_map.Scale);
            
            var queen = new Queen(
                _map.PointsOfInterestPositions(PointOfInterest.Queen)[0],
                new DefaultAntFactory(path, 0.2f),
                new HomeList(20, _map, _map.PointsOfInterestPositions(PointOfInterest.DiggersHome)
                    .Select(position => new AntHome(position, taskStorage)).ToArray<IHome>()),
                new HomeList(20, _map, _map.PointsOfInterestPositions(PointOfInterest.LoadersHome)
                    .Select(position => new AntHome(position, taskStorage)).ToArray<IHome>()));

            _camera = Camera.main;
            
            while (true)
            {
                yield return null;
                yield return new WaitUntil(() => Input.anyKey);

                Vector3 mouseClickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y);
                var targetPosition = _camera.ScreenToWorldPoint(mouseClickPosition);
                var targetAxialPosition = targetPosition.ToAxialCoordinate(_map.Scale);

                if (Input.GetMouseButtonDown(0))
                {
                    if (_map.HasPosition(targetAxialPosition) == false)
                        continue;
                
                    var targetHex = _map.HexFrom(targetAxialPosition);
                    
                    while (targetHex.HasParts)
                        targetHex.RemoveClosestPartFor(targetPosition);
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (queen.CanCreateDigger)
                    {
                        var ant = queen.CreateDigger();
                        _ants.Add(ant);
                        _diggerView.Add(ant);
                    }
                }

                if (Input.GetMouseButtonDown(1))
                    if (_map.HasObstacleIn(targetAxialPosition) == false)
                        taskStorage.AddTask(new DefaultTask(targetAxialPosition));

                if (Input.GetKeyDown(KeyCode.Space))
                    Debug.Log(_map.ToString());

                _map.Visualize(_hexMapView.Value);
            }
        }

        private void Update()
        {
            _diggerView.UpdateRender();
            
            foreach (var ant in _ants)
                ant.Update(Time.deltaTime);
        }
    }
}
