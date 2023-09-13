using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Inventory inventoryBag;
    [SerializeField] private Inventory inventoryEquip;

    public float Money
    {
        get => m_playerMoney;
        set
        {
            m_playerMoney = value;
            gameEvent.MoneyChanged(m_playerMoney);
        }
    }

    private float m_playerMoney;
    private Inventory m_inventoryShop;
    private Skeleton2D m_skeleton2D;
    private Interactable m_shopInteraction;

    private void OnEnable()
    {
        gameEvent.OnSlotSelected += OnSlotSelected;
        gameEvent.OnInventoryShow += OnInventoryShow;
        gameEvent.OnInventoryChanged += OnInventoryChanged;
    }

    private void OnDisable()
    {
        gameEvent.OnSlotSelected -= OnSlotSelected;
        gameEvent.OnInventoryShow -= OnInventoryShow;
        gameEvent.OnInventoryChanged -= OnInventoryChanged;
    }

    internal void Init()
    {
        m_skeleton2D = GetComponent<Skeleton2D>();
    }

    internal void Think()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();

        if (Input.GetKeyDown(KeyCode.Escape))
            CloseAllInventories();

        if (m_shopInteraction != null && !m_shopInteraction.CanInteract(transform))
            CloseAllInventories();
    }

    private void ToggleInventory()
    {
        gameEvent.InventoryShow(inventoryBag, !inventoryBag.IsOpen);
        gameEvent.InventoryShow(inventoryEquip, !inventoryEquip.IsOpen && m_inventoryShop == null);
    }

    private void CloseAllInventories()
    {
        gameEvent.InventoryShow(inventoryBag, false);
        gameEvent.InventoryShow(inventoryEquip, false);

        if (m_inventoryShop)
            gameEvent.InventoryShow(m_inventoryShop, false);

        m_inventoryShop = null;
    }

    private void OnSlotSelected(Inventory inventory, Slot slot, GameEvent.ItemAction action)
    {
        if (inventory == null || slot == null)
            return;

        Item item = slot.Item;

        switch (action)
        {
            case GameEvent.ItemAction.SelectRight: // Pressed right click
                {
                    switch (inventory.TypeId)
                    {
                        case Inventory.Type.Bag:
                            {
                                if (m_inventoryShop != null) // Sell item
                                    Sell(m_inventoryShop, item);
                                else if (inventoryEquip.IsOpen) // Equip item
                                    Equip(item);

                                break;
                            }
                    }

                    break;
                }
            case GameEvent.ItemAction.SelectLeft: // Pressed left click
                {
                    switch (inventory.TypeId)
                    {
                        case Inventory.Type.Shop:
                            {
                                if (m_inventoryShop != null) // Buy item
                                    Buy(m_inventoryShop, item);

                                break;
                            }
                    }

                    break;
                }
        }
    }

    private void OnInventoryShow(Inventory inventory, bool show)
    {
        if (inventory == null || inventory.TypeId != Inventory.Type.Shop)
            return;

        if (show) // Show bag when shop is oppened
        {
            m_inventoryShop = inventory;
            m_shopInteraction = m_inventoryShop.GetComponent<Interactable>();

            gameEvent.InventoryShow(inventoryBag, true);
            gameEvent.InventoryShow(inventoryEquip, false);
        }
        else // Close everything when shop is closed
        {
            m_inventoryShop = null;
            m_shopInteraction = null;

            gameEvent.InventoryShow(inventoryBag, false);
            gameEvent.InventoryShow(inventoryEquip, false);
        }
    }

    private void OnInventoryChanged(Inventory inventory)
    {
        if (inventory == inventoryEquip)
            RefreshPlayerOutfit();
    }

    private void Buy(Inventory shop, Item itemToBuy)
    {
        if (itemToBuy == null)
            return;

        float price = itemToBuy.Data.Price;

        if (Money < price)
        {
            gameEvent.SendMessage("Not enough money!");
            return;
        }

        Money -= price;
        shop.Remove(itemToBuy, 1);
        inventoryBag.Add(itemToBuy.Data, 1);
    }

    private void Sell(Inventory shop, Item itemToSell)
    {
        if (itemToSell == null)
            return;

        Money += itemToSell.Data.Price;
        shop.Add(itemToSell.Data, 1);
        inventoryBag.Remove(itemToSell, 1);
    }

    private void Equip(Item item)
    {
        if (item.Data is not ItemOutfit itemOutfit)
            return;

        Slot slotEquipment = inventoryEquip.FindSlot(itemOutfit.SlotId);

        if (slotEquipment != null)
            inventoryBag.Move(item.Slot, slotEquipment);
    }

    private void RefreshPlayerOutfit()
    {
        foreach (var slot in inventoryEquip.Slots)
        {
            Item item = slot.Item;

            if (item != null && item.Data is ItemOutfit itemOutfit)
                m_skeleton2D.SetOutfit(itemOutfit.Clothes);
        }
    }
}
