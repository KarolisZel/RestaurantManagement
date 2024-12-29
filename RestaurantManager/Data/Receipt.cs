namespace RestaurantManager;

public class Receipt
{
    public int Number { get; set; }
    public Order Order { get; set; }
    public DateTime PrintTime { get; set; }
    public string Waiter { get; set; }
}
