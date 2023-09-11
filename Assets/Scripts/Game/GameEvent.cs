using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    [SerializeField] private ItemData[] items;

    public ItemData[] Items => items;

    #region EVENTS

    #endregion
}
