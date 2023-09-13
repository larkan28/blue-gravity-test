using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InteractableShop : Interactable
{
    [SerializeField] private ItemTemplate[] itemsToSell;
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private AudioClip soundInteract;

    private Inventory m_inventory;

    private void Awake()
    {
        m_inventory = GetComponent<Inventory>();

        foreach (var item in itemsToSell)
            m_inventory.Add(item.Data, item.Quantity);
    }

    public override void Interact(Transform actor)
    {
        if (CanInteract(actor))
        {
            gameEvent.InventoryShow(m_inventory, true);
            GameSound.Instance.Play(soundInteract);
        }
    }
}

[System.Serializable]
public struct ItemTemplate
{
    public ItemData Data;
    public int Quantity;
}
