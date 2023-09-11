using UnityEngine;

public class InteractableShop : Interactable
{
    public override void Interact(Transform actor)
    {
        if (actor == null)
            return;

        print("Interact");
    }
}
