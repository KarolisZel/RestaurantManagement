namespace RestaurantManager;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}

class Table
{
    public int TableNumber { get; set; }
    public int SeatCount { get; set; }
    public bool IsAvailable { get; set; }
}

class Waiter
{
    public string Name { get; set; }
    public string Pin { get; set; }
}

class Order
{
    public int Number { get; set; }
    public List<MenuItem> Type { get; set; }
    public Table Table { get; set; }
    public DateTime OrderingTime { get; set; }
}

class MenuItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

class Food : MenuItem { }

class Drink : MenuItem { }

class Receipt
{
    public int Number { get; set; }
    public Order Order { get; set; }
    public DateTime PrintTime { get; set; }
    public Waiter Waiter { get; set; }
}

class RestaurantReceipt : Receipt
{
    public Table Table { get; set; }
}

static class FileManager { }
