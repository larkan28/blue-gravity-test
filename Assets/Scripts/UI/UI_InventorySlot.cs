using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private GameObject quantityRoot;
    [SerializeField] private TextMeshProUGUI textQuantity;

    [HideInInspector] public UI_Inventory Manager;
    [HideInInspector] public Item Item;

    public void Show(Item item)
    {
        imageIcon.sprite = item?.Data.Icon;
        Item = item;

        ShowQuantity();
    }

    private void ShowQuantity()
    {
        if (Item != null && Item.Quantity > 1)
        {
            textQuantity.text = Item.Quantity.ToString();
            quantityRoot.SetActive(true);
        }
        else
            quantityRoot.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_Inventory.Tooltip.Show(Item, transform.position);
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
                Manager.SelectItem(this, GameEvent.ItemAction.Equip);
                break;
            case PointerEventData.InputButton.Right:
                Manager.SelectItem(this, GameEvent.ItemAction.Remove);
                break;
        }
    }
}
