using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Inventory inventoryBag;
    [SerializeField] private Inventory inventoryEquip;

    private Inventory m_inventoryShop;
    private Skeleton2D m_skeleton2D;

    private void OnEnable()
    {
        gameEvent.OnItemSelected += OnItemSelected;
        gameEvent.OnInventoryShow += OnInventoryShow;
    }

    private void OnDisable()
    {
        gameEvent.OnItemSelected -= OnItemSelected;
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
    }

    private void ToggleInventory()
    {
        gameEvent.InventoryShow(inventoryBag, !inventoryBag.IsOpen);
        gameEvent.InventoryShow(inventoryEquip, !inventoryEquip.IsOpen);
    }

    private void OnItemSelected(Inventory inventory, Item item, GameEvent.ItemAction action)
    {
        if (inventory == null || item == null)
            return;

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
        shop.Remove(itemToBuy);
        inventoryBag.Add(itemToBuy);
    }

    private void Sell(Inventory shop, Item itemToSell)
    {
        shop.Add(itemToSell);
        inventoryBag.Remove(itemToSell);
    }

    private void Equip(Item item)
    {
        if (item != null && item.Data is ItemOutfit itemOutfit)
            m_skeleton2D.SetOutfit(itemOutfit.Clothes);
    }
}
