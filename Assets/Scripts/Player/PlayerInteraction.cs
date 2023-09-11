using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask layerInteraction;

    private Camera m_mainCamera;

    internal void Init()
    {
        m_mainCamera = Camera.main;
    }

    internal void Think()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Interact();
    }

    private void Interact()
    {
        Vector2 origin = m_mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, layerInteraction);

        if (hit.collider != null && hit.collider.TryGetComponent(out Interactable interaction))
            interaction.Interact(transform);
    }
}
