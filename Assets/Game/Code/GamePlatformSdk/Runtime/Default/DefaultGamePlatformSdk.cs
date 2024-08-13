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
            Initialized = true;
            yield break;
        }

        public bool Initialized { get; private set; }
        public IAdvertisement Advertisement { get; } = new DefaultAdvertisement();
        public ISave Save { get; } = new PlayerPrefsSave();
        public Language Language { get; private set; }
    }
}