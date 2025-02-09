using System;
using System.Collections;
using System.Threading;
using Tridi.AI.Implementation;
using Unity.VisualScripting;

namespace Tridi.AI
{
    public static class Task
    {
        public static IAction Do(Action action)
        {
            var task = new TaskImpl();
            return task.Do(action);
        }
    }
    
    public abstract class BaseTask : ITask
    {
        public bool IsIdle => State == TaskState.Waiting;
        public bool IsActive => State == TaskState.Executing;
        public bool IsCompleted => State == TaskState.Completed;
        public bool IsCanceled => State == TaskState.Canceled;

        private TaskState _state;
        
        public TaskState State
        {
            get => _state;
            protected set
            {
                if (_state == value) return;

                if (_state < TaskState.Completed)
                {
                    switch (value)
                    {
                        case TaskState.Completed:
                            onCompleted?.Invoke(this);
                            break;
                        case TaskState.Canceled:
                            onCancelded?.Invoke(this);
                            break;
                        case TaskState.Waiting:
                        case TaskState.Executing:
                        default:
                            break;
                    }
                }

                _state = value;
            }
        }

        private CancellationTokenSource _tokenSource = new();
        private CancellationToken Token => _tokenSource.Token;

        public delegate void OnStateChanged(ITask task);

        public event OnStateChanged onCompleted;
        public event OnStateChanged onCancelded;
        
        protected abstract IEnumerator Execute(CancellationToken token);
        
        public ITask Execute(bool reset = true)
        {
            if (reset && State > TaskState.Executing) State = TaskState.Waiting;
            if (State > TaskState.Waiting) return null;
            State = TaskState.Executing;
            
            CoroutineRunner.instance.StartCoroutine(Execute(Token));
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