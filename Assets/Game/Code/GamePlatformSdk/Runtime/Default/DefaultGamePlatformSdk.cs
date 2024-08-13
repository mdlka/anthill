using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class DefaultGamePlatformSdk : IGamePlatformSdk
    {
        public IEnumerator Initialize()
        {
            Language = Application.systemLanguage switch
            {
                 SystemLanguage.Russian or SystemLanguage.Belarusian or SystemLanguage.Ukrainian => Language.Russian,
                _ => Language.English
            };

            Debug.Log("Sdk initialized");
            yield break;
        }

        public bool Initialized => true;
        public IAdvertisement Advertisement { get; } = new DefaultAdvertisement();
        public ISave Save { get; } = new DefaultSave();
        public Language Language { get; private set; }
    }
}