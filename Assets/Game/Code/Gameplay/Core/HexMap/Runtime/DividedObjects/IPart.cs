using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal interface IPart : IReadOnlyPart
    {
        void Destroy();
        void Restore();
    }

    public interface IReadOnlyPart
    {
        Vector3 LocalPosition { get; }
        bool Destroyed { get; }
    }
}