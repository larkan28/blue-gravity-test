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

    private void OnEnable()
    {
        gameEvent.OnSlotSelected += OnSlotSelected;
        gameEvent.OnInventoryShow += OnInventoryShow;
    }

    private void OnDisable()
    {
        gameEvent.OnSlotSelected -= OnSlotSelected;
        gameEvent.OnInventoryShow -= OnInventoryShow;
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
    }

    private void OnSlotSelected(Inventory inventory, Slot slot, GameEvent.ItemAction action)
    {
        if (inventory == null || slot == null)
            return;

        Item item = slot.Item;

        switch (action)
        {
            case GameEvent.ItemAction.SelectRight:
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
            case GameEvent.ItemAction.SelectLeft:
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

        if (show)
        {
            m_inventoryShop = inventory;

            gameEvent.InventoryShow(inventoryBag, true);
            gameEvent.InventoryShow(inventoryEquip, false);
        }
        else
        {
            m_inventoryShop = null;

            gameEvent.InventoryShow(inventoryBag, false);
            gameEvent.InventoryShow(inventoryEquip, false);
        }
    }

    private void Buy(Inventory shop, Item itemToBuy)
    {
        if (itemToBuy == null)
            return;

        float price = itemToBuy.Data.Price;

        if (Money < price)
            return;

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
}
