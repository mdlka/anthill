using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    public interface IGamePlatformSdk
    {
        IEnumerator Initialize();

        void Ready();

        bool Initialized { get; }
        IAdvertisement Advertisement { get; }
        IConfig Config { get; }
        ISave Save { get; }
        ISocialInteraction SocialInteraction { get; }

        Language Language { get; }
    }
}