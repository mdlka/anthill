namespace YellowSquad.GamePlatformSdk
{
    public static class GamePlatformSdkContext
    {
        static GamePlatformSdkContext()
        {
#if YANDEX && !UNITY_EDITOR
            Current = new YandexGamesSdk();
#else
            Current = new DefaultGamePlatformSdk();
#endif
        }
        
        public static IGamePlatformSdk Current { get; }
    }
}