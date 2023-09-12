using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private ItemData[] items;

    private Inventory m_inventory;
    private Skeleton2D m_skeleton2D;

    private void OnEnable()
    {
        gameEvent.OnItemSelected += OnItemSelected;
    }

    private void OnDisable()
    {
        gameEvent.OnItemSelected -= OnItemSelected;
    }

    internal void Init()
    {
        m_inventory = GetComponent<Inventory>();
        m_skeleton2D = GetComponent<Skeleton2D>();
    }

    internal void Think()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();

        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (var item in items)
                m_inventory.Add(item);
        }
    }

    private void ToggleInventory()
    {
        gameEvent.InventoryToggle(m_inventory);
    }

    private void OnItemSelected(Inventory inventory, Item item, GameEvent.ItemAction action)
    {
        switch (action)
        {
            case GameEvent.ItemAction.Remove:
                inventory.Remove(item);
                break;
            case GameEvent.ItemAction.Equip:
                EquipOutfit(item);
                break;
        }
    }

    private void EquipOutfit(Item item)
    {
        if (item != null && item.Data is ItemOutfit itemOutfit)
            m_skeleton2D.SetOutfit(itemOutfit.Type, itemOutfit.Sprite);
    }
}
