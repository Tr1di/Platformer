using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Tridi.AI
{
    public delegate void OnActionEvent(IAction action);
    
    public interface IAction
    {
        public bool IsIdle { get; }
        public bool IsActive { get; }
        public bool IsCompleted { get; }
        public bool IsCanceled { get; }
        public ITaskBuilder Then { get; }
        public TaskState State { get; }
        public ICondition If(Predicate<ITask> predicate);
        public ICondition While(Predicate<ITask> predicate);
        public ICondition Until(Predicate<ITask> predicate);
        public IRepeat For(float forValue);
        public ICycling For();
        public IAction Once();
        public IDelay WithDelay(float seconds);
        public IEnumerator Execute(CancellationToken token);
        public IAction OnCompleted(OnActionEvent method);
        public IAction OnCanceled(OnActionEvent method);
    }
    
    namespace Implementation
    {
        public class ActionImpl : IAction
        {
            public bool IsIdle => State == TaskState.Waiting;
            public bool IsActive => State == TaskState.Executing;
            public bool IsCompleted => State == TaskState.Completed;
            public bool IsCanceled => State == TaskState.Canceled;
            
            private Action _action;

            private IList<ICondition> _if = new List<ICondition>();
            private IList<ICondition> _while = new List<ICondition>();
            private IList<ICondition> _until = new List<ICondition>();
            private ICycling _cycling;
            private IDelay _delay;

            public ITaskBuilder Then { get; }
            public TaskState State { get; private set; } = TaskState.Waiting;

            public event OnActionEvent onActionCompleted;
            public event OnActionEvent onActionCanceled;
            
            public ActionImpl(ITaskBuilder builder, Action action)
            {
                Then = builder;
                _action = action;
                _cycling = new CyclingImpl(this);
                _delay = new DelayImpl(this);
            }

            private bool ShouldExecute()
            {
                return _if.Aggregate(true, (current, cond) => current & cond.Invoke(Then.Prepare()));
            }

            private bool StillExecute()
            {
                return _while.Aggregate(true, (current, cond) => current & cond.Invoke(Then.Prepare()));
            }

            private bool ShouldComplete()
            {
                return _until.Count > 0 &&
                       _until.Aggregate(true, (current, cond) => current & cond.Invoke(Then.Prepare()));
            }

            public IEnumerator Execute(CancellationToken token)
            {
                if (!ShouldExecute()) yield break;

                State = TaskState.Executing;
                
                if (_delay.Duration > 0f)
                    yield return new WaitForSeconds(_delay.Duration);

                var counter = _cycling.Duration;

                while (StillExecute())
                {
                    _action.Invoke();

                    counter -= _cycling.Type == RepeatType.Seconds ? _cycling.Update : 1;

                    if (token.IsCancellationRequested) break;
                    if (_cycling.Duration > 0 && counter < 0) break;
                    if (ShouldComplete()) break;

                    yield return new WaitForSeconds(_cycling.Update);
                }

                State = token.IsCancellationRequested ? TaskState.Canceled : TaskState.Completed;
            }

            public IAction OnCompleted(OnActionEvent method)
            {
                onActionCompleted += method;
                return this;
            }

            public IAction OnCanceled(OnActionEvent method)
            {
                onActionCanceled += method;
                return this;
            }

            public ICondition If(Predicate<ITask> predicate)
            {
                var cond = new ConditionImpl(this, predicate);
                _if.Add(cond);
                return cond;
            }

            public ICondition While(Predicate<ITask> predicate)
            {
                var cond = new ConditionImpl(this, predicate);
                _while.Add(cond);
                return cond;
            }

            public ICondition Until(Predicate<ITask> predicate)
            {
                var cond = new ConditionImpl(this, predicate);
                _until.Add(cond);
                return cond;
            }

            public IRepeat For(float forValue)
            {
                var repeat = new RepeatImpl(this, forValue);
                _cycling = repeat.Times;
                return repeat;
            }

            public ICycling For()
            {
                For(-1);
                return _cycling;
            }

            public IAction Once()
            {
                return For(1).Times.EveryFixedUpdate();
            }

            public IDelay WithDelay(float seconds)
            {
                _delay = new DelayImpl(this, seconds);
                return _delay;
            }
        }
    }
}
