using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesConfig : IConfig
    {
        private readonly Dictionary<string, string> _config = new();
        
        private bool _loaded;
        
        public IEnumerator Load(Dictionary<string, string> defaultConfig)
        {
            if (_loaded)
                yield break;

            Flags.Get(defaultConfig, onSuccessCallback: flags =>
                {
                    foreach (var pair in flags)
                        _config[pair.Key] = pair.Value;
                    
                    _loaded = true;
                }, 
                onErrorCallback: _ =>
                {
                    foreach (var pair in defaultConfig)
                        _config[pair.Key] = pair.Value;

                    _loaded = true;
                });

            yield return new WaitUntil(() => _loaded);
        }

        public string ValueBy(string key)
        {
            if (_loaded == false)
                throw new InvalidOperationException();

            return _config[key];
        }
    }
}