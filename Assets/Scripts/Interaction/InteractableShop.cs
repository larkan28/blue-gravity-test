using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InteractableShop : Interactable
{
    [SerializeField] private Item[] itemsToSell;
    [SerializeField] private GameEvent gameEvent;

    private Inventory m_inventory;

    private void Awake()
    {
        m_inventory = GetComponent<Inventory>();

        foreach (var item in itemsToSell)
            m_inventory.Add(item);
    }

    public override void Interact(Transform actor)
    {
        if (CanInteract(actor))
            gameEvent.InventoryShow(m_inventory, true);
    }
}
