using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agava.WebUtility;
using Newtonsoft.Json;
using TNRD;
using UnityEngine;
using YellowSquad.Anthill.Application.Adapters;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.CameraControl;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.Anthill.Core.GameTime;
using YellowSquad.Anthill.Levels;
using YellowSquad.Anthill.UserInput;
using YellowSquad.Anthill.Meta;
using YellowSquad.Anthill.Tutorial;
using YellowSquad.GamePlatformSdk;
using YellowSquad.HexMath;
using YellowSquad.Utils;

namespace YellowSquad.Anthill.Application
{
    public class Game : MonoBehaviour
    {
        private const int IntervalBetweenSaveInSeconds = 10;

        private readonly Stopwatch _stopwatch = new();
        
        [SerializeField] private bool _skipTutorial;
        [SerializeField] private LevelList _levelList;
        [SerializeField] private TutorialRoot _tutorialRoot;
        [SerializeField] private CanvasGroup _blackScreen;

        [Header("Core settings")]
        [SerializeField] private SerializableInterface<IHexMapView> _hexMapView;
        [SerializeField] private AntView _diggerView;
        [SerializeField] private AntView _loaderView;
        [SerializeField] private MovementSettings _movementSettings;
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
        [SerializeField] private LevelSwitchView _levelSwitchView;
        [SerializeField] private MoneyAnimation _moneyAnimation;
        [SerializeField, Min(0)] private int _takeLeafTaskPrice;
        [SerializeField] private UpgradeShopInfo _upgradeShopSettings;

        [Header("Ads")] 
        [SerializeField, Min(0)] private int _delayBetweenAdsInSeconds;
        [SerializeField] private AdsTimer _adsTimer;

#if UNITY_EDITOR
        [Header("Debug")] 
        [SerializeField] private bool _needDebugSdk;
        [SerializeField] private Language _debugLanguage;
#endif

        private IAnthill _anthill;
        private InputRoot _inputRoot;
        private LeafTasksLoop _leafTasksLoop;
        private MovementPath _movementPath;
        private MapCellShop _mapCellShop;
        private LevelSwitch _levelSwitch;
        private ITaskStorage _diggerTaskStorage;
        private Level _currentLevel;
        private CellsPriceList _mapCellPriceList;
        private ISave _save;
        
        private bool _gameInitialized;
        private float _saveElapsedTime;

        private void Awake()
        {
#if !UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = -1;
#endif
        }

        private IEnumerator Start()
        {
            _blackScreen.Enable();
            
#if UNITY_EDITOR
            if (_needDebugSdk)
                GamePlatformSdkContext.EnableLanguageDebug(_debugLanguage);
#endif

            if (GamePlatformSdkContext.Current.Initialized == false)
            {
                yield return GamePlatformSdkContext.Current.Initialize();
                yield return GamePlatformSdkContext.Current.Config.Load(new Dictionary<string, string>
                {
                    { ConfigConstants.DelayBetweenAdsInSeconds, _delayBetweenAdsInSeconds.ToString() }
                });
            }
            
            yield return GamePlatformSdkContext.Current.Advertisement.ShowInterstitial();

            _save = GamePlatformSdkContext.Current.Save;
            var config = GamePlatformSdkContext.Current.Config;
            
            if (_levelList.Initialized == false)
                _levelList.Initialize(_save);

            _currentLevel = _levelList.CurrentLevel();
            
            var map = _currentLevel.MapFactory.Create(_save);
            map.UpdateAllClosedPositions();
            map.Visualize(_hexMapView.Value);

            var loaderTaskStorage = new DefaultStorage(_save, "", needSave: false);
            _diggerTaskStorage = new DefaultStorage(_save, SaveConstants.DiggerTaskStorageSaveKey, needSave: true);
            
            _diggerTasksProgressView.Initialize(map);

            _diggerView.Initialize(map.Scale);
            _loaderView.Initialize(map.Scale);

            _movementPath = new MovementPath(map, new Path(new MapMovePolicy(map)), _movementSettings);
            
            var wallet = new DefaultWallet(_walletView.Value, _save, _currentLevel.StartWalletValue);
            wallet.Spend(0); // initialize view
            
            var mapGoal = new MapGoal(_currentLevel.GoalAnts, _mapGoalView);
            _levelSwitch = new LevelSwitch(_levelList, mapGoal, _levelSwitchView);

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
                _diggerView, _loaderView, _save);

            _anthill.Load();
            mapGoal.AddProgress(_anthill.Diggers.CurrentCount + _anthill.Loaders.CurrentCount);

            _mapCellPriceList = new CellsPriceList(map, _save, 10, 25);
            _mapCellPriceList.Load();

            _mapCellShop = new MapCellShop(wallet, _mapCellPriceList);
            _mapCellCellShopView.Initialize(map, _diggerTaskStorage);

