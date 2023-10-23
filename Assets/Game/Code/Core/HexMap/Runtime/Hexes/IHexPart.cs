using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexPart : IReadOnlyHexPart
    {
        void Disable();
    }

    public interface IReadOnlyHexPart
    {
        Vector3 LocalPosition { get; }
        bool NeedRender { get; }
    }
}