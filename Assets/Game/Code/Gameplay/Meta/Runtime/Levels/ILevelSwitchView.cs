using System;

namespace YellowSquad.Anthill.Meta
{
    public interface ILevelSwitchView
    {
        bool Rendered { get; }
        
        void Render(Action onNextLevelButtonClick);
    }
}