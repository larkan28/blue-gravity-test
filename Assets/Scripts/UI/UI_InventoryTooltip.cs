using TMPro;
using UnityEngine;

public class UI_InventoryTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textDescription;

    private void Awake()
    {
        Hide();
    }

    public void Show(Item item, Vector3 position)
    {
        if (item == null)
            return;

        transform.position = position;

        textTitle.text = item.Data.Name;
        textDescription.text = item.Data.Description;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
