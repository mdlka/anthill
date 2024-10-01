using System.Collections;
using Agava.VKGames;

namespace YellowSquad.GamePlatformSdk
{
    internal class VkGamesSdk : IGamePlatformSdk
    {
        public IEnumerator Initialize()
        {
            yield return VKGamesSdk.Initialize();
            
            Language = Language.Russian;
            Initialized = true;
        }

        public void Ready() { }

        public bool Initialized { get; private set; }

        public IAdvertisement Advertisement { get; } = new VkGamesAdvertisement();
        public IConfig Config { get; } = new LocalConfig();
        public ISave Save { get; } = new PlayerPrefsSave();
        public ISocialInteraction SocialInteraction { get; } = new VkSocialInteraction();
        public Language Language { get; private set; }
    }
}
