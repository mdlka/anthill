namespace YellowSquad.BehaviorTree
{
    public class ConstCondition : ICondition
    {
        public ConstCondition(bool completed)
        {
            Completed = completed;
        }
        
        public bool Completed { get;}
    }
}