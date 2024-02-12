using YellowSquad.GameLoop;

namespace YellowSquad.Anthill.Session
{
    public interface ISession : IGameLoop
    {
        int MaxDiggers { get; }
        int MaxLoaders { get; }
        int CurrentDiggers { get; }
        int CurrentLoaders { get; }
        
        bool CanAddDigger { get; }
        bool CanAddLoader { get; }

        void AddDigger();
        void AddLoader();
        void ChangeAntsMoveDuration(float value);
    }
}