            var collectHexTaskGroupFactory = new CollectHexTaskGroupFactory(map, _hexMapView.Value, _stopwatch, _delayBetweenTasks);
            
            if (_save.HasKey(SaveConstants.DiggerTaskStorageSaveKey))
            {
                var diggerTaskStorageSaveData = JsonConvert.DeserializeObject<TaskStorageSave>(
                    _save.GetString(SaveConstants.DiggerTaskStorageSaveKey));

                foreach (AxialCoordinate position in diggerTaskStorageSaveData.ActiveTasks)
                    if (map.HasPosition(position) && map.HexFrom(position).HasParts)
                        _diggerTaskStorage.AddTaskGroup(collectHexTaskGroupFactory.Create(position));
            }

            _inputRoot = new InputRoot(map, Device.IsMobile ? new TouchInput() : new MouseInput(), 
                new DefaultCamera(Camera.main, _currentLevel.CameraSettings), 
                new[]
                {
                    new FirstTrueCommand(
                        new AddDiggerTaskCommand(_diggerTaskStorage, collectHexTaskGroupFactory, _mapCellShop), 
                        new RemoveDiggerTaskCommand(_diggerTaskStorage, _mapCellShop)),
                    new RestoreLeafCommand(map, _hexMapView.Value, wallet, _mapCellPriceList, _moneyAnimation),
                    _tutorialRoot.CreateTutorialCommand()
                });

            _leafTasksLoop = new LeafTasksLoop(map, loaderTaskStorage, 
                new CollectPointOfInterestTaskGroupFactory(map, _hexMapView.Value, _stopwatch, _delayBetweenTasks, _takeLeafTaskPrice));

            var shopButtons = _upgradeShop.Initialize(new[]
            {
                new UpgradeButtonDTO
                {
                    ButtonName = _upgradeShopSettings.DiggerButton.Headers.SelectCurrentLanguageText(),
                    Icon = _upgradeShopSettings.DiggerButton.Icon,
                    Upgrade = new CallbackUpgrade(new DiggersCountUpgrade(_anthill, new AlgebraicProgressionPriceList(0, 10, _anthill.Diggers.CurrentCount), wallet), 
                        () => mapGoal.AddProgress()),
                },
                new UpgradeButtonDTO
                {
                    ButtonName = _upgradeShopSettings.LoaderButton.Headers.SelectCurrentLanguageText(),
                    Icon = _upgradeShopSettings.LoaderButton.Icon,
                    Upgrade = new CallbackUpgrade(new LoadersCountUpgrade(_anthill, new AlgebraicProgressionPriceList(0, 10, _anthill.Loaders.CurrentCount), wallet),
                        () => mapGoal.AddProgress()),
                },
            });
            
            _gameInitialized = true;
            _blackScreen.Disable(0.5f, onComplete: () => GamePlatformSdkContext.Current.Ready());

            if (_skipTutorial || _save.HasKey(SaveConstants.TutorialSaveKey) || _levelList.CurrentLevelIsTutorial == false)
            {
                _adsTimer.StartTimer(config.GetInt(ConfigConstants.DelayBetweenAdsInSeconds), _inputRoot);
                yield break;
            }
            
            yield return _tutorialRoot.StartTutorial(map, _anthill, _diggerTaskStorage, shopButtons[0], shopButtons[1]);
            _save.SetInt(SaveConstants.TutorialSaveKey, 1);
            
            _adsTimer.StartTimer(config.GetInt(ConfigConstants.DelayBetweenAdsInSeconds), _inputRoot);
        }

        private void Update()
        {
            if (GamePlatformSdkContext.Current.Initialized == false || _gameInitialized == false)
                return;
            
            _stopwatch.Update(Time.deltaTime * _timeScale.Value);
            _anthill.Update(Time.deltaTime * _timeScale.Value);
            _inputRoot.Update(Time.deltaTime);
            _leafTasksLoop.Update(Time.deltaTime);
            _levelSwitch.Update(Time.deltaTime);
            
            _mapCellShop.Visualize(_mapCellCellShopView);
            _diggerTaskStorage.Visualize(_diggerTasksProgressView);

            _saveElapsedTime += Time.deltaTime;

            if ((_save.HasKey(SaveConstants.TutorialSaveKey) == false && _skipTutorial == false) || _saveElapsedTime < IntervalBetweenSaveInSeconds)
                return;

            _saveElapsedTime = 0f;
            _save.Save();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            _movementPath?.OnDrawGizmos();
            _currentLevel?.CameraSettings?.OnDrawGizmos();
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
            [field: SerializeField] public Sprite Icon { get; private set; }
            [field: SerializeField] public LocalizedText[] Headers { get; private set; }
        }
    }
}