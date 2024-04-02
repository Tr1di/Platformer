using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tridi
{
    [RequireComponent(typeof(Collider2D))]
    public class Interactor : MonoBehaviour
    {
        private List<IInteractive> _interactiveObjects = new();

        #region Collision

        private void OnCollisionEnter2D(Collision2D other)
        {
            AddObject(other.collider);
        }
    
        private void OnCollisionExit2D(Collision2D other)
        {
            RemoveObject(other.collider);
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            AddObject(other);
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            RemoveObject(other);
        }

        #endregion
        
        private void AddObject(Collider2D other)
        {
            var interactive = other.GetComponent<IInteractive>();
            if (interactive != null) _interactiveObjects.Add(interactive);
        }
    
        private void RemoveObject(Collider2D other)
        {
            var interactive = other.GetComponent<IInteractive>();
            if (interactive != null) _interactiveObjects.Remove(interactive);
        }
        
        private void Sort()
        {
            if (_interactiveObjects.Count < 1) return;
            _interactiveObjects = _interactiveObjects.OrderBy(x => x.Priority).ToList();
        }
        
        public void Interact()
        {
            Sort();
            var interactive = _interactiveObjects.LastOrDefault();
            interactive?.Interact(this);
        }

        public bool HasInteractions()
        {
            return _interactiveObjects.Count > 0;
        }
    }
}
