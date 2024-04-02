using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tridi
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<Item> items;
        [SerializeField] private int capacity;
    
        public List<Item> Items => items;

        public delegate void OnItemCollected(Item item, int amount);
        public event OnItemCollected onItemCollected;
        
        public bool CanPickup(Item item)
        {
            return items.Count + 1 <= capacity;
        }
    
        public void PickUp(Item item)
        {
            var stored = items.Find(x => x.Equals(item) && !x.IsFull);
        
            if (stored)
            {
                var oldAmount = stored.Amount;
                stored.Amount += item.Amount;
                var delta = stored.Amount - oldAmount;
                item.Amount -= delta;
                onItemCollected?.Invoke(stored, delta);
            }

            if (item.Amount > 0 && CanPickup(item))
            {
                items.Add(Instantiate(item));
                var delta = item.Amount;
                item.Amount = 0;
                onItemCollected?.Invoke(item, delta);
            }

            if (item.Amount == 0)
            {
                item.PickedUp(this);
            }
        }

        public T Get<T>() where T : Item
        {
            return (T)items.First(x => x is T);
        }

        public T Get<T>(string id) where T : Item
        {
            return (T)items.First(x => x is T && x.Id == id);
        }

        public Item Get(string id)
        {
            return items.First(x => x.Id == id);
        }
        
        public IList<T> GetAll<T>() where T : Item
        {
            return items.OfType<T>().ToList();
        }

        public IList<Item> GetAll()
        {
            return GetAll<Item>();
        }
        
        public IList<T> GetAll<T>(string id) where T : Item
        {
            return items.OfType<T>().ToList();
        }
        
        public IList<Item> GetAll(string id)
        {
            return GetAll<Item>(id);
        }
    }
}