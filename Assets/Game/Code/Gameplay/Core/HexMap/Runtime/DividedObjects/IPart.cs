using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IPart : IReadOnlyPart
    {
        void Resize(float size);
        
        void Destroy();
        void Restore();
    }

    public interface IReadOnlyPart
    {
        Vector3 LocalPosition { get; }
        float Size { get; }
        bool Destroyed { get; }
    }
}