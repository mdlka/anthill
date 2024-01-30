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
        public bool Destroyed { get; private set; }
        
        public void Destroy()
        {
            Destroyed = true;
        }

        public void Restore()
        {
            Destroyed = false;
        }
    }
}