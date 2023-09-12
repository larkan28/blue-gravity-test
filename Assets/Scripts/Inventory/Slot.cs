[System.Serializable]
public class Slot
{
    public enum Type
    {
        Generic = 0,
        Hair,
        Face,
        Boots,
        Gloves,
        Chest,
        Pelvis
    };

    public Item Item;
    public Type TypeId;

    public bool IsEmpty => (Item == null);
}
