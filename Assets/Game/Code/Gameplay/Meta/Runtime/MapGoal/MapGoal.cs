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

        public void AddProgress()
        {
            _currentValue += 1;
            _view.Render(_currentValue, _targetValue);
        }
    }
}
