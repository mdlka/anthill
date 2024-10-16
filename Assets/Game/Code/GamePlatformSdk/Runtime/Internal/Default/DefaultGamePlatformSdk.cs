﻿using System.Collections;
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
                 SystemLanguage.Turkish => Language.Turkish,
                _ => Language.English
            };

            Debug.Log("Sdk initialized");
            Initialized = true;
            yield break;
        }

        public void Ready()
        {
            Debug.Log("Game ready");
        }

        public bool Initialized { get; private set; }
        public IAdvertisement Advertisement { get; } = new DefaultAdvertisement();
        public IConfig Config { get; } = new LocalConfig();
        public ISave Save { get; } = new PlayerPrefsSave();
        public ISocialInteraction SocialInteraction { get; } = new NullableSocialInteraction();
        public Language Language { get; private set; }
    }
}