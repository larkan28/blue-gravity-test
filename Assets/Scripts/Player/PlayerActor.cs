using UnityEngine;

public class PlayerActor : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInventory playerInventory;

    private void Awake()
    {
        playerInteraction.Init();
        playerController.Init();
        playerInventory.Init();
    }

    private void Update()
    {
        playerInteraction.Think();
        playerController.Think();
        playerInventory.Think();
    }

    private void FixedUpdate()
    {
        playerInteraction.ThinkFixed();
        playerController.ThinkFixed();
    }
}
