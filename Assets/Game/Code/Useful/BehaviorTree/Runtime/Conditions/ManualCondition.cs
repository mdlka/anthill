namespace YellowSquad.BehaviorTree
{
    public class ManualCondition : ICondition
    {
        public bool Completed { get; private set; }

        public void Complete() => Completed = true;
        public void Reset() => Completed = false;
    }
}