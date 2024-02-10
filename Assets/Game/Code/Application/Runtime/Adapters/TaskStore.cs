using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.Anthill.Meta;

namespace YellowSquad.Anthill.Application.Adapters
{
    public class TaskStore : ITaskStore
    {
        private readonly IWallet _wallet;

        public TaskStore(IWallet wallet)
        {
            _wallet = wallet;
        }
        
        public void Sell(ITask task)
        {
            _wallet.Add(task.Price);
        }
    }
}