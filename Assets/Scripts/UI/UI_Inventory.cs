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
        gameEvent.OnInventoryShow += OnInventoryShow;
    }

    private void OnDisable()
    {
        gameEvent.OnInventoryChanged -= OnInventoryChanged;
        gameEvent.OnInventoryShow -= OnInventoryShow;
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

    private void OnInventoryShow(Inventory inventory, bool show)
    {
        if (type == inventory.TypeId)
        {
            Show(show);
            Draw(inventory);
        }

        Tooltip.Hide();
    }

    public void Show(bool value)
    {
        invetoryRoot.SetActive(value);
    }

    private void Draw(Inventory inventory)
    {
        if (!IsOpen)
            return;

        if (inventory != m_inventory)
            CreateSlots(inventory.Capacity);

        for (int i = 0; i < m_slots.Length; i++)
        {
            Slot slot = inventory.Slots[i];

            if (slot.IsEmpty && !showEmptySlots)
                m_slots[i].Hide();
            else
                m_slots[i].Show(slot);
        }

        m_inventory = inventory;
        m_inventory.InventoryUI = this;
    }

    private void CreateSlots(int maxSlots)
    {
        if (m_slots != null)
            Clear();

        m_slots = new UI_InventorySlot[maxSlots];

        for (int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i] = Instantiate(slotPrefab, itemsContainer);

            if (m_slots[i] != null)
                m_slots[i].Init(this);
        }
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

    public void SelectItem(UI_InventorySlot itemUI, GameEvent.ItemAction action)
    {
        if (itemUI != null)
            gameEvent.SlotSelected(m_inventory, itemUI.Slot, action);

        Tooltip.Hide();
    }
}
