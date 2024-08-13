using System;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class DefaultAdvertisement : BaseAdvertisement
    {
        protected override void OnShowInterstitial(Action onEnd)
        {
            Debug.Log("Show Interstitial");
            onEnd.Invoke();
        }

        protected override void OnShowRewarded(Action<Result> onEnd)
        {
            Debug.Log("Show Rewarded");
            onEnd.Invoke(Result.Success);
        }
    }
}
