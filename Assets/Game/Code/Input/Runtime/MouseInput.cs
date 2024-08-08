using UnityEngine;

namespace YellowSquad.Anthill.UserInput
{
    public class MouseInput : IInput
    {
        public bool PointerDown => Input.GetMouseButtonDown(0);
        public bool PointerUp => Input.GetMouseButtonUp(0);
        
        public Vector2 PointerPosition => Input.mousePosition;
        public float ZoomDelta => Input.mouseScrollDelta.y;
    }
}