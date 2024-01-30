using System;

namespace YellowSquad.Anthill.Core.HexMap
{
    public readonly struct MapCell
    {
        private readonly IDividedPointOfInterest _dividedPointOfInterest;
        
        internal MapCell(IHex hex, PointOfInterestType pointOfInterestType, IDividedPointOfInterest dividedPointOfInterest)
        {
            Hex = hex;
            PointOfInterestType = pointOfInterestType;
            _dividedPointOfInterest = dividedPointOfInterest;
        }

        public IHex Hex { get; }
        public PointOfInterestType PointOfInterestType { get; }
        public IDividedPointOfInterest DividedPointOfInterest => _dividedPointOfInterest ?? throw new NullReferenceException();
        public bool HasDividedPointOfInterest => _dividedPointOfInterest != null;
    }
}