using TMPro;
using UnityEngine;

public class UI_ShopSlot : UI_InventorySlot
{
    [SerializeField] private TextMeshProUGUI textPrice;

    public override void Init(UI_Inventory manager)
    {
        base.Init(manager);
    }

    public override void Show(Item item)
    {
        base.Show(item);
        textPrice.text = item?.Data.Price.ToString();
    }
}
