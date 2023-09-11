using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Inventory m_inventory;

    internal void Init()
    {

    }

    internal void Think()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();
    }

    private void ToggleInventory()
    {

    }
}
