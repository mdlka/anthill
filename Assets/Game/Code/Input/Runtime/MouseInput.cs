using UnityEngine;

namespace YellowSquad.Anthill.UserInput
{
    public class MouseInput : IInput
    {
        public bool PointerDown => Input.GetMouseButtonDown(0);
        public bool PointerUp => Input.GetMouseButtonUp(0);
        
        public Vector2 PointerPosition => Input.mousePosition;
        public Vector2 MoveDelta => -new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        public float ZoomDelta => Input.mouseScrollDelta.y;
    }
}