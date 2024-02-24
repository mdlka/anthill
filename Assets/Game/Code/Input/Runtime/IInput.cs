using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Input
{
    public interface IInput
    {
        bool ClickedOnOpenMapPosition(out AxialCoordinate position);
    }
}
