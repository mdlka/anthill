namespace YellowSquad.GamePlatformSdk
{
    public static class GamePlatformSdkContext
    {
        private static bool _debugEnabled;
        
        static GamePlatformSdkContext()
        {
#if YANDEX && !UNITY_EDITOR
            Current = new YandexGamesSdk();
#else
            Current = new DefaultGamePlatformSdk();
#endif
        }
        
        public static IGamePlatformSdk Current { get; private set; }

        public static void EnableLanguageDebug(Language targetLanguage)
        {
            if (_debugEnabled)
                return;
            
            Current = new DebugLanguageSdk(Current, targetLanguage);
            _debugEnabled = true;
        }
    }
}