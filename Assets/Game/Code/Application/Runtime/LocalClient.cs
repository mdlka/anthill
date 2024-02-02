using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TNRD;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YellowSquad.HexMath;
using YellowSquad.Anthill.Application.Adapters;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.Anthill.Meta;

namespace YellowSquad.Anthill.Application
{
    public class LocalClient : MonoBehaviour
    {
        private readonly List<FracAxialCoordinate> _test = new();

        [Header("Core settings")]
        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField] private SerializableInterface<IHexMapView> _hexMapView;
        [SerializeField] private AntView _diggerView;
        [SerializeField] private AntView _loaderView;
        [SerializeField] private MovementSettings _movementSettings;
        [SerializeField, Min(1)] private int _homesCapacity;
        [SerializeField, Min(0)] private float _homeDelayBetweenFindTasks;
        [Header("Meta settings")] 
        [SerializeField] private Shop _shop;
        [SerializeField] private SerializableInterface<IWalletView> _walletView;
        [SerializeField, Min(0)] private int _startWalletValue;
        [SerializeField, Min(0)] private int _takeLeafTaskPrice;
        [SerializeField, Min(0)] private int _restoreLeafReward;
        [Header("Mobile input")] 
        [SerializeField] private Button _spawnAntsButton;

        private IHexMap _map;
        private Camera _camera;
        private Session _session;
        private LeafTasksLoop _leafTasksLoop;
        private MovementPath _movementPath;
        private IWallet _wallet;

        private void Awake()
        {
            UnityEngine.Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            _spawnAntsButton.onClick.AddListener(SpawnAnts);
        }

        private void OnDestroy()
        {
            _spawnAntsButton.onClick.RemoveListener(SpawnAnts);
        }

        private void Start()
        {
            _map = _mapFactory.Create();
            _map.Visualize(_hexMapView.Value);

            var diggerTaskStorage = new DefaultStorage();
            var loaderTaskStorage = new DefaultStorage();

            _diggerView.Initialize(_map.Scale);
            _loaderView.Initialize(_map.Scale);
            
            _movementSettings.Initialize(_map.Scale);
            _movementPath = new MovementPath(_map, new Path(new MapMovePolicy(_map)), _movementSettings);
            
            _wallet = new Wallet(_walletView.Value, _startWalletValue);
            _wallet.Spend(0); // initialize view

            _session = new Session(
                new Queen(
                    _map.PointsOfInterestPositions(PointOfInterestType.Queen)[0],
                    new DefaultAntFactory(_movementPath, _movementSettings, new TaskStore(_wallet)),
                    new HomeList(_homesCapacity, _map, _map.PointsOfInterestPositions(PointOfInterestType.DiggersHome)
                        .Select(position => new AntHome(position, diggerTaskStorage, _homeDelayBetweenFindTasks))
                        .ToArray<IHome>()),
                    new HomeList(_homesCapacity, _map, _map.PointsOfInterestPositions(PointOfInterestType.LoadersHome)
                        .Select(position => new AntHome(position, loaderTaskStorage, _homeDelayBetweenFindTasks))
                        .ToArray<IHome>())),
                _diggerView,
                _loaderView, 
                _movementSettings);
            
            _shop.Initialize(_wallet, _session, _movementSettings.MaxMoveDuration);

            _leafTasksLoop = new LeafTasksLoop(_map, _hexMapView.Value, loaderTaskStorage, _takeLeafTaskPrice);
            _camera = Camera.main;

            StartCoroutine(InputLoop(diggerTaskStorage));
        }

        private IEnumerator InputLoop(ITaskStorage diggerTaskStorage)
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
                    if (IsPointerOverUIObject(mouseClickPosition))
                        continue;
                    
                    if (_map.HasPosition(targetAxialPosition) == false)
                        continue;

                    var targetHex = _map.HexFrom(targetAxialPosition);

                    if (Input.GetMouseButtonDown(1))
                    {
                        while (targetHex.HasParts)
                            targetHex.DestroyClosestPartFor(targetPosition);
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
                                
                                diggerTaskStorage.AddTaskGroup(new TaskGroup(targetAxialPosition, tasks));

                            }
                            else if (_map.HasDividedPointOfInterestIn(targetAxialPosition))
                            {
                                var targetDividedPointOfInterest = _map.DividedPointOfInterestFrom(targetAxialPosition);

                                if (targetDividedPointOfInterest.HasParts == false)
                                {
                                    if (targetDividedPointOfInterest.CanRestore)
                                    {
                                        targetDividedPointOfInterest.Restore();
                                        _wallet.Add(_restoreLeafReward);
                                    }
                                }
                            }
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.C))
                    SpawnAnts();

                _map.Visualize(_hexMapView.Value);
            }
        }

        private void Update()
        {
            _session.Update(Time.deltaTime);
            _leafTasksLoop.Update(Time.deltaTime);
        }
        
        private void SpawnAnts()
        {
            if (_session.CanAddDigger)
                _session.AddDigger();

            if (_session.CanAddLoader)
                _session.AddLoader();
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
        
        private bool IsPointerOverUIObject(Vector2 inputPosition)
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = inputPosition };
            var results = new List<RaycastResult>();
            
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            foreach (var result in results)
                if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;

            return false;
        }
    }
}
