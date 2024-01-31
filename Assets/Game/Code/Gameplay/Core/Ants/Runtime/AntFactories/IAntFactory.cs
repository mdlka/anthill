using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IAntFactory
    {
        IAnt CreateDigger(IHome home, AxialCoordinate startPosition);
        IAnt CreateLoader(IHome home, AxialCoordinate startPosition);
    }
}