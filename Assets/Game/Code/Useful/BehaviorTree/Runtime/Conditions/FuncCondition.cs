using System;

namespace YellowSquad.BehaviorTree
{
    public class FuncCondition : ICondition
    {
        private readonly Func<bool> _condition;

        public FuncCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public bool Completed => _condition();
    }
}