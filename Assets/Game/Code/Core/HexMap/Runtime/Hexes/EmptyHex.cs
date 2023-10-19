using System;

namespace YellowSquad.Anthill.Core.HexMap
{
    public class EmptyHex : BaseHex
    {
        public EmptyHex() : base(Array.Empty<IHexPart>()) { }
        public EmptyHex(IHexMesh hexMesh) : base(hexMesh) { }
        
        public override bool IsObstacle => false;
    }
}