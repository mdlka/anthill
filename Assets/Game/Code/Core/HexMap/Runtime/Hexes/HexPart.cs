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
        public bool NeedRender { get; private set; } = true;
        
        public void Disable()
        {
            NeedRender = false;
        }
    }
}