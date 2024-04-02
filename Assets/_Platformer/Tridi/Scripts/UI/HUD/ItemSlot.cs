using System.Linq;
using TMPro;
using Tridi;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private Inventory inventory;
    [Space]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI textMesh;

    private void OnValidate()
    {
        if (icon && item) icon.sprite = item.Sprite;
    }

    private void Start()
    {
        icon.enabled = false;
        textMesh.enabled = false;

        try
        {
            OnUpdate(inventory.Get(item.Id), 0);
        }
        catch
        {
            // ignored
        }
    }

    private void OnEnable()
    {
        inventory.onItemCollected += OnUpdate;
    }

    private void OnDisable()
    {
        inventory.onItemCollected -= OnUpdate;
    }

    private void OnUpdate(Item collectedItem, int amount)
    {
        if (!collectedItem.Equals(item)) return;
        
        icon.enabled = true;
        textMesh.enabled = true;
            
        var total = inventory.GetAll(item.Id).Sum(x => x.Amount);
        
        textMesh.text = $"x{total}";
    }
}