﻿using System;
using UnityEngine;

namespace YellowSquad.Anthill.Core.CameraControl
{
    public interface ICamera
    {
        Vector3 Position { get; }
        float ZoomFactor { get; }

        void StartMove(Vector2 pointerPosition);
        void Move(Vector2 pointerPosition);
        
        void Zoom(float delta, Func<Vector2> pointerPosition);

        Vector3 ScreenToWorldPoint(Vector3 position);
        Ray ScreenPointToRay(Vector3 position);
    }
}