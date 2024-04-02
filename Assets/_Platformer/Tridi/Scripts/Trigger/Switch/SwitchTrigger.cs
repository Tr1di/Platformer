using UnityEngine;

namespace Tridi
{
    public class SwitchTrigger : Trigger
    {
        [Header("Switch")] 
        [SerializeField] private SwitchActions onEnter;
        [Space]
        [SerializeField] private SwitchActions onExit;

        public override void Enter(Collider2D other)
        {
            onEnter.Invoke();
        }

        public override void Exit(Collider2D other)
        {
            onExit.Invoke();
        }
    }
}
