namespace RestaurantManager;

public class Order
{
    public int OrderId { get; set; }
    public Table Table { get; set; }
    public List<MenuItem> Items { get; set; } = new();
    public DateTime OrderTime { get; set; }
    public string WaiterName { get; set; }
    public decimal Total => Items.Sum(item => item.Price);
}