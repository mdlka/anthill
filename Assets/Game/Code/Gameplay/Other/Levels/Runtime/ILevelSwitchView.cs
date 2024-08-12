using System;

namespace YellowSquad.Anthill.Levels
{
    public interface ILevelSwitchView
    {
        bool Rendered { get; }
        
        void Render(Action onNextLevelButtonClick);
    }
}