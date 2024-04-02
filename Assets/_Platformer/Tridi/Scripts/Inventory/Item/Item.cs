using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tridi
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
    public class Item : ScriptableObject
    {
        [Id]
        [SerializeField] private string id;
        
        [Header("Item")]
        [SerializeField] private string title;
        [SerializeField] private Sprite sprite;
    
        [Header("Stacking")]
        [SerializeField, Min(0)] private int amount = 1;
        [SerializeField, Min(0)] private int maxAmount = 10;
    
        [Header("Info")]
        [TextArea(3, 10)]
        [SerializeField] private string description;

        public string Id => id;
        public string Title => title;
        public Sprite Sprite => sprite;
    
        public string Description => description;
    
        public int Amount
        {
            get => amount;
            set => amount = Math.Clamp(value, 0, maxAmount);
        }

        public int MaxAmount => maxAmount;

        public bool IsFull => amount >= maxAmount;
    
        public delegate void OnItemAction(Item item);
        public event OnItemAction onPickedUp;
    
        private void OnValidate()
        {
            Amount = amount;
        }
        
        public virtual void PickedUp(Inventory instigator)
        {
            onPickedUp?.Invoke(this);
        }

        #region Equality check
        
        protected bool Equals(Item other)
        {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Item)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), id);
        }

        #endregion
    }
}
