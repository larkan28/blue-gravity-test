using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InteractableShop : Interactable
{
    [SerializeField] private Item[] itemsToSell;
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private float minInteractDistance;

    private Inventory m_inventory;

    private void Awake()
    {
        m_inventory = GetComponent<Inventory>();

        foreach (var item in itemsToSell)
            m_inventory.Add(item);
    }

    public override void Interact(Transform actor)
    {
        if (actor == null)
            return;

        float distance = Vector2.Distance(actor.position, transform.position);

        if (distance <= minInteractDistance)
            gameEvent.InventoryToggle(m_inventory);
    }
}
