using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ItemOutfit")]
public class ItemOutfit : ItemData
{
    [Header("Outfit")]
    public Bone2D.Type Type;
    public Sprite Sprite;
}
