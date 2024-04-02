using UnityEngine;
using UnityEngine.Events;

namespace Tridi
{
    public class SimpleTrigger : Trigger
    {
        [Header("Simple")] 
        [SerializeField] public UnityEvent onEnter;
        [Space] 
        [SerializeField] public UnityEvent onExit;

        public override void Enter(Collider2D other)
        {
            onEnter?.Invoke();
        }

        public override void Exit(Collider2D other)
        {
            onExit?.Invoke();
        }
    }
}