using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace YellowSquad.GamePlatformSdk
{
    internal class YandexGamesCloudSave : ISave
    {
        public IEnumerator Load()
        {
            bool saveLoadEnded = false;
            
            PlayerPrefs.Load(onSuccessCallback: () => saveLoadEnded = true, onErrorCallback: _ => saveLoadEnded = true);
            yield return new WaitUntil(() => saveLoadEnded);
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public int GetLeaderboardScore(string leaderboardName)
        {
            return PlayerPrefs.GetInt(leaderboardName, 0);
        }

        public void SetLeaderboardScore(string leaderboardName, int value)
        {
            PlayerPrefs.SetInt(leaderboardName, value);
            
            if (PlayerAccount.IsAuthorized)
                Leaderboard.SetScore(leaderboardName, value);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}