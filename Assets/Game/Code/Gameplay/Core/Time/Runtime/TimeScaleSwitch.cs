using System.Collections.Generic;
using UnityEngine;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.Anthill.Core.GameTime
{
    public class TimeScaleSwitch : MonoBehaviour
    {
        private readonly List<TimeScaleButton> _buttons = new();

        [SerializeField] private float _rewardedSpeedDurationInSeconds;
        [SerializeField] private TimeScale _timeScale;
        [SerializeField] private TimeScaleButton _buttonTemplate;
        [SerializeField] private RewardedTimeScaleButton _rewardedButtonTemplate;
        [SerializeField] private Transform _content;
        [SerializeField] private List<float> _speeds;
        [SerializeField] private float _rewardedSpeed;
        
        private RewardedTimeScaleButton _rewardedButtonInstance;

        private void Awake()
        {
            foreach (float speed in _speeds)
            {
                var buttonInstance = Instantiate(_buttonTemplate, _content);
                buttonInstance.Initialize(speed);
                
                buttonInstance.Clicked += Select;
                _buttons.Add(buttonInstance);
            }

            _rewardedButtonInstance = Instantiate(_rewardedButtonTemplate, _content);
            _rewardedButtonInstance.Initialize(_rewardedSpeed);
            _rewardedButtonInstance.Clicked += Select;
            _rewardedButtonInstance.Deactivated += Deactivate;
            
            if (_buttons.Count > 0)
                Select(_buttons[0]);
        }

        private void OnDestroy()
        {
            foreach (var button in _buttons)
                button.Clicked -= Select;
            
            _rewardedButtonInstance.Clicked -= Select;
            _rewardedButtonInstance.Deactivated -= Deactivate;
        }

        private void Select(TimeScaleButton button)
        {
            if (button.Activated == false)
            {
                GamePlatformSdkContext.Current.Advertisement.ShowRewarded(onEnd: result =>
                {
                    if (result == Result.Failure)
                        return;
                    
                    button.ActivateFor(_rewardedSpeedDurationInSeconds);
                    Select(button);
                });

                return;
            }
            
            foreach (var localButton in _buttons)
                localButton.Unselect();
            
            _rewardedButtonInstance.Unselect();
            
            button.Select();
            _timeScale.ChangeValue(button.TargetSpeed);
        }
        
        private void Deactivate(TimeScaleButton button)
        {
            if (button.Selected == false)
                return;
            
            Select(_buttons[^1]);
        }
    }
}