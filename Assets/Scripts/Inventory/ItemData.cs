using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    [Header("Data")]
    public string Name;
    public string Description;
    public Sprite Icon;
    public float Price;
    public bool IsStackable;
}
