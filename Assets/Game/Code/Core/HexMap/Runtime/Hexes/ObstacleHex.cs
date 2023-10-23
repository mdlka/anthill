using System;

namespace YellowSquad.Anthill.Core.HexMap
{
    public class ObstacleHex : BaseHex
    {
        public ObstacleHex() : base(Array.Empty<IHexPart>()) { }
        internal ObstacleHex(IHexMesh hexMesh) : base(hexMesh) { }
        
        public override bool IsObstacle => true;
    }
}