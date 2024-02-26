using System;
using UnityEngine;

namespace YellowSquad.Anthill.Core.CameraControl
{
    public interface ICamera
    {
        void Move(Vector2 delta);
        void Zoom(float delta, Func<Vector2> cursorPosition);
    }
}