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

        public bool Initialized => _targetSdk.Initialized;
        public IAdvertisement Advertisement => _targetSdk.Advertisement;
        public ISave Save => _targetSdk.Save;
        public Language Language { get; }
    }
}
