using YellowSquad.GamePlatformSdk;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexMapFactory
    {
        IHexMap Create(ISave save);
    }
}