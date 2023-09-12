using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxCapacity;
    [SerializeField] private GameEvent gameEvent;

    public int Count => Items.Count;
    public int Capacity => maxCapacity;

    public readonly List<Item> Items = new();

    public void Add(ItemData data, int quantity = 1)
    {
        if (data == null)
            return;

        if (quantity < 1)
            quantity = 1;

        Item existingItem = Items.Find(x => x.Data == data);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
            gameEvent.InventoryChanged(this);
        }
        else if (Count < maxCapacity)
        {
            Items.Add(new Item(data, quantity));
            gameEvent.InventoryChanged(this);
        }
    }

    public void Remove(Item item)
    {
        if (item != null)
        {
            Items.Remove(item);
            gameEvent.InventoryChanged(this);
        }
    }
}
