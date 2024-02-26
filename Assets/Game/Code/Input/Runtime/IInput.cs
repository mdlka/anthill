using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Input
{
    public interface IInput
    {
        Vector2 CursorPosition { get; }
        
        bool ClickedOnOpenMapPosition(out AxialCoordinate position);
        bool Zoomed(out float delta);
    }
}
