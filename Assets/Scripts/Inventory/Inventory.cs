using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum Type
    {
        Bag = 0,
        Equip,
        Shop
    };

    [SerializeField] private Type type;
    [SerializeField] private int maxCapacity;
    [SerializeField] private GameEvent gameEvent;

    public int Count => Items.Count;
    public int Capacity => maxCapacity;
    public Type TypeId => type;

    public readonly List<Item> Items = new();

    public void Add(Item item)
    {
        if (item != null)
            Add(item.Data, item.Quantity);
    }

    public void Add(ItemData data, int quantity = 1)
    {
        if (data == null)
            return;

        if (quantity < 1)
            quantity = 1;

        if (data.IsStackable)
        {
            Item existingItem = Items.Find(x => x.Data == data);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;

                gameEvent.InventoryChanged(this);
                return;
            }
        }

        if (Count >= maxCapacity)
            return;

        Items.Add(new Item(data, quantity));
        gameEvent.InventoryChanged(this);
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
