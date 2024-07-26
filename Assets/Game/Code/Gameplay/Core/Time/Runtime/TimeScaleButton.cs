using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Core.GameTime
{
    internal class TimeScaleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Sprite _selectIcon;
        [SerializeField] private Sprite _unselectIcon;

        public event Action<TimeScaleButton> Clicked;

        public float TargetSpeed { get; private set; }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Initialize(float targetSpeed)
        {
            TargetSpeed = targetSpeed;
            _text.text = $"x{targetSpeed}";
            
            Unselect();
        }

        public void Select()
        {
            ChangeIcon(_selectIcon);
        }

        public void Unselect()
        {
            ChangeIcon(_unselectIcon);
        }

        private void ChangeIcon(Sprite icon)
        {
            _button.image.sprite = icon;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke(this);
        }
    }
}