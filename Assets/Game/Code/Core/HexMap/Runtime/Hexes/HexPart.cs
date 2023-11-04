using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexPart : IHexPart
    {
        public HexPart(Vector3 position)
        {
            LocalPosition = position;
        }
        
        public Vector3 LocalPosition { get; }
        public bool Destroyed { get; private set; }
        
        public void Destroy()
        {
            Destroyed = true;
        }
    }
}