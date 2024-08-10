using UnityEngine;

namespace YellowSquad.Anthill.UserInput
{
    public interface IInput
    {
        float ZoomDelta { get; }
        
        bool HasPointerDown { get; }
        bool HasPointerUp { get; }

        void Update();
        
        int ReadPointerDown();
        int ReadPointerUp();

        Vector2 PointerPosition(int pointerId);
    }
}
