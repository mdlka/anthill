using System.Collections;
using System.Collections.Generic;

namespace YellowSquad.GamePlatformSdk
{
    public interface IConfig
    {
        IEnumerator Load(Dictionary<string, string> defaultConfig);
        
        string ValueBy(string key);
    }
}