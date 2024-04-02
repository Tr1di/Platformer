using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Tridi
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class ItemPlacement : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private int initialAmount = 1;
        [SerializeField] private AudioClip pickupSound;
        [Space]
        [SerializeField] private UnityEvent onPickedUp;

        private Item _instance;
    
        private void OnValidate()
        {
            if (!item) return;
            
            GetComponent<SpriteRenderer>().sprite = item.Sprite;
            initialAmount = Math.Clamp(initialAmount, 0, item.MaxAmount);
        }
        
        private void Awake()
        {
            _instance = Instantiate(item);
            _instance.Amount = initialAmount;
        }
    
        private void OnEnable()
        {
            _instance.onPickedUp += OnPickedUp;
        }

        private void OnDisable()
        {
            _instance.onPickedUp -= OnPickedUp;
        }

        private void OnPickedUp(Item pickedItem)
        {
            onPickedUp.Invoke();
            pickupSound.PlayOneShot(transform.position);
            if (pickedItem.Amount == 0) gameObject.SetActive(false);
        }

        public void PickUp(Inventory inventory)
        {
            if (!inventory) return;
            inventory.PickUp(_instance);
        }
    }
}