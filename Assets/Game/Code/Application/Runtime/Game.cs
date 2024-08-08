using System;
using System.Linq;
using Agava.WebUtility;
using TNRD;
using UnityEngine;
using YellowSquad.Anthill.Application.Adapters;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.CameraControl;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.Anthill.Core.GameTime;
using YellowSquad.Anthill.UserInput;
using YellowSquad.Anthill.Meta;

namespace YellowSquad.Anthill.Application
{
    public class Game : MonoBehaviour
    {
        private readonly Stopwatch _stopwatch = new();
        
        [Header("Core settings")]
        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField] private SerializableInterface<IHexMapView> _hexMapView;
        [SerializeField] private AntView _diggerView;
        [SerializeField] private AntView _loaderView;
        [SerializeField] private MovementSettings _movementSettings;
        [SerializeField] private CameraSettings _cameraSettings;
        [SerializeField] private TimeScale _timeScale;
        [SerializeField] private TasksProgressView _diggerTasksProgressView;
        [SerializeField] private TaskStoreView _taskStoreView;
        [SerializeField, Min(1)] private int _homesCapacity;
        [SerializeField, Min(0)] private float _delayBetweenHomeFindTask;
        [SerializeField, Min(0)] private float _delayBetweenTasks;

        [Header("Meta settings")] 
        [SerializeField] private UpgradeShop _upgradeShop;
        [SerializeField] private SerializableInterface<IWalletView> _walletView;
        [SerializeField] private MapCellCellShopView _mapCellCellShopView;
        [SerializeField] private MapGoalView _mapGoalView;
        [SerializeField] private MoneyAnimation _moneyAnimation;
        [SerializeField, Min(0)] private int _mapTargetAnts;
        [SerializeField, Min(0)] private int _startWalletValue;
        [SerializeField, Min(0)] private int _takeLeafTaskPrice;
        [SerializeField] private UpgradeShopInfo _upgradeShopSettings;

        private IAnthill _anthill;
        private InputRoot _inputRoot;
        private LeafTasksLoop _leafTasksLoop;
        private MovementPath _movementPath;
        private MapCellShop _mapCellShop;
        private ITaskStorage _diggerTaskStorage;

        private void Awake()
        {
#if !UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = -1;//(int)Screen.currentResolution.refreshRateRatio.value;
#endif
        }

        private void Start()
        {
            var map = _mapFactory.Create();
            map.UpdateAllClosedPositions();
            map.Visualize(_hexMapView.Value);

            var loaderTaskStorage = new DefaultStorage();
            _diggerTaskStorage = new DefaultStorage();
            
            _diggerTasksProgressView.Initialize(map);

            _diggerView.Initialize(map.Scale);
            _loaderView.Initialize(map.Scale);

            _movementSettings.Initialize(map.Scale);
            _movementPath = new MovementPath(map, new Path(new MapMovePolicy(map)), _movementSettings);
            
            var wallet = new DefaultWallet(_walletView.Value, _startWalletValue);
            wallet.Spend(0); // initialize view

            _anthill = new DefaultAnthill(
                new Queen(
                    map.PointsOfInterestPositions(PointOfInterestType.Queen)[0],
                    new DefaultAntFactory(_movementPath, _movementSettings, new TaskStore(map.Scale, wallet, _taskStoreView)),
                    new HomeList(_homesCapacity, map, map.PointsOfInterestPositions(PointOfInterestType.DiggersHome)
                        .Select(position => new AntHome(position, _diggerTaskStorage, _stopwatch, _delayBetweenHomeFindTask))
                        .ToArray<IHome>()),
                    new HomeList(_homesCapacity, map, map.PointsOfInterestPositions(PointOfInterestType.LoadersHome)
                        .Select(position => new AntHome(position, loaderTaskStorage, _stopwatch, _delayBetweenHomeFindTask))
                        .ToArray<IHome>())),
                _diggerView, 
                _loaderView);

            var mapCellPriceList = new CellsPriceList(map, 10, 25);
            _mapCellShop = new MapCellShop(wallet, mapCellPriceList);
            _mapCellCellShopView.Initialize(map, _diggerTaskStorage);

            var mapGoal = new MapGoal(_mapTargetAnts, _mapGoalView);
            var collectHexTaskGroupFactory = new CollectHexTaskGroupFactory(map, _hexMapView.Value, _stopwatch, _delayBetweenTasks);

            _inputRoot = new InputRoot(map, Device.IsMobile ? new TouchInput() : new MouseInput(), 
                new DefaultCamera(Camera.main, _cameraSettings), 
                new IClickCommand[]
                {
                    new FirstTrueCommand(
                        new AddDiggerTaskCommand(_diggerTaskStorage, collectHexTaskGroupFactory, _mapCellShop), 
                        new RemoveDiggerTaskCommand(_diggerTaskStorage, _mapCellShop)),
                    new RestoreLeafCommand(map, _hexMapView.Value, wallet, mapCellPriceList, _moneyAnimation)
                });

            _leafTasksLoop = new LeafTasksLoop(map, loaderTaskStorage, 
                new CollectPointOfInterestTaskGroupFactory(map, _hexMapView.Value, _stopwatch, _delayBetweenTasks, _takeLeafTaskPrice));
            
            _upgradeShop.Initialize(new[]
            {
                new UpgradeButtonDTO
                {
                    ButtonName = _upgradeShopSettings.DiggerButton.HeaderEn,
                    Icon = _upgradeShopSettings.DiggerButton.Icon,
                    Upgrade = new CallbackUpgrade(new DiggersCountUpgrade(_anthill, new AlgebraicProgressionPriceList(0, 10), wallet), 
                        () => mapGoal.AddProgress()),
                },
                new UpgradeButtonDTO
                {
                    ButtonName = _upgradeShopSettings.LoaderButton.HeaderEn,
                    Icon = _upgradeShopSettings.LoaderButton.Icon,
                    Upgrade = new CallbackUpgrade(new LoadersCountUpgrade(_anthill, new AlgebraicProgressionPriceList(0, 10), wallet),
                        () => mapGoal.AddProgress()),
                },
            });
        }

        private void Update()
        {
            _stopwatch.Update(Time.deltaTime * _timeScale.Value);
            _anthill.Update(Time.deltaTime * _timeScale.Value);
            _inputRoot.Update(Time.deltaTime);
            _leafTasksLoop.Update(Time.deltaTime);
            
            _mapCellShop.Visualize(_mapCellCellShopView);
            _diggerTaskStorage.Visualize(_diggerTasksProgressView);
        }
        
        private void OnGUI()
        {
            foreach (var touch in Input.touches)
            {
                var touchPosition = touch.position;
                touchPosition.y = Screen.height - touchPosition.y;

                var rect = new Rect(
                    touchPosition.x - 50,
                    touchPosition.y - 50,
                    100,
                    100);

                GUI.Box(rect, $"{touch.fingerId}", new GUIStyle {fontSize = 64});
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _movementPath?.OnDrawGizmos();
            _cameraSettings?.OnDrawGizmos();
        }
#endif
    }

    [Serializable]
    internal class UpgradeShopInfo
    {
        [field: SerializeField] public ButtonInfo DiggerButton { get; private set; }
        [field: SerializeField] public ButtonInfo LoaderButton { get; private set; }

        [Serializable]
        internal class ButtonInfo
        {
            [field: SerializeField] public string HeaderRu { get; private set; }
            [field: SerializeField] public string HeaderEn { get; private set; }
            [field: SerializeField] public Sprite Icon { get; private set; }
        }
    }
}