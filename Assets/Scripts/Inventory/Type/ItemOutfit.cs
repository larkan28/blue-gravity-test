using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ItemOutfit")]
public class ItemOutfit : ItemData
{
    [Header("Outfit")]
    public SkeletonOutfit[] Clothes;
    public Slot.Type SlotId;
}
