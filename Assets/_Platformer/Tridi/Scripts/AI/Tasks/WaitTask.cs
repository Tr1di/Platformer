using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Tridi.AI
{
    [Serializable]
    public class WaitTask : BaseTask
    {
        [SerializeField] private float duration = 2f;

        public WaitTask()
        {}

        public WaitTask(float duration)
        {
            this.duration = duration;
        }
        
        protected override IEnumerator Execute(CancellationToken token)
        {
            var time = duration;
            while (time > 0f)
            {
                if (token.IsCancellationRequested) break;
                time -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            State = token.IsCancellationRequested ? TaskState.Canceled : TaskState.Completed;
        }
    }
}