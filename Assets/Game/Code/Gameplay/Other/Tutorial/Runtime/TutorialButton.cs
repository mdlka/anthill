using System;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Tutorial
{
    internal class TutorialButton
    {
        private readonly Button _button;

        public TutorialButton(Button button)
        {
            _button = button;
        }

        public Transform Transform => _button.transform;

        public void Enable()
        {
            _button.enabled = true;
        }

        public void Disable()
        {
            _button.enabled = false;
        }
    }
}