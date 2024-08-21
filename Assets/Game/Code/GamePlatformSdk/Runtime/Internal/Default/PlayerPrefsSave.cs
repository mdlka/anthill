using System.Collections;
using UnityEngine;

namespace YellowSquad.GamePlatformSdk
{
    internal class PlayerPrefsSave : ISave
    {
        public IEnumerator Load()
        {
            yield break;
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
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

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public int GetLeaderboardScore(string leaderboardName)
        {
            return PlayerPrefs.GetInt(leaderboardName, 0);
        }

        public void SetLeaderboardScore(string leaderboardName, int value)
        {
            PlayerPrefs.SetInt(leaderboardName, value);
        }

        public void Save()
        {
            PlayerPrefs.Save();
            
#if UNITY_EDITOR
            Debug.Log("Save");
#endif
        }
    }
}