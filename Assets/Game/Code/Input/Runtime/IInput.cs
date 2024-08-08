using UnityEngine;

namespace YellowSquad.Anthill.UserInput
{
    public interface IInput
    {
        bool PointerDown { get; }
        bool PointerUp { get; }
        
        Vector2 PointerPosition { get; }
        float ZoomDelta { get; }
    }
}
