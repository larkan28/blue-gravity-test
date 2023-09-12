using UnityEngine;

public class InteractableBank : Interactable
{
    [SerializeField] private float moneyAmount;

    public override void Interact(Transform actor)
    {
        if (!CanInteract(actor))
            return;

        if (actor.TryGetComponent(out PlayerInventory playerInventory))
            playerInventory.Money += moneyAmount;
    }
}
