using System;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class Part : IPart
    {
        public Part(Vector3 position)
        {
            LocalPosition = position;
        }
        
        public Vector3 LocalPosition { get; }
        public float Size { get; private set; } = 1;
        public bool Destroyed { get; private set; }

        public void Resize(float size)
        {
            if (size is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(size));

            Size = size;
        }

        public void Destroy()
        {
            Destroyed = true;
        }

        public void Restore()
        {
            Size = 1f;
            Destroyed = false;
        }
    }
}