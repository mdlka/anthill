using System;
using Agava.VKGames;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class VkGamesAdvertisement : BaseAdvertisement
    {
        protected override void OnShowInterstitial(Action onEnd)
        {
            AudioListener.pause = true;
            Time.timeScale = 0;
                        
            onEnd += () =>
            {
                AudioListener.pause = false;
                Time.timeScale = 1f;
            };
            
            Interstitial.Show(
                onOpenCallback: () => onEnd.Invoke(), 
                onErrorCallback: () => onEnd.Invoke());
        }

        protected override void OnShowRewarded(Action<Result> onEnd)
        {
            AudioListener.pause = true;
            Time.timeScale = 0;
            
            onEnd += _ =>
            {
                AudioListener.pause = false;
                Time.timeScale = 1f;
            };
            
            VideoAd.Show(
                onRewardedCallback: () => onEnd.Invoke(Result.Success), 
                onErrorCallback: () => onEnd.Invoke(Result.Failure));
        }
    }
}