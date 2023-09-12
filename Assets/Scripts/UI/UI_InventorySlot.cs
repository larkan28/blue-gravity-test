using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private GameObject quantityRoot;
    [SerializeField] private TextMeshProUGUI textQuantity;
    [SerializeField] private bool alwaysShowQuantity;

    public Slot Slot => m_slot;

    private UI_Inventory m_manager;
    private Slot m_slot;

    public virtual void Init(UI_Inventory manager)
    {
        m_manager = manager;
        m_slot = null;
    }

    public virtual void Show(Slot slot)
    {
        imageIcon.sprite = slot.Item?.Data.Icon;
        m_slot = slot;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        ShowQuantity();
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    private void ShowQuantity()
    {
        Item item = m_slot.Item;

        if (item != null && (item.Quantity > 1 || alwaysShowQuantity))
        {
            textQuantity.text = item.Quantity.ToString();
            quantityRoot.SetActive(true);
        }
        else
            quantityRoot.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_Inventory.Tooltip.Show(m_slot.Item, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Inventory.Tooltip.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                m_manager.SelectItem(this, GameEvent.ItemAction.SelectLeft);
                break;
            case PointerEventData.InputButton.Right:
                m_manager.SelectItem(this, GameEvent.ItemAction.SelectRight);
                break;
        }
    }
}
