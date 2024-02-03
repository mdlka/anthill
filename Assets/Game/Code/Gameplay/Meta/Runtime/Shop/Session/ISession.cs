namespace YellowSquad.Anthill.Meta
{
    public interface ISession
    {
        float MaxAntMoveDuration { get; }
        
        bool CanAddDigger { get; }
        bool CanAddLoader { get; }

        void AddDigger();
        void AddLoader();
        void ChangeAntsMoveDuration(float value);

        void Visualize(ISessionView view);
    }
}