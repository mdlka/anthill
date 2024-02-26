namespace YellowSquad.Anthill.Core.CameraControl
{
    public struct MinMaxFloat
    {
        public MinMaxFloat(float min, float max)
        {
            Min = min;
            Max = max;
        }        
        
        public float Min { get; }
        public float Max { get; }
    }
}