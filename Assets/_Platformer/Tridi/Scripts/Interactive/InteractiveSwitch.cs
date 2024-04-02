using UnityEngine;

namespace Tridi
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractiveSwitch : MonoBehaviour, IInteractive
    {
        [SerializeField] private int priority;
        [SerializeField] private SwitchActions onInteract;

        public int Priority => priority;

        public void Interact(Interactor instigator)
        {
            onInteract.Invoke();
        }
    }
}