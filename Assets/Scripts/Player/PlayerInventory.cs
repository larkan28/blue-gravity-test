using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Inventory inventoryBag;
    [SerializeField] private Inventory inventoryEquip;
    [SerializeField] private AudioClip soundBuy;
    [SerializeField] private AudioClip soundEquip;
    [SerializeField] private AudioClip soundError;
    [SerializeField] private AudioClip soundOpenInventory;

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
    private GameSound m_gameSound;
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
        m_gameSound = GameSound.Instance;
        m_skeleton2D = GetComponent<Skeleton2D>();
    }

    internal void Think()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();

        if (Input.GetKeyDown(KeyCode.Escape))
            CloseAllInventories();

        if (m_shopInteraction != null && !m_shopInteraction.CanInteract(transform))
            CloseAllInventories(true);
    }

    private void ToggleInventory()
    {
        gameEvent.InventoryShow(inventoryBag, !inventoryBag.IsOpen);
        gameEvent.InventoryShow(inventoryEquip, !inventoryEquip.IsOpen && m_inventoryShop == null);

        m_gameSound.Play(soundOpenInventory);
    }

    private void CloseAllInventories(bool forced = false)
    {
        if (!forced && (inventoryBag.IsOpen || inventoryEquip.IsOpen))
            m_gameSound.Play(soundOpenInventory);

        gameEvent.InventoryShow(inventoryBag, false);
        gameEvent.InventoryShow(inventoryEquip, false);

        if (m_inventoryShop != null)
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
        if (shop == null || !shop.IsOpen || itemToBuy == null)
            return;

        float price = itemToBuy.Data.Price;

        if (Money < price)
        {
            ErrorMessage("Not enough money!");
            return;
        }

        Inventory.ErrorCode result = inventoryBag.Add(itemToBuy.Data, 1);

        if (result == Inventory.ErrorCode.Full)
        {
            ErrorMessage("Your inventory is full");
            return;
        }

        Money -= price;
        shop.Remove(itemToBuy, 1);
        gameEvent.SendMessage($"You bought '{itemToBuy.Data.Name}' (-${price})");
        m_gameSound.Play(soundBuy);
    }

    private void ErrorMessage(string error)
    {
        gameEvent.SendMessage(error);
        m_gameSound.Play(soundError);
    }

    private void Sell(Inventory shop, Item itemToSell)
    {
        if (shop == null || !shop.IsOpen || itemToSell == null)
            return;

        Inventory.ErrorCode result = shop.Add(itemToSell.Data, 1);

        if (result == Inventory.ErrorCode.Full)
        {
            ErrorMessage("Shopkeeper inventory is full");
            return;
        }

        float price = itemToSell.Data.Price;
        Money += price;

        inventoryBag.Remove(itemToSell, 1);
        gameEvent.SendMessage($"You have sold '{itemToSell.Data.Name}' (+${price})");
        m_gameSound.Play(soundBuy);
    }

    private void Equip(Item item)
    {
        if (!inventoryEquip.IsOpen || item == null || item.Data is not ItemOutfit itemOutfit)
            return;

        Slot slotEquipment = inventoryEquip.FindSlot(itemOutfit.SlotId);

        if (slotEquipment != null)
        {
            inventoryBag.Move(item.Slot, slotEquipment);
            m_gameSound.Play(soundEquip);
        }
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
