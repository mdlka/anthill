using System.Collections;

namespace YellowSquad.GamePlatformSdk
{
    public interface ISave
    {
        IEnumerator Load();
        void Save();

        void DeleteAll();

        bool HasKey(string key);

        void SetString(string key, string value);
        string GetString(string key, string defaultValue = "");

        int GetLeaderboardScore(string leaderboardName);
        void SetLeaderboardScore(string leaderboardName, int value);
    }
}