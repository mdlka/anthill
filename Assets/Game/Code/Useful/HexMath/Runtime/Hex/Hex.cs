namespace YellowSquad.HexMath
{
    public class Hex
    {
        private readonly AxialCoordinate _position;

        public Hex(AxialCoordinate position)
        {
            _position = position;
        }

        public AxialCoordinate Position => _position;

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + _position.GetHashCode();
                return hash;
            }
        }
        
        public bool Equals(Hex other) => _position == other._position;
        public override bool Equals(object obj) => obj is Hex hex && Equals(hex);
        
        public static bool operator ==(Hex hex1, Hex hex2) => hex1.Equals(hex2);
        public static bool operator !=(Hex hex1, Hex hex2) => hex1.Equals(hex2) == false;
    }
}