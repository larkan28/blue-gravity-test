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
        Pelvis,
        Weapon
    };

    public bool IsEmpty => (Item == null);
    public Item Item
    {
        get => m_item;
        set
        {
            m_item = value;

            if (m_item != null)
                m_item.Slot = this;
        }
    }
    public Type TypeId;
    public Inventory Parent;

    private Item m_item;
}
