using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexPart : IHexPart
    {
        public HexPart(Vector3 position)
        {
            Position = position;
        }
        
        public Vector3 Position { get; }
        public bool NeedRender { get; private set; } = true;
        
        public void Disable()
        {
            NeedRender = false;
        }
    }
}