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
        [Header("Ants movement")]
        [SerializeField] private float _moveToGoalDuration = 0.3f;
        [SerializeField] private int _stepsToGoal = 5;

        private Camera _camera;
        private IHexMap _map;
        private ITaskStorage _taskStorage;
        private Queen _queen;

        private IEnumerator Start()
        {
            _map = _mapFactory.Create();
            _map.Visualize(_hexMapView.Value);

            _taskStorage = new DefaultStorage();
            _diggerView.Initialize(_map.Scale);
            
            _queen = new Queen(
                _map.PointsOfInterestPositions(PointOfInterest.Queen)[0],
                new DefaultAntFactory(new Path(new MapMovePolicy(_map)), _moveToGoalDuration, _stepsToGoal),
                new HomeList(20, _map, _map.PointsOfInterestPositions(PointOfInterest.DiggersHome)
                    .Select(position => new AntHome(position, _taskStorage)).ToArray<IHome>()),
                new HomeList(20, _map, _map.PointsOfInterestPositions(PointOfInterest.LoadersHome)
                    .Select(position => new AntHome(position, _taskStorage)).ToArray<IHome>()));

            _camera = Camera.main;

            StartCoroutine(AntLoop());
            StartCoroutine(InputLoop());

            yield return null;
        }

        private IEnumerator InputLoop()
        {
            while (true)
            {
                yield return null;
                yield return new WaitUntil(() => Input.anyKey);

                Vector3 mouseClickPosition =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y);
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
                    if (_queen.CanCreateDigger)
                    {
                        var ant = _queen.CreateDigger();
                        _ants.Add(ant);
                        _diggerView.Add(ant);
                    }
                }

                if (Input.GetMouseButtonDown(1))
                    if (_map.HasPosition(targetAxialPosition) && _map.HasObstacleIn(targetAxialPosition) == false)
                        _taskStorage.AddTask(new DefaultTask(targetAxialPosition));

                if (Input.GetKeyDown(KeyCode.Space))
                    Debug.Log(_map.ToString());

                _map.Visualize(_hexMapView.Value);
            }
        }

        private IEnumerator AntLoop()
        {
            while (true)
            {
                _diggerView.UpdateRender();
            
                foreach (var ant in _ants)
                    ant.Update(Time.deltaTime);
                
                yield return null;
            }
        }
    }
}
