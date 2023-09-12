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

    public Item Item => m_item;

    private UI_Inventory m_manager;
    private Item m_item;

    public virtual void Init(UI_Inventory manager)
    {
        m_manager = manager;
        m_item = null;
    }

    public virtual void Show(Item item)
    {
        imageIcon.sprite = item?.Data.Icon;
        m_item = item;

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
        if (m_item != null && (m_item.Quantity > 1 || alwaysShowQuantity))
        {
            textQuantity.text = m_item.Quantity.ToString();
            quantityRoot.SetActive(true);
        }
        else
            quantityRoot.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_Inventory.Tooltip.Show(m_item, transform.position);
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
