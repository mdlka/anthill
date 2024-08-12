using System;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.Utils;

namespace YellowSquad.Anthill.Meta
{
    internal class LevelSwitchWarning : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Action _onNextButtonClick;
        
        public bool Rendered { get; private set; }

        private void Awake()
        {
            _canvasGroup.Disable();
            
            _nextButton.onClick.AddListener(OnNextButtonClick);
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnDestroy()
        {
            _nextButton.onClick.RemoveListener(OnNextButtonClick);
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        internal void Render(Action onNextButtonClick)
        {
            if (Rendered)
                throw new InvalidOperationException();

            Rendered = true;
            _onNextButtonClick = onNextButtonClick;
            _canvasGroup.Enable(duration: 0.2f);
        }

        private void OnNextButtonClick()
        {
            if (Rendered == false)
                return;
            
            _onNextButtonClick?.Invoke();
        }

        private void OnCloseButtonClick()
        {
            if (Rendered == false)
                return;
            
            _canvasGroup.Disable(duration: 0.2f);
            Rendered = false;
        }
    }
}