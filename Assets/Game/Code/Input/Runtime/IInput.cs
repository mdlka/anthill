using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Input
{
    public interface IInput
    {
        Vector2 CursorPosition { get; }
        
        bool ClickedOnOpenMapPosition(out AxialCoordinate position);
        bool Moved(out Vector2 delta);
        bool Zoomed(out float delta);

        void Update(float deltaTime);
    }
}
