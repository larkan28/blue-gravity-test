using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private UI_InventorySlot slotPrefab;
    [SerializeField] private UI_InventoryTooltip tooltipPrefab;

    public bool IsOpen => itemsContainer.gameObject.activeSelf;

    private Inventory m_inventory;
    private UI_InventorySlot[] m_itemsUI;

    public static UI_InventoryTooltip Tooltip;

    private void OnEnable()
    {
        gameEvent.OnInventoryChanged += OnInventoryChanged;
        gameEvent.OnInventoryToggle += OnInventoryToggle;
    }

    private void OnDisable()
    {
        gameEvent.OnInventoryChanged -= OnInventoryChanged;
        gameEvent.OnInventoryToggle -= OnInventoryToggle;
    }

    private void Awake()
    {
        Show(false);

        if (Tooltip == null)
            Tooltip = Instantiate(tooltipPrefab, transform.root);
    }

    private void OnInventoryChanged(Inventory inventory)
    {
        Draw(inventory);
    }

    private void OnInventoryToggle(Inventory inventory)
    {
        Show(!IsOpen);
        Draw(inventory);
    }

    private void Show(bool value)
    {
        itemsContainer.gameObject.SetActive(value);
    }

    private void Draw(Inventory inventory)
    {
        if (!IsOpen)
            return;

        if (inventory != m_inventory)
        {
            if (m_itemsUI != null)
                Clean();

            m_itemsUI = new UI_InventorySlot[inventory.Capacity];

            for (int i = 0; i < m_itemsUI.Length; i++)
            {
                m_itemsUI[i] = Instantiate(slotPrefab, itemsContainer);

                if (m_itemsUI[i] != null)
                    m_itemsUI[i].Manager = this;
            }
        }

        for (int i = 0; i < m_itemsUI.Length; i++)
        {
            if (i < inventory.Count)
                m_itemsUI[i].Show(inventory.Items[i]);
            else
                m_itemsUI[i].Show(null);
        }

        m_inventory = inventory;
    }

    private void Clean()
    {
        foreach (var item in m_itemsUI)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        m_itemsUI = null;
    }

    public void SelectItem(UI_InventorySlot itemUI, GameEvent.ItemAction action)
    {
        if (itemUI != null)
            gameEvent.ItemSelected(m_inventory, itemUI.Item, action);

        Tooltip.Hide();
    }
}
