using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexPart : IReadOnlyHexPart
    {
        void Destroy();
    }

    public interface IReadOnlyHexPart
    {
        Vector3 LocalPosition { get; }
        bool Destroyed { get; }
    }
}