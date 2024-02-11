using System.Collections.Generic;
using System.Linq;
using TNRD;
using UnityEngine;
using UnityEngine.EventSystems;
using YellowSquad.HexMath;
using YellowSquad.Anthill.Application.Adapters;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.Anthill.Meta;
using YellowSquad.Anthill.Session;

namespace YellowSquad.Anthill.Application
{
    public class Game : MonoBehaviour
    {
        [Header("Core settings")]
        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField] private SerializableInterface<IHexMapView> _hexMapView;
        [SerializeField] private SerializableInterface<ISessionView> _sessionView;
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
        [SerializeField, Min(0)] private float _minUpgradeAntsMoveDuration;

        private IHexMap _map;
        private Camera _camera;
        private ISession _session;
        private LeafTasksLoop _leafTasksLoop;
        private MovementPath _movementPath;
        private ITaskStorage _diggerTaskStorage;
        private IWallet _wallet;

        private ITaskGroupFactory _collectHexTaskGroupFactory;

        private void Awake()
        {
#if !UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif
        }

        private void Start()
        {
            _map = _mapFactory.Create();
            _map.Visualize(_hexMapView.Value);

            var loaderTaskStorage = new DefaultStorage();
            _diggerTaskStorage = new DefaultStorage();

            _diggerView.Initialize(_map.Scale);
            _loaderView.Initialize(_map.Scale);

            _collectHexTaskGroupFactory = new CollectHexTaskGroupFactory(_map, _hexMapView.Value);
            
            _movementSettings.Initialize(_map.Scale);
            _movementPath = new MovementPath(_map, new Path(new MapMovePolicy(_map)), _movementSettings);
            
            _wallet = new Wallet(_walletView.Value, _startWalletValue);
            _wallet.Spend(0); // initialize view

            _session = new Session.Session(
                new Queen(
                    _map.PointsOfInterestPositions(PointOfInterestType.Queen)[0],
                    new DefaultAntFactory(_movementPath, _movementSettings, new TaskStore(_wallet)),
                    new HomeList(_homesCapacity, _map, _map.PointsOfInterestPositions(PointOfInterestType.DiggersHome)
                        .Select(position => new AntHome(position, _diggerTaskStorage, _homeDelayBetweenFindTasks))
                        .ToArray<IHome>()),
                    new HomeList(_homesCapacity, _map, _map.PointsOfInterestPositions(PointOfInterestType.LoadersHome)
                        .Select(position => new AntHome(position, loaderTaskStorage, _homeDelayBetweenFindTasks))
                        .ToArray<IHome>())),
                _movementSettings,
                _diggerView, 
                _loaderView);
            
            _session.Visualize(_sessionView.Value);
            _shop.Initialize(_wallet, new ShopButtonDTO[]
            {
                new ShopButtonDTO()
                {
                    ButtonName = "Add digger",
                    ButtonCommand = new UpdateSessionViewCommand(new AddDiggerCommand(_session), _session, _sessionView.Value),
                    PriceList = new AlgebraicProgressionPriceList(0, 1)
                },
                new ShopButtonDTO()
                {
                    ButtonName = "Add loader",
                    ButtonCommand = new UpdateSessionViewCommand(new AddLoaderCommand(_session), _session, _sessionView.Value),
                    PriceList = new AlgebraicProgressionPriceList(0, 1)
                },
                new ShopButtonDTO()
                {
                    ButtonName = "Increase speed",
                    ButtonCommand = new UpdateSessionViewCommand(new IncreaseSpeedCommand(_session, 
                        new UpgradeAntMoveDurationList(20, _minUpgradeAntsMoveDuration, _session.MaxAntMoveDuration)),
                        _session, _sessionView.Value),
                    PriceList = new AlgebraicProgressionPriceList(0, 1)
                }
            });

            _leafTasksLoop = new LeafTasksLoop(_map, loaderTaskStorage, new CollectPointOfInterestTaskGroupFactory(_map, _hexMapView.Value, _takeLeafTaskPrice));
            _camera = Camera.main;
        }

        private void Update()
        {
            InputLoop();
            
            _session.Update(Time.deltaTime);
            _leafTasksLoop.Update(Time.deltaTime);
        }

        private void InputLoop()
        {
            if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) 
                return;
            
            var mouseClickPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y);
            var targetPosition = _camera.ScreenToWorldPoint(mouseClickPosition);
            var targetAxialPosition = targetPosition.ToAxialCoordinate(_map.Scale);

            if (IsPointerOverUIObject(mouseClickPosition))
                return;

            if (_map.HasPosition(targetAxialPosition) == false)
                return;

            var targetHex = _map.HexFrom(targetAxialPosition);

            if (Input.GetMouseButtonDown(1))
            {
                if (_diggerTaskStorage.HasTaskGroupIn(targetAxialPosition) == false)
                    while (targetHex.HasParts)
                        targetHex.DestroyClosestPartFor(targetPosition);
                    
                _map.Visualize(_hexMapView.Value);
                _session.Visualize(_sessionView.Value);
            }
            else
            {
                if (_map.IsClosed(targetAxialPosition)) 
                    return;
                    
                if (targetHex.HasParts)
                {
                    if (_diggerTaskStorage.HasTaskGroupIn(targetAxialPosition)) 
                        return;

                    if (_collectHexTaskGroupFactory.CanCreate(targetAxialPosition) == false)
                        return;

                    _diggerTaskStorage.AddTaskGroup(_collectHexTaskGroupFactory.Create(targetAxialPosition,
                        onComplete: () => _session.Visualize(_sessionView.Value)));
                }
                else if (_map.HasDividedPointOfInterestIn(targetAxialPosition))
                {
                    var targetDividedPointOfInterest = _map.DividedPointOfInterestFrom(targetAxialPosition);

                    if (targetDividedPointOfInterest.HasParts) 
                        return;
                        
                    if (targetDividedPointOfInterest.CanRestore == false) 
                        return;
                            
                    targetDividedPointOfInterest.Restore();
                    _wallet.Add(_restoreLeafReward);
                    _map.Visualize(_hexMapView.Value);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _movementPath?.OnDrawGizmos();
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
