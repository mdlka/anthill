using UnityEngine.SceneManagement;
using YellowSquad.Anthill.Meta;
using YellowSquad.GameLoop;

namespace YellowSquad.Anthill.Levels
{
    public class LevelSwitch : IGameLoop
    {
        private readonly LevelList _levelList;
        private readonly MapGoal _mapGoal;
        private readonly ILevelSwitchView _view;
        
        public LevelSwitch(LevelList levelList, MapGoal mapGoal, ILevelSwitchView view)
        {
            _levelList = levelList;
            _mapGoal = mapGoal;
            _view = view;
        }
        
        public void Update(float deltaTime)
        {
            if (_mapGoal.Complete == false)
                return;

            if (_view.Rendered)
                return;
            
            _view.Render(onNextLevelButtonClick: () =>
            {
                _levelList.NextLevel();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
    }
}
