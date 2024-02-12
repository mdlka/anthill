using YellowSquad.GameLoop;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IAnthill : IGameLoop
    {
        IReadOnlyAntsList Loaders { get; }
        IReadOnlyAntsList Diggers { get; }
        
        bool CanAddLoader { get; }
        bool CanAddDigger { get; }
        
        void AddLoader();
        void AddDigger();
    }
}