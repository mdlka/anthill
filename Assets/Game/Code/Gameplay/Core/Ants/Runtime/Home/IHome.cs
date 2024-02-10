using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IHome : IReadOnlyTaskStorage
    {
        AxialCoordinate Position { get; }
    }
}