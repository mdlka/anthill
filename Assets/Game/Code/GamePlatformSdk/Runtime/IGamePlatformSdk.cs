using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    public interface IGamePlatformSdk
    {
        IEnumerator Initialize();

        bool Initialized { get; }
        IAdvertisement Advertisement { get; }
        ISave Save { get; }
        
        Language Language { get; }
    }
}