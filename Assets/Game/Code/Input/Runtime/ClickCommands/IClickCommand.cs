using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.UserInput
{
    public interface IClickCommand
    {
        bool TryExecute(ClickInfo clickInfo);
    }

    public struct ClickInfo
    {
        public Vector3 ScreenPosition { get; init; }
        public Vector3 WorldPosition { get; init; }
        public AxialCoordinate MapPosition { get; init; }
        public float ZoomFactor { get; init; }
    }
}