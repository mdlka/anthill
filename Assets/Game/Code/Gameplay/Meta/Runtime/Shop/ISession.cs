namespace YellowSquad.Anthill.Meta
{
    public interface ISession
    {
        bool CanAddDigger { get; }
        bool CanAddLoader { get; }
        bool CanIncreaseSpeed { get; }

        void AddDigger();
        void AddLoader();
        void IncreaseSpeed();
    }
}