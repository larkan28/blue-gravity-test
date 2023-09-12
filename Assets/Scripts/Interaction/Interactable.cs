using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float minDistance;

    public abstract void Interact(Transform actor);

    protected bool CanInteract(Transform actor)
    {
        if (actor == null)
            return false;

        return Vector2.Distance(actor.position, transform.position) < minDistance;
    }
}
