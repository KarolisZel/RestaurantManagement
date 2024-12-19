namespace RestaurantManager;

public class Waiter(string name, string pin)
{
    public string Name { get; set; } = name;
    public string Pin { get; set; } = pin;
    public List<Receipt> Receipts { get; set; }
}