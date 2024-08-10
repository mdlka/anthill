using System;
using UnityEngine;

namespace YellowSquad.Anthill.UserInput
{
    public class MouseInput : IInput
    {
        public float ZoomDelta => Input.mouseScrollDelta.y;
        public bool HasPointerDown { get; private set; }
        public bool HasPointerUp { get; private set; }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
                HasPointerDown = true;

            if (Input.GetMouseButtonUp(0))
                HasPointerUp = true;
        }

        public int ReadPointerDown()
        {
            if (HasPointerDown == false)
                throw new InvalidOperationException();

            HasPointerDown = false;
            return 0;
        }

        public int ReadPointerUp()
        {
            if (HasPointerUp == false)
                throw new InvalidOperationException();

            HasPointerUp = false;
            return 0;
        }

        public Vector2 PointerPosition(int pointerId)
        {
            return Input.mousePosition;
        }
    }
}