using YellowSquad.GameLoop;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IAntsList : IReadOnlyAntsList, IGameLoop
    {
        void Add(IAnt ant);
    }
    
    public interface IReadOnlyAntsList
    {
        int CurrentCount { get; }
        int MaxCount { get; }
    }
}