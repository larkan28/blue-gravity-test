using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    public enum ItemAction
    {
        SelectLeft = 0,
        SelectRight
    };

    [SerializeField] private ItemData[] items;

    public ItemData[] Items => items;

    #region EVENTS
    public event Action<Inventory> OnInventoryChanged;
    public void InventoryChanged(Inventory inventory) { OnInventoryChanged?.Invoke(inventory); }

    public event Action<Inventory, bool> OnInventoryShow;
    public void InventoryShow(Inventory inventory, bool show) { OnInventoryShow?.Invoke(inventory, show); }

    public event Action<Inventory, Item, ItemAction> OnItemSelected;
    public void ItemSelected(Inventory inventory, Item item, ItemAction action) { OnItemSelected?.Invoke(inventory, item, action); }
    #endregion
}
