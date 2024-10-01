using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    internal class DebugLanguageSdk : IGamePlatformSdk
    {
        private readonly IGamePlatformSdk _targetSdk;

        public DebugLanguageSdk(IGamePlatformSdk targetSdk, Language debugLanguage)
        {
            _targetSdk = targetSdk;
            Language = debugLanguage;
        }

        public IEnumerator Initialize()
        {
            yield return _targetSdk.Initialize();
        }

        public void Ready()
        {
            _targetSdk.Ready();
        }

        public bool Initialized => _targetSdk.Initialized;
        public IAdvertisement Advertisement => _targetSdk.Advertisement;
        public IConfig Config => _targetSdk.Config;
        public ISave Save => _targetSdk.Save;
        public ISocialInteraction SocialInteraction => _targetSdk.SocialInteraction;
        public Language Language { get; }
    }
}
