using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.Anthill.Meta;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Application.Adapters
{
    public class TaskStore : ITaskStore
    {
        private readonly float _mapScale;
        private readonly IWallet _wallet;
        private readonly TaskStoreView _taskStoreView;

        public TaskStore(float mapScale, IWallet wallet, TaskStoreView taskStoreView)
        {
            _mapScale = mapScale;
            _wallet = wallet;
            _taskStoreView = taskStoreView;
        }

        public void Sell(ITask task, AxialCoordinate sellPosition)
        {
            _wallet.Add(task.Price);
            
            if (task.Price > 0)
                _taskStoreView.Render(sellPosition.ToVector3(_mapScale));
        }
    }
}