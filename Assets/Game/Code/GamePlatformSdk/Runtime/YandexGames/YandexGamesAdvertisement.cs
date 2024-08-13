using System;
using Agava.YandexGames;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesAdvertisement : BaseAdvertisement
    {
        protected override void OnShowInterstitial(Action onEnd)
        {
            AudioListener.pause = true;
            onEnd += () => AudioListener.pause = false;
            
            InterstitialAd.Show(
                onCloseCallback: _ => onEnd.Invoke(), 
                onErrorCallback: _ => onEnd.Invoke(), 
                onOfflineCallback: onEnd.Invoke);
        }

        protected override void OnShowRewarded(Action<Result> onEnd)
        {
            AudioListener.pause = true;
            onEnd += _ => AudioListener.pause = false;
            
            VideoAd.Show(
                onRewardedCallback: () => onEnd.Invoke(Result.Success), 
                onCloseCallback: () => onEnd.Invoke(Result.Failure), 
                onErrorCallback: _ => onEnd.Invoke(Result.Failure));
        }
    }
}