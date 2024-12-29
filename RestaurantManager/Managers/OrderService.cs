namespace RestaurantManager;

public class OrderService
{
    public List<Order> Orders = new();
    private int NextOrderId = 1;

    public Order PlaceOrder(Table table, List<MenuItem> items, string waiterName)
    {
        var order = new Order
        {
            OrderId = NextOrderId++,
            Table = table,
            Items = items,
            OrderTime = DateTime.Now,
            WaiterName = waiterName
        };
        Orders.Add(order);
        // table.IsAvailable = false;
        return order;
    }

    public Order RemoveItem(Order order)
    {
        Console.WriteLine("Order Items:");
        Console.WriteLine($"\t{string.Join(",\n\t", order.Items)}");
        Console.WriteLine();

        Console.WriteLine("Specify, what needs removing");
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Try again:");
            input = Console.ReadLine();
        }
        var remove = order.Items.Find(x => x.Name == input);

        if (remove is not null)
        {
            order.Items.Remove(remove);
            return order;
        }

        Console.WriteLine("Could not find specified Item...");
        return order;
    }

    // public Order AddItem(Order order, MenuItem item)
    // {
    //     order.Items.Add(item);
    //     return order;
    // }
}
