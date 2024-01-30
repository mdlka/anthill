using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TNRD;
using UnityEngine;
using UnityEngine.UI;
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
        private readonly List<FracAxialCoordinate> _test = new();

        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField] private SerializableInterface<IHexMapView> _hexMapView;
        [SerializeField] private MovementSettings _movementSettings;
        [SerializeField] private AntView _diggerView;
        [SerializeField] private AntView _loaderView;
        [SerializeField, Min(1)] private int _homesCapacity;
        [SerializeField, Min(0)] private float _homeDelayBetweenFindTasks;

        [Header("Mobile input")] 
        [SerializeField] private Button _spawnAntsButton;

        private Camera _camera;
        private IHexMap _map;
        private ITaskStorage _diggerTaskStorage;
        private ITaskStorage _loaderTaskStorage;
        private Queen _queen;
        private MovementPath _movementPath;

        private void Awake()
        {
            UnityEngine.Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            _spawnAntsButton.onClick.AddListener(SpawnAnts);
        }

        private void OnDestroy()
        {
            _spawnAntsButton.onClick.RemoveListener(SpawnAnts);
        }

        private IEnumerator Start()
        {
            _map = _mapFactory.Create();
            _map.Visualize(_hexMapView.Value);

            _diggerTaskStorage = new DefaultStorage();
            _loaderTaskStorage = new DefaultStorage();
            
            _diggerView.Initialize(_map.Scale);
            //_loaderView.Initialize(_map.Scale);
            
            _movementSettings.Initialize(_map.Scale);
            _movementPath = new MovementPath(_map, new Path(new MapMovePolicy(_map)), _movementSettings);

            _queen = new Queen(
                _map.PointsOfInterestPositions(PointOfInterestType.Queen)[0],
                new DefaultAntFactory(_movementPath, _movementSettings),
                new HomeList(_homesCapacity, _map, _map.PointsOfInterestPositions(PointOfInterestType.DiggersHome)
                    .Select(position => new AntHome(position, _diggerTaskStorage, _homeDelayBetweenFindTasks)).ToArray<IHome>()),
                new HomeList(_homesCapacity, _map, _map.PointsOfInterestPositions(PointOfInterestType.LoadersHome)
                    .Select(position => new AntHome(position, _loaderTaskStorage, _homeDelayBetweenFindTasks)).ToArray<IHome>()));

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

                var mouseClickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y);
                var targetPosition = _camera.ScreenToWorldPoint(mouseClickPosition);
                var targetAxialPosition = targetPosition.ToAxialCoordinate(_map.Scale);

                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    if (_map.HasPosition(targetAxialPosition) == false)
                        continue;

                    var targetHex = _map.HexFrom(targetAxialPosition);

                    if (Input.GetMouseButtonDown(1))
                    {
                        while (targetHex.HasParts)
                            targetHex.DestroyClosestPartFor(targetPosition);

                        if (_map.HasDividedPointOfInterestIn(targetAxialPosition))
                        {
                            var dividedPointOfInterest = _map.DividedPointOfInterestFrom(targetAxialPosition);
                            
                            while (dividedPointOfInterest.HasParts)
                                dividedPointOfInterest.DestroyClosestPartFor(targetPosition);
                        }
                    }
                    else
                    {
                        if (_map.IsClosed(targetAxialPosition) == false)
                        {
                            var tasks = new HashSet<ITask>();
                            _test.Clear();

                            if (targetHex.HasParts)
                            {
                                var hexMatrix = _hexMapView.Value.HexMatrixBy(_map.Scale, targetAxialPosition);
                            
                                foreach (var part in targetHex.Parts)
                                {
                                    _test.Add(hexMatrix.MultiplyPoint(part.LocalPosition).ToFracAxialCoordinate(_map.Scale));

                                    tasks.Add(new TaskWithCallback(
                                        new TakePartTask(hexMatrix.MultiplyPoint(part.LocalPosition).ToFracAxialCoordinate(_map.Scale), targetHex, part), 
                                        onComplete: () => _map.Visualize(_hexMapView.Value)));
                                }
                                
                                _diggerTaskStorage.AddTaskGroup(new TaskGroup(targetAxialPosition, tasks));

                            }
                            else if (_map.HasDividedPointOfInterestIn(targetAxialPosition))
                            {
                                var targetDividedPointOfInterest = _map.DividedPointOfInterestFrom(targetAxialPosition);

                                if (targetDividedPointOfInterest.HasParts == false)
                                {
                                    if (targetDividedPointOfInterest.CanRestore)
                                        targetDividedPointOfInterest.Restore();
                                }
                                else
                                {
                                    var pointOfInterestMatrix = _hexMapView.Value.PointOfInterestMatrixBy(_map.Scale, 
                                        targetAxialPosition, _map.PointOfInterestTypeIn(targetAxialPosition));
                                    
                                    foreach (var part in targetDividedPointOfInterest.Parts)
                                    {
                                        _test.Add(pointOfInterestMatrix.MultiplyPoint(part.LocalPosition).ToFracAxialCoordinate(_map.Scale));

                                        tasks.Add(new TaskWithCallback(
                                            new TakePartTask(pointOfInterestMatrix.MultiplyPoint(part.LocalPosition).ToFracAxialCoordinate(_map.Scale), 
                                                targetDividedPointOfInterest, part), 
                                            onComplete: () => _map.Visualize(_hexMapView.Value)));
                                    }
                                    
                                    _loaderTaskStorage.AddTaskGroup(new TaskGroup(targetAxialPosition, tasks));
                                }
                            }
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.C))
                    SpawnAnts();

                if (Input.GetKeyDown(KeyCode.V))
                    if (_map.HasPosition(targetAxialPosition) && _map.HasObstacleIn(targetAxialPosition) == false)
                        _diggerTaskStorage.AddTaskGroup(new UniqueTaskGroup(new TaskGroup(targetAxialPosition, new MoveToCellTask(targetAxialPosition))));

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
        
        private void SpawnAnts()
        {
            while (_queen.CanCreateDigger)
            {
                var ant = _queen.CreateDigger();
                _ants.Add(ant);
                _diggerView.Add(ant);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _movementPath?.OnDrawGizmos();

            if (_test == null) 
                return;
            
            foreach (var position in _test)
                Gizmos.DrawSphere(position.ToVector3(_map.Scale), 0.2f);
        }
#endif
    }
}
