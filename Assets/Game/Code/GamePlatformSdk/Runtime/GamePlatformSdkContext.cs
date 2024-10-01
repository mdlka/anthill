namespace YellowSquad.GamePlatformSdk
{
    public static class GamePlatformSdkContext
    {
#if UNITY_EDITOR
        private static bool _debugEnabled;
#endif
        
        static GamePlatformSdkContext()
        {
#if YANDEX && !UNITY_EDITOR
            Current = new YandexGamesSdk();
#elif VK_GAMES && !UNITY_EDITOR
            Current = new VkGamesSdk();
#else
            Current = new DefaultGamePlatformSdk();
#endif
        }
        
        public static IGamePlatformSdk Current { get; private set; }

#if UNITY_EDITOR
        public static void EnableLanguageDebug(Language targetLanguage)
        {
            if (_debugEnabled)
                return;
            
            Current = new DebugLanguageSdk(Current, targetLanguage);
            _debugEnabled = true;
        }
#endif
    }
}