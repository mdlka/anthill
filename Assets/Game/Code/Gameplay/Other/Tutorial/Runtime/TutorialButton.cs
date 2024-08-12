using System;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Tutorial
{
    internal class TutorialButton : IDisposable
    {
        private readonly Button _button;

        public TutorialButton(Button button)
        {
            _button = button;
            _button.onClick.AddListener(OnButtonClick);
        }

        public Transform Transform => _button.transform;
        public int ClickCount { get; private set; }

        public void Enable()
        {
            _button.enabled = true;
        }

        public void Disable()
        {
            _button.enabled = false;
        }
        
        public void Dispose()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            ClickCount += 1;
        }
    }
}