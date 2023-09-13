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

    public event Action<Inventory, Slot, ItemAction> OnSlotSelected;
    public void SlotSelected(Inventory inventory, Slot slot, ItemAction action) { OnSlotSelected?.Invoke(inventory, slot, action); }

    public event Action<float> OnMoneyChanged;
    public void MoneyChanged(float money) { OnMoneyChanged?.Invoke(money); }

    public event Action<string> OnSendMessage;
    public void SendMessage(string text) { OnSendMessage?.Invoke(text); }

    public event Action<Interactable> OnShowInteraction;
    public void ShowInteraction(Interactable interaction) { OnShowInteraction?.Invoke(interaction); }
    #endregion
}
