using System;

namespace YellowSquad.Anthill.Meta
{
    public class MapGoal
    {
        private readonly int _targetValue;
        private readonly IMapGoalView _view;
        
        private int _currentValue;

        public MapGoal(int targetValue, IMapGoalView view)
        {
            _targetValue = targetValue;
            _view = view;
            
            _view.Render(0, _targetValue);
        }

        public bool Complete => _currentValue >= _targetValue;

        public void AddProgress(int value = 1)
        {
            if (value < 0)
                throw new InvalidOperationException();
            
            _currentValue += value;
            _view.Render(_currentValue, _targetValue);
        }
    }
}
