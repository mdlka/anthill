using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IHomeList : IReadOnlyHomeList
    {
        bool HasFreeHome { get; }
        IHome FindFreeHome();
        
        void AddAntTo(AxialCoordinate position);
    }

    public interface IReadOnlyHomeList
    {
        int BusyPlaces { get; }
        int OpenPlaces { get; }
    }
}