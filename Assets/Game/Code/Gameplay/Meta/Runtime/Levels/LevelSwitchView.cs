using System;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Meta
{
    public class LevelSwitchView : MonoBehaviour, ILevelSwitchView
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private LevelSwitchWarning _warning;

        private Action _onNextLevelButtonClick;
        
        public bool Rendered { get; private set; }

        private void Awake()
        {
            _nextButton.onClick.AddListener(OnNextButtonClick);
            _nextButton.gameObject.SetActive(false);
        }
        
        private void OnDestroy()
        {
            _nextButton.onClick.RemoveListener(OnNextButtonClick);
        }

        public void Render(Action onNextLevelButtonClick)
        {
            if (Rendered)
                throw new InvalidOperationException();
            
            Rendered = true;
            _onNextLevelButtonClick = onNextLevelButtonClick;
            _nextButton.gameObject.SetActive(true);
        }
        
        private void OnNextButtonClick()
        {
            if (_warning.Rendered == false)
                _warning.Render(_onNextLevelButtonClick);
        }
    }
}