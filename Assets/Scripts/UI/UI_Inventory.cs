using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private Inventory.Type type;
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject invetoryRoot;
    [SerializeField] private UI_InventorySlot slotPrefab;
    [SerializeField] private UI_InventoryTooltip tooltipPrefab;
    [SerializeField] private bool showEmptySlots;

    public bool IsOpen => invetoryRoot.activeSelf;

    private Inventory m_inventory;
    private UI_InventorySlot[] m_slots;

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
        if (type == inventory.TypeId)
            Draw(inventory);
    }

    private void OnInventoryToggle(Inventory inventory)
    {
        if (type == inventory.TypeId)
        {
            Show(!IsOpen);
            Draw(inventory);
        }
    }

    private void Show(bool value)
    {
        invetoryRoot.SetActive(value);
    }

    private void Draw(Inventory inventory)
    {
        if (!IsOpen)
            return;

        if (HasToRecreateSlots(inventory))
        {
            if (m_slots != null)
                Clear();

            int maxSize = showEmptySlots ? inventory.Capacity : inventory.Count;
            m_slots = new UI_InventorySlot[maxSize];

            for (int i = 0; i < m_slots.Length; i++)
            {
                m_slots[i] = Instantiate(slotPrefab, itemsContainer);

                if (m_slots[i] != null)
                    m_slots[i].Init(this);
            }
        }

        for (int i = 0; i < m_slots.Length; i++)
        {
            if (i < inventory.Count)
                m_slots[i].Show(inventory.Items[i]);
            else
                m_slots[i].Show(null);
        }

        m_inventory = inventory;
    }

    private void Clear()
    {
        foreach (var slot in m_slots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }

        m_slots = null;
    }

    private bool HasToRecreateSlots(Inventory inventory)
    {
        if (!showEmptySlots && m_slots != null && m_slots.Length != inventory.Count)
            return true;

        return inventory != m_inventory || m_slots == null;
    }

    public void SelectItem(UI_InventorySlot itemUI, GameEvent.ItemAction action)
    {
        if (itemUI != null)
            gameEvent.ItemSelected(m_inventory, itemUI.Item, action);

        Tooltip.Hide();
    }
}
