using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.GameTime
{
    public class TimeScaleSwitch : MonoBehaviour
    {
        private readonly List<TimeScaleButton> _buttons = new();
        
        [SerializeField] private TimeScale _timeScale;
        [SerializeField] private TimeScaleButton _buttonTemplate;
        [SerializeField] private Transform _content;
        [SerializeField] private List<float> _speeds;

        private void Awake()
        {
            foreach (float speed in _speeds)
            {
                var buttonInstance = Instantiate(_buttonTemplate, _content);
                buttonInstance.Initialize(speed);
                
                buttonInstance.Clicked += Select;
                _buttons.Add(buttonInstance);
            }
            
            if (_buttons.Count > 0)
                Select(_buttons[0]);
        }

        private void OnDestroy()
        {
            foreach (var button in _buttons)
                button.Clicked -= Select;
        }

        private void Select(TimeScaleButton button)
        {
            foreach (var localButton in _buttons)
                localButton.Unselect();
            
            button.Select();
            _timeScale.ChangeValue(button.TargetSpeed);
        }
    }
}