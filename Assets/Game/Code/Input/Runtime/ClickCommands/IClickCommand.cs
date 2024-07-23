using YellowSquad.HexMath;

namespace YellowSquad.Anthill.UserInput
{
    public interface IClickCommand
    {
        bool CanExecute(AxialCoordinate position);
        void Execute(AxialCoordinate position);
    }
}