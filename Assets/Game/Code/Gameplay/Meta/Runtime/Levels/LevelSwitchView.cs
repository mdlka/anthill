using System;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Meta
{
    public class LevelSwitchView : MonoBehaviour, ILevelSwitchView
    {
        [SerializeField] private Button _nextButton;

        private Action _onNextLevel;
        
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

        public void Render(Action onNextLevel)
        {
            if (Rendered)
                throw new InvalidOperationException();
            
            Rendered = true;

            _onNextLevel = onNextLevel;
            _nextButton.gameObject.SetActive(true);
        }
        
        private void OnNextButtonClick()
        {
            _onNextLevel?.Invoke();
        }
    }
}