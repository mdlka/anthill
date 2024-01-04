using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal interface IHexPart : IReadOnlyHexPart
    {
        void Destroy();
    }

    public interface IReadOnlyHexPart
    {
        Vector3 LocalPosition { get; }
        bool Destroyed { get; }
    }
}