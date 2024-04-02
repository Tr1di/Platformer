using UnityEngine;

namespace Tridi
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectableItem : ItemPlacement
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            PickUp(other.GetComponent<Inventory>());
        }
    }
}