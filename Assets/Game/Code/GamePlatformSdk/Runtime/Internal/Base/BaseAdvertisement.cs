using System;
using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal abstract class BaseAdvertisement : IAdvertisement
    {
        public event Action AdsStarted;
        public event Action AdsEnded;
        
        public Result LastRewardedResult { get; private set; }
        public double LastAdTime { get; private set; } = -SdkSettings.MinIntervalBetweenAdsInSeconds;
        public bool CanShowInterstitial => 
            Time.realtimeSinceStartupAsDouble - LastAdTime >= SdkSettings.MinIntervalBetweenAdsInSeconds;

        public void ShowInterstitial(Action onEnd)
        {
            if (CanShowInterstitial == false)
                return;

            AdsStarted?.Invoke();
            onEnd += () =>
            {
                LastAdTime = Time.realtimeSinceStartupAsDouble;
                AdsEnded?.Invoke();
            };
            OnShowInterstitial(onEnd);
        }

        public void ShowRewarded(Action<Result> onEnd)
        {
            AdsStarted?.Invoke();
            onEnd += _ =>
            {
                LastAdTime = Time.realtimeSinceStartupAsDouble;
                AdsEnded?.Invoke();
            };
            OnShowRewarded(onEnd);
        }

        public IEnumerator ShowInterstitial()
        {
            if (CanShowInterstitial == false)
                yield break;

            bool ended = false;
            
            AdsStarted?.Invoke();
            OnShowInterstitial(onEnd: () => ended = true);
            yield return new WaitUntil(() => ended);
            AdsEnded?.Invoke();
            
            LastAdTime = Time.realtimeSinceStartupAsDouble;
        }

        public IEnumerator ShowRewarded()
        {
            bool ended = false;
            LastRewardedResult = Result.Failure;
            
            AdsStarted?.Invoke();
            OnShowRewarded(onEnd: result => { ended = true; LastRewardedResult = result; });
            yield return new WaitUntil(() => ended);
            AdsEnded?.Invoke();
            
            LastAdTime = Time.realtimeSinceStartupAsDouble;
        }

        protected abstract void OnShowInterstitial(Action onEnd);
        protected abstract void OnShowRewarded(Action<Result> onEnd);
    }
}
