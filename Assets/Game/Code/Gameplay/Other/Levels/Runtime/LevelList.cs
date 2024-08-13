using System;
using UnityEngine;
using YellowSquad.Anthill.Core.CameraControl;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.Anthill.Levels
{
    [CreateAssetMenu(menuName = "Anthill/Levels/Create LevelList", fileName = "LevelList", order = 56)]
    public class LevelList : ScriptableObject
    {
        [SerializeField] private int _editorLevelIndex = -1;
        [SerializeField] private Level[] _levels;

        [NonSerialized] private int _currentLevelIndex;
        [NonSerialized] private ISave _save;

        public bool CurrentLevelIsTutorial => _currentLevelIndex == 0;
        public bool Initialized => _save != null;

        public void Initialize(ISave save)
        {
            if (Initialized)
                throw new InvalidOperationException();
            
            _save = save;
            _currentLevelIndex = _save.GetInt(SaveConstants.CurrentLevelSaveKey, _currentLevelIndex);
        }

        public Level CurrentLevel()
        {
#if UNITY_EDITOR
            if (_editorLevelIndex >= 0)
                return _levels[_editorLevelIndex];
#endif
            
            return _levels[_currentLevelIndex];
        }

        internal void NextLevel()
        {
            _currentLevelIndex = Math.Min(_currentLevelIndex + 1, _levels.Length - 1);
            _save.SetInt(SaveConstants.CurrentLevelSaveKey, _currentLevelIndex);
            
            if (_save.HasKey(SaveConstants.WalletSaveKey))
                _save.DeleteKey(SaveConstants.WalletSaveKey);
            
            if (_save.HasKey(SaveConstants.AnthillSaveKey))
                _save.DeleteKey(SaveConstants.AnthillSaveKey);
            
            _save.Save();
        }
    }
    
    [Serializable]
    public class Level
    {
        [Header("Core")] 
        [SerializeField] private BaseMapFactory _mapFactory;
        [SerializeField] private CameraSettings _cameraSettings;
        [Header("Meta")] 
        [SerializeField] private int _startWalletValue;
        [SerializeField] private int _goalAnts;

        public BaseMapFactory MapFactory => _mapFactory;
        public CameraSettings CameraSettings => _cameraSettings;
        public int StartWalletValue => _startWalletValue;
        public int GoalAnts => _goalAnts;
    }
}