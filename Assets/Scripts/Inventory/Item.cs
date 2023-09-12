public class Item
{
    public ItemData Data;
    public Slot Slot;
    public int Quantity;

    public Item(ItemData data, Slot slot, int quantity = 1)
    {
        Data = data;
        Slot = slot;
        Quantity = quantity;
    }
}
