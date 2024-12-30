namespace RestaurantManager;

static class WaiterService
{
    public static List<Waiter> WaiterList { get; set; } = new();

    private static Waiter CreateWaiter()
    {
        string name;
        do
        {
            Console.WriteLine("Enter waiter Name:");
            name = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(name));

        string pin;
        do
        {
            Console.WriteLine("Enter waiter Pin:");
            pin = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(pin));

        return new Waiter(name, pin);
    }

    public static void AddWaiter()
    {
        var waiter = CreateWaiter();

        WaiterList.Add(waiter);
    }

    // public static List<Waiter> GetWaiterList()
    // {
    //     return WaiterList;
    // }

    public static Waiter Login()
    {
        if (WaiterList.Count == 0)
        {
            Console.WriteLine("No waiters in the system. Please add a waiter first.");
            AddWaiter();
        }

        Console.WriteLine("Enter name:");
        var name = Console.ReadLine();

        while (!WaiterList.Exists(x => x.Name == name))
        {
            Console.WriteLine($"Waiter with name {name} does not exist. Try again!");
            Console.WriteLine("Enter name:");
            name = Console.ReadLine();
        }

        var waiter = WaiterList.Find(x => x.Name == name);
        Console.WriteLine("Enter Pin:");
        var pin = Console.ReadLine();

        while (waiter.Pin != pin)
        {
            Console.WriteLine("Wrong pin! Try again...");
            Console.WriteLine("Enter Pin:");
            pin = Console.ReadLine();
        }

        return waiter;
    }

    public static string GenerateCustomerReceipt(Order order)
    {
        return $"Order ID: {order.OrderId}\n"
            + $"Waiter: {order.WaiterName}\n"
            + $"Items:\n{string.Join("\n", order.Items.Select(i => $"{i.Name} - {i.Price:C}"))}\n"
            + $"Total: {order.Total:C}\n";
    }
}
