namespace YellowSquad.Anthill.Meta
{
    public interface ISession
    {
        bool CanAddDigger { get; }
        bool CanAddLoader { get; }

        void AddDigger();
        void AddLoader();
        void ChangeAntsMoveDuration(float value);
    }
}