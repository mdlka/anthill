namespace YellowSquad.Anthill.Core.HexMap
{
    public class Leaf : BaseDividedObject, IDividedPointOfInterest
    {
        internal Leaf(Hardness hardness, IDividedObjectMesh mesh) : base(hardness, mesh) { }

        public override bool CanRestore => HasParts == false;
    }
}