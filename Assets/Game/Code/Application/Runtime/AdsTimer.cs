using System;
using System.Collections;
using TMPro;
using UnityEngine;
using YellowSquad.GamePlatformSdk;
using YellowSquad.Utils;

namespace YellowSquad.Anthill.Application
{
    internal class AdsTimer : MonoBehaviour
    {
        [SerializeField] private int _delayBeforeAdsInSeconds = 3;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private LocalizedText[] _warningText;

        private bool _timerStarted;
        
        internal bool ShowAds { get; private set; }

        private void Awake()
        {
            _canvasGroup.Disable();
        }

        public void StartTimer(int delayInSeconds)
        {
            if (_timerStarted)
                throw new InvalidOperationException();

            StartCoroutine(Working(delayInSeconds));
        }

        private IEnumerator Working(int delayInSeconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(delayInSeconds);
                
                if (GamePlatformSdkContext.Current.Advertisement.CanShowInterstitial == false)
                    continue;

                int elapsedTime = 0;
                _timerText.text = $"{_warningText.SelectCurrentLanguageText()}: {_delayBeforeAdsInSeconds}";
                _canvasGroup.Enable(0.2f);

                ShowAds = true;

                while (elapsedTime < _delayBeforeAdsInSeconds)
                {
                    yield return new WaitForSecondsRealtime(1f);
                    elapsedTime += 1;
                
                    _timerText.text = $"{_warningText.SelectCurrentLanguageText()}: {_delayBeforeAdsInSeconds - elapsedTime}";
                }

                _canvasGroup.Disable();
                yield return GamePlatformSdkContext.Current.Advertisement.ShowInterstitial();

                ShowAds = false;
            }
        }
    }
}