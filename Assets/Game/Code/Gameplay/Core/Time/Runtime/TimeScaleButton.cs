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
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _unselectColor;

        public event Action<TimeScaleButton> Clicked;
        public virtual event Action<TimeScaleButton> Deactivated;

        public float TargetSpeed { get; private set; }
        public bool Selected { get; private set; }
        public virtual bool Activated => true;

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
            OnInitialize();
        }

        public void Select()
        {
            if (Activated == false)
                throw new InvalidOperationException();
            
            ChangeColor(_selectColor);
            Selected = true;
        }

        public void Unselect()
        {
            ChangeColor(_unselectColor);
            Selected = false;
        }

        public virtual void ActivateFor(float durationInSeconds) { }
        protected virtual void OnInitialize() {}

        private void ChangeColor(Color color)
        {
            _button.image.color = color;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke(this);
        }
    }
}