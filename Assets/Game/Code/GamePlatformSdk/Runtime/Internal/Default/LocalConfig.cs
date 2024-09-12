using System;
using System.Collections;
using System.Collections.Generic;

namespace YellowSquad.GamePlatformSdk
{
    internal class LocalConfig : IConfig
    {
        private readonly Dictionary<string, string> _config = new();
        
        private bool _loaded;

        public IEnumerator Load(Dictionary<string, string> defaultConfig)
        {
            if (_loaded)
                yield break;

            foreach (var pair in defaultConfig)
                _config[pair.Key] = pair.Value;

            _loaded = true;
        }

        public string ValueBy(string key)
        {
            if (_loaded == false)
                throw new InvalidOperationException();

            return _config[key];
        }
    }
}