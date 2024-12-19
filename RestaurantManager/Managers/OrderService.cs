namespace RestaurantManager;

public class OrderService
{
    private List<Order> Orders = new();
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
        table.IsAvailable = false;
        return order;
    }
}
