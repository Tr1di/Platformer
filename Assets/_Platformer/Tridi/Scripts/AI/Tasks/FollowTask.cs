using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Tridi.AI
{
    [Serializable]
    public class FollowTask : BaseTask
    {
        [Header("Character")]
        [SerializeField] private Transform center;
        [SerializeField] private Movement movement;
        
        [Header("Settings")]
        [SerializeField, Min(0f)] private float threshold = 2f;
        
        public float Threshold => threshold;
        public Vector3 Target { get; private set; }

        protected override IEnumerator Execute(CancellationToken token)
        {
            while ((Target - center.position).magnitude > threshold)
            {
                if (token.IsCancellationRequested) break;
                movement.Move((Target - center.position).normalized);
                yield return new WaitForFixedUpdate();
            }
            
            movement.Move(Vector2.zero);
            State = token.IsCancellationRequested ? TaskState.Canceled : TaskState.Completed;
        }

        public void SetTarget(Vector3 transform)
        {
            Target = transform;
        }
        
        public void SetTarget(Transform transform)
        {
            Target = transform.position;
        }
    }
}