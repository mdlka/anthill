using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Core.GameTime
{
    internal class RewardedTimeScaleButton : TimeScaleButton
    {
        [SerializeField] private Image _outline;
        [SerializeField] private Image _adsIcon;

        private bool _activated;
        private float _elapsedTime;

        public override event Action<TimeScaleButton> Deactivated;
        public override bool Activated => _activated;
        
        protected override void OnInitialize()
        {
            _outline.fillAmount = 0f;
            _adsIcon.enabled = true;
        }

        public override void ActivateFor(float durationInSeconds)
        {
            if (_activated)
                return;

            _elapsedTime = 0;
            _activated = true;
            _adsIcon.enabled = false;

            StartCoroutine(Working(durationInSeconds));
        }

        private IEnumerator Working(float durationInSeconds)
        {
            _outline.fillAmount = 1f;
            
            while (_elapsedTime < durationInSeconds)
            {
                _elapsedTime += Time.deltaTime;
                _outline.fillAmount = 1f - _elapsedTime / durationInSeconds;

                yield return null;
            }

            _activated = false;
            _adsIcon.enabled = true;
            
            Deactivated?.Invoke(this);
        }
    }
}