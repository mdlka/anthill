using YellowSquad.GameLoop;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IAnt : IGameLoop
    {
        FracAxialCoordinate CurrentPosition { get; }
        bool Moving { get; }
        
        bool HasPart { get; }
        bool InHome { get; }
    }
}