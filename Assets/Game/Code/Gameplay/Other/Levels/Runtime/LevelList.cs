using System;
using UnityEngine;
using YellowSquad.Anthill.Core.CameraControl;
using YellowSquad.Anthill.Core.HexMap;

namespace YellowSquad.Anthill.Levels
{
    [CreateAssetMenu(menuName = "Anthill/Levels/Create LevelList", fileName = "LevelList", order = 56)]
    public class LevelList : ScriptableObject
    {
        [SerializeField] private int _editorLevelIndex = -1;
        [SerializeField] private Level[] _levels;

        [NonSerialized] private int _currentLevelIndex;

        public bool CurrentLevelIsTutorial => _currentLevelIndex == 0;

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