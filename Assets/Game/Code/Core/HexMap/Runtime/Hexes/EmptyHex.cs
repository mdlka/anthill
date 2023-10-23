using System;

namespace YellowSquad.Anthill.Core.HexMap
{
    public class EmptyHex : BaseHex
    {
        public EmptyHex() : base(Array.Empty<IHexPart>()) { }
        internal EmptyHex(IHexMesh hexMesh) : base(hexMesh) { }
        
        public override bool IsObstacle => false;
    }
}