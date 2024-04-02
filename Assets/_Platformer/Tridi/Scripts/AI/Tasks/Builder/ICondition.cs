using System;

namespace Tridi.AI
{
    public interface ICondition
    {
        public IAction Also { get; }
        public ICondition And(Predicate<ITask> predicate);
        public ICondition Or(Predicate<ITask> predicate);
        public bool Invoke(ITask task);
    }

    namespace Implementation
    {
        public class ConditionImpl : ICondition
        {
            private Predicate<ITask> _predicate;

            public IAction Also { get; }

            public ConditionImpl(IAction action, Predicate<ITask> predicate)
            {
                Also = action;
                _predicate = predicate;
            }
        
            public ICondition And(Predicate<ITask> predicate)
            {
                var clone = _predicate.Clone();
                _predicate = task => ((Predicate<ITask>)clone).Invoke(task) && predicate.Invoke(task);
                return this;
            }

            public ICondition Or(Predicate<ITask> predicate)
            {
                var clone = _predicate.Clone();
                _predicate = task => ((Predicate<ITask>)clone).Invoke(task) || predicate.Invoke(task);
                return this;
            }

            public bool Invoke(ITask task)
            {
                return _predicate.Invoke(task);
            }
        }
    }
}