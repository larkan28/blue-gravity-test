public class Item
{
    public ItemData Data;
    public int Quantity;

    public Item(ItemData data, int quantity = 1)
    {
        Data = data;
        Quantity = quantity;
    }
}
