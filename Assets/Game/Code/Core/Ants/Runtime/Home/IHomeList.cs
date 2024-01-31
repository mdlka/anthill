using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IHomeList
    {
        bool HasFreeHome { get; }
        IHome FindFreeHome();
        
        void AddAntTo(AxialCoordinate position);
        void RemoveAntFrom(AxialCoordinate position);
    }
}