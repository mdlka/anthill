using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesSdk : IGamePlatformSdk
    {
        private bool _readyWasCalled;
        
        public IEnumerator Initialize()
        {
            yield return Agava.YandexGames.YandexGamesSdk.Initialize();
            yield return Save.Load();

            Language = Agava.YandexGames.YandexGamesSdk.Environment.i18n.lang switch
            {
                "en" => Language.English,
                "tr" => Language.Turkish,
                _ => Language.Russian
            };

            Initialized = true;
        }

        public void Ready()
        {
            if (_readyWasCalled)
                return;
            
            Agava.YandexGames.YandexGamesSdk.GameReady();
            _readyWasCalled = true;
        }

        public bool Initialized { get; private set; }

        public IAdvertisement Advertisement { get; } = new YandexGamesAdvertisement();
        public ISave Save { get; } = new YandexGamesCloudSave();
        public Language Language { get; private set; }
    }
}