using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tridi
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractiveTrigger : MonoBehaviour, IInteractive
    {
        [SerializeField] private int priority = 1;
        [SerializeField] private UnityEvent onInteract;

        public int Priority => priority;

        public void Interact(Interactor instigator)
        {
            onInteract?.Invoke();
        }
    }
}
