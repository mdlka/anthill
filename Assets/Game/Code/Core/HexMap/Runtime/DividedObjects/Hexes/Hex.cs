namespace YellowSquad.Anthill.Core.HexMap
{
    public class Hex : BaseDividedObject, IHex
    {
        internal Hex(Hardness hardness, IDividedObjectMesh mesh) : base(hardness, mesh) { }
        
        public override bool CanRestore => false;
    }
}