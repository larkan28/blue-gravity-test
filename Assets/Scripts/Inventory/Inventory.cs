using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum Type
    {
        Bag = 0,
        Equip,
        Shop
    };

    public enum ErrorCode : int
    {
        Ok,
        Full,
        Invalid
    }

    public Slot[] Slots;

    [SerializeField] private Type type;
    [SerializeField] private GameEvent gameEvent;

    public int Count
    {
        get
        {
            int count = 0;

            foreach (Slot slot in Slots)
            {
                if (!slot.IsEmpty)
                    count++;
            }

            return count;
        }
    }
    public int Capacity => Slots.Length;
    public bool IsOpen => InventoryUI != null && InventoryUI.IsOpen;
    public Type TypeId => type;

    [HideInInspector] public UI_Inventory InventoryUI;

    private void Awake()
    {
        foreach (Slot slot in Slots)
            slot.Parent = this;
    }

    public ErrorCode Add(ItemData data, int quantity = 1)
    {
        if (data == null)
            return ErrorCode.Invalid;

        if (quantity < 1)
            quantity = 1;

        if (data.IsStackable)
        {
            Item item = Find(data)?.Item;

            if (item != null)
            {
                item.Quantity += quantity;

                gameEvent.InventoryChanged(this);
                return ErrorCode.Ok;
            }
        }

        Slot emptySlot = FindEmptySlot();

        if (emptySlot == null)
            return ErrorCode.Full;

        emptySlot.Item = new Item(data, emptySlot, quantity);
        gameEvent.InventoryChanged(this);

        return ErrorCode.Ok;
    }

    public void Remove(Item item, int quantity = 1)
    {
        if (item == null)
            return;

        Slot slot = Find(item);

        if (slot == null)
            return;

        slot.Item.Quantity -= quantity;

        if (slot.Item.Quantity < 1)
            slot.Item = null;

        gameEvent.InventoryChanged(this);
    }

    public void Move(Slot fromSlot, Slot toSlot)
    {
        if (fromSlot == null || toSlot == null || fromSlot == toSlot)
            return;

        Inventory toInventory = toSlot.Parent;

        if (toInventory == null)
            return;

        (toSlot.Item, fromSlot.Item) = (fromSlot.Item, toSlot.Item);

        gameEvent.InventoryChanged(this);
        gameEvent.InventoryChanged(toInventory);
    }

    public Slot Find(Item item)
    {
        return (item == null) ? null : Array.Find(Slots, x => x.Item == item);
    }

    public Slot Find(ItemData data)
    {
        return (data == null) ? null : Array.Find(Slots, x => x.Item?.Data == data);
    }

    public Slot FindEmptySlot()
    {
        foreach (var slot in Slots)
        {
            if (slot.IsEmpty)
                return slot;
        }

        return null;
    }

    public Slot FindSlot(Slot.Type type)
    {
        return Array.Find(Slots, x => x.TypeId == type);
    }
}
