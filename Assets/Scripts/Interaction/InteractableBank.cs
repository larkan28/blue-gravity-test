using UnityEngine;

public class InteractableBank : Interactable
{
    [SerializeField] private float moneyAmount;
    [SerializeField] private GameEvent gameEvent;

    public override void Interact(Transform actor)
    {
        if (!CanInteract(actor))
            return;

        if (actor.TryGetComponent(out PlayerInventory playerInventory))
        {
            playerInventory.Money += moneyAmount;
            gameEvent.SendMessage("You got +$" + Mathf.RoundToInt(moneyAmount));
        }
    }
}
