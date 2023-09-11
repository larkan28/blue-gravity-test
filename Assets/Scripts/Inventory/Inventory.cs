using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public readonly List<Item> Items = new();

    public void Add(ItemData data, int quantity = 1)
    {
        if (data == null)
            return;

        Item existingItem = Items.Find(x => x.Data == data);

        if (existingItem != null)
            existingItem.Quantity += quantity;
        else
            Items.Add(new Item(data, quantity));
    }

    public void Remove(Item item)
    {
        if (item != null)
            Items.Remove(item);
    }
}
