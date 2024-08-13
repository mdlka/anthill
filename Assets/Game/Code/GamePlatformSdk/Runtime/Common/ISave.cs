using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    public interface ISave
    {
        IEnumerator Load();
        void Save();

        void DeleteAll();
        void DeleteKey(string key);

        bool HasKey(string key);

        void SetString(string key, string value);
        string GetString(string key, string defaultValue = "");

        void SetInt(string key, int value);
        int GetInt(string key, int defaultValue = 0);

        int GetLeaderboardScore(string leaderboardName);
        void SetLeaderboardScore(string leaderboardName, int value);
    }
}