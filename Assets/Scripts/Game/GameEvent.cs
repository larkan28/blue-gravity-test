using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    public enum ItemAction
    {
        Equip = 0,
        Remove
    };

    [SerializeField] private ItemData[] items;

    public ItemData[] Items => items;

    #region EVENTS
    public event Action<Inventory> OnInventoryChanged;
    public void InventoryChanged(Inventory inventory) { OnInventoryChanged?.Invoke(inventory); }

    public event Action<Inventory> OnInventoryToggle;
    public void InventoryToggle(Inventory inventory) { OnInventoryToggle?.Invoke(inventory); }

    public event Action<Inventory, Item, ItemAction> OnItemSelected;
    public void ItemSelected(Inventory inventory, Item item, ItemAction action) { OnItemSelected?.Invoke(inventory, item, action); }
    #endregion
}
