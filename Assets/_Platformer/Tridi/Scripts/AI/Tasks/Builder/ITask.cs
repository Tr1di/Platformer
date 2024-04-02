using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tridi.AI.Implementation;
using Unity.VisualScripting;

namespace Tridi.AI
{
    public interface ITaskBuilder
    {
        public IAction Do(Action action);
        public ITask Prepare();
    }

    public enum TaskState
    {
        Waiting,
        Executing,
        Completed,
        Canceled
    }

    public interface ITask
    {
        public bool IsIdle { get; }
        public bool IsActive { get; }
        public bool IsCompleted { get; }
        public bool IsCanceled { get; }
        public TaskState State { get; }
        public ITask Execute(bool reset = true);
        public void Cancel();
    }
    
    namespace Implementation
    {
        public class TaskImpl : ITask, ITaskBuilder
        {
            private List<IAction> _actions = new();

            private CancellationTokenSource _tokenSource = new();
            private CancellationToken Token => _tokenSource.Token;
        
            public IAction Do(Action action)
            {
                var act = new ActionImpl(this, action);
                _actions.Add(act);
                return act;
            }
        
            public ITask Prepare()
            {
                return this;
            }

            public bool IsIdle => State == TaskState.Waiting;
            public bool IsActive => State == TaskState.Executing;
            public bool IsCompleted => State == TaskState.Completed;
            public bool IsCanceled => State == TaskState.Canceled;
            public TaskState State { get; private set; }

            public ITask Execute(bool reset = true)
            {
                if (reset && State > TaskState.Executing) State = TaskState.Waiting;
                switch (State)
                {
                    case TaskState.Executing:
                        return this;
                    case > TaskState.Executing:
                        return null;
                }

                State = TaskState.Executing;
                _actions.ForEach(action => CoroutineRunner.instance.StartCoroutine(action.Execute(Token)));
                return this;
            }
        
            public void Cancel()
            {
                if (State > TaskState.Executing) return;
            
                State = TaskState.Canceled;
            
                _tokenSource.Cancel();
                _tokenSource = new CancellationTokenSource();
            }
        }
    }
}
