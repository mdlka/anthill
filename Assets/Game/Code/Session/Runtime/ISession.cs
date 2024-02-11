using YellowSquad.GameLoop;

namespace YellowSquad.Anthill.Session
{
    public interface ISession : IGameLoop
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