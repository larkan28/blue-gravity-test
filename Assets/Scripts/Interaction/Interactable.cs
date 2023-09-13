using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private string textInteraction;

    public abstract void Interact(Transform actor);

    public bool CanInteract(Transform actor)
    {
        if (actor == null)
            return false;

        return Vector2.Distance(actor.position, transform.position) < minDistance;
    }

    public string GetText()
    {
        return textInteraction;
    }
}
