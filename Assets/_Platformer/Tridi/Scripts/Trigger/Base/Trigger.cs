using System;
using System.Linq;
using UnityEngine;

namespace Tridi
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Trigger : MonoBehaviour
    {
        [Serializable]
        public class EventSettings
        {
            [SerializeField] public bool execute = true;
            [SerializeField] public bool once;

            [HideInInspector] public bool done;
        }

        [Header("Trigger")] 
        [SerializeField] private string[] tags = { "Player" };
        [Space]
        [SerializeField] private EventSettings enter;
        [SerializeField] private EventSettings exit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!tags.Any(other.CompareTag)) return;
            if (!enter.execute || enter.done) return;
            enter.done = enter.once;
            Enter(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!tags.Any(other.CompareTag)) return;
            if (!exit.execute || exit.done) return;
            exit.done = exit.once;
            Exit(other);
        }

        public virtual void Enter(Collider2D other)
        {
            Debug.Log($"{gameObject.name} doesn't implement Enter event");
        }

        public virtual void Exit(Collider2D other)
        {
            Debug.Log($"{gameObject.name} doesn't implement Exit event");
        }
    }
}
