using System;
using UnityEngine;

namespace YellowSquad.Anthill.Core.CameraControl
{
    public interface ICamera
    {
        Vector3 Position { get; }
        
        void Move(Vector2 delta);
        void Zoom(float delta, Func<Vector2> cursorPosition);

        Vector3 ScreenToWorldPoint(Vector3 position);
    }
}