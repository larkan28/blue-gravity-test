using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask layerInteraction;
    [SerializeField] private GameEvent gameEvent;

    private Camera m_mainCamera;
    private Interactable m_lastInteraction;

    internal void Init()
    {
        m_mainCamera = Camera.main;
    }

    internal void Think()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Interact();
    }

    internal void ThinkFixed()
    {
        ShowInteractions();
    }

    private void ShowInteractions()
    {
        Interactable interaction = GetInteraction();

        if (m_lastInteraction != interaction)
            gameEvent.ShowInteraction(interaction);

        m_lastInteraction = interaction;
    }

    private void Interact()
    {
        Interactable interaction = GetInteraction();

        if (interaction != null)
            interaction.Interact(transform);
    }

    private Interactable GetInteraction()
    {
        Vector2 origin = m_mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, layerInteraction);

        if (hit.collider != null)
            return hit.collider.GetComponent<Interactable>();

        return null;
    }
}
