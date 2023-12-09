namespace YellowSquad.Anthill.Core.HexMap
{
    public struct MapCell
    {
        internal MapCell(IHex hex, PointOfInterest pointOfInterest)
        {
            Hex = hex;
            PointOfInterest = pointOfInterest;
        }

        public IHex Hex { get; }
        public PointOfInterest PointOfInterest { get; }
    }
}