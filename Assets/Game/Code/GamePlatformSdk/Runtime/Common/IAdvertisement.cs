using System;
using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    public interface IAdvertisement
    {
        event Action AdsStarted;
        event Action AdsEnded;
        
        Result LastRewardedResult { get; }
        double LastAdTime { get; }

        void ShowInterstitial(Action onEnd);
        void ShowRewarded(Action<Result> onEnd);
        
        IEnumerator ShowInterstitial();
        IEnumerator ShowRewarded();
    }

    public enum Result
    {
        Failure = 1,
        Success = 2
    }
}