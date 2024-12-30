namespace RestaurantManager;

class Program
{
    public static readonly OrderService OrderService = new();
    public static List<Order> Orders { get; set; } = new();
    public static List<FoodItem> FoodMenu { get; set; } = new();
    public static List<DrinkItem> DrinksMenu { get; set; } = new();
    public static Dictionary<int, string> TableAvailability { get; set; } = new();

    public static Waiter? CurrentUser { get; set; }
    public static Table? CurrentTable { get; set; }

    public static void Main(string[] args)
    {
        CurrentUser = null;
        var isUsing = true;
        bool confirm;

        FoodMenu = FileManager.LoadFoodMenu();
        DrinksMenu = FileManager.LoadDrinkMenu();

        while (isUsing)
        {
            TableAvailability = TableService.CreateTableAvailability();

            PrintHeader();
            if (CurrentUser is null)
            {
                Console.WriteLine("Please login to continue!\n");
                CurrentUser = WaiterService.Login();
            }

            while (CurrentUser is not null)
            {
                TableAvailability = TableService.CreateTableAvailability();
                PrintStartMenu();
                var sel = Console.ReadKey(true);

                switch (sel.Key)
                {
                    case ConsoleKey.D1:
                        // Select a table
                        CurrentTable = SelectTable();
                        break;

                    case ConsoleKey.D2:
                        // Start order
                        // if (CurrentTable is not null)
                        StartOrder();
                        break;

                    case ConsoleKey.D3:
                        // Modify order
                        // if (CurrentTable is not null)
                        ModifyOrder(SelectOrder());
                        break;

                    case ConsoleKey.D4:
                        // Remove table from selection
                        // if (CurrentTable is not null)
                        Console.WriteLine("Are you sure? Y/N");
                        confirm = GetConfirmation();

                        if (confirm)
                        {
                            CurrentTable = null;
                            Console.Clear();
                        }
                        break;

                    case ConsoleKey.D5:
                        // Print receipt
                        PrintOrderReceipt(SelectOrder());
                        break;

                    case ConsoleKey.D9:
                        Console.WriteLine("Are you sure? Y/N");
                        confirm = GetConfirmation();

                        if (confirm)
                        {
                            Logout();
                            Console.Clear();
                        }
                        break;

                    case ConsoleKey.Q:
                        Console.WriteLine("Are you sure? Y/N");
                        confirm = GetConfirmation();

                        if (confirm)
                        {
                            Logout();
                            Console.Clear();
                            isUsing = false;
                        }
                        break;

                    default:
                        // Default should never happen
                        Console.WriteLine("This MUST never happen!");
                        break;
                }
            }
        }
    }

    public static void PrintOrderReceipt(Order order)
    {
        PrintHeader();

        Console.WriteLine("Will the customer need a receipt?");

        var confirm = GetConfirmation();

        if (confirm)
        {
            PrintHeader();
            Console.WriteLine("Customer receipt:\n");

            Console.WriteLine($"\tOrdered at: {order.OrderTime}\n");
            Console.WriteLine($"\tOrder contains:");
            foreach (var item in order.Items)
                Console.WriteLine($"\t\t{item.Name}: \t${item.Price}");
            Console.WriteLine($"\t\tTotal: ${order.Total}\n");
            Console.WriteLine($"\tServed by {order.WaiterName}");
            Console.WriteLine($"\tOrder finished at: {DateTime.Now}\n");

            Console.WriteLine("Have a nice day!\n\n");
        }
        else
        {
            Console.WriteLine("Customer refused the receipt. Sending via email...");
            FileManager.SendReceiptViaEmail("to@email.com", order);
        }

        FileManager.SaveRestaurantReceipt(order);
        TableService.Tables.First(x => x.TableNumber == order.Table.TableNumber).IsAvailable = true;
        CurrentTable = null;
        Orders.RemoveAll(x => x.OrderId == order.OrderId);

        Console.WriteLine("Press Q to go back.");
        GoBack();
    }

    public static void StartOrder()
    {
        var isAdding = true;
        var order = new List<MenuItem>();

        while (isAdding)
        {
            PrintCurrentOrder(order);

            Console.WriteLine("1. Add Drinks");
            Console.WriteLine("2. Add Food");
            Console.WriteLine("\n");
            Console.WriteLine("Q. Save & Go back");
            var sel = Console.ReadKey(true);

            switch (sel.Key)
            {
                case ConsoleKey.D1:
                    order = AddDrinks(order);
                    break;

                case ConsoleKey.D2:
                    order = AddFood(order);
                    break;

                case ConsoleKey.Q:
                    Console.WriteLine("Are you sure? Y/N");
                    var confirm = GetConfirmation();

                    if (confirm)
                    {
                        CurrentTable.IsAvailable = false;
                        isAdding = false;
                        Console.Clear();
                    }
                    break;

                default:
                    // Default should never happen
                    Console.WriteLine("This MUST never happen!");
                    break;
            }
        }

        Orders.Add(OrderService.PlaceOrder(CurrentTable, order, CurrentUser.Name));
    }

    public static Order SelectOrder()
    {
        PrintHeader();

        var selection = Orders.Where(x => x.WaiterName == CurrentUser.Name).ToList();

        Console.WriteLine("Available orders:");
        for (int i = 0; i < selection.Count; i++)
        {
            Console.WriteLine($"\tOrder ID: {selection[i].OrderId}.");
            Console.WriteLine($"\tTable Number: {selection[i].Table.TableNumber}.\n");
        }

        Console.WriteLine("Select order (ID):\n\n");
        var sel = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(sel))
        {
            Console.WriteLine("Bad input. Try again");
            sel = Console.ReadLine();
        }

        if (!int.TryParse(sel, out var id))
            Console.WriteLine("Please ender a number corresponding the order ID!");

        return selection.Find(x => x.OrderId == id);
    }

    public static void ModifyOrder(Order order)
    {
        var isModify = true;

        while (isModify)
        {
            PrintCurrentOrder(order.Items);

            Console.WriteLine("1. Remove Item");
            Console.WriteLine("2. Add Drinks");
            Console.WriteLine("3. Add Food");
            Console.WriteLine("\n");
            Console.WriteLine("Q. Save & Go back");

            var sel = Console.ReadKey(true);

            switch (sel.Key)
            {
                case ConsoleKey.D1:
                    // Remove Item
                    order = OrderService.RemoveItem(order);
                    break;

                case ConsoleKey.D2:
                    // Add Drinks
                    order.Items = AddDrinks(order.Items);
                    break;

                case ConsoleKey.D3:
                    // Add Food
                    order.Items = AddFood(order.Items);
                    break;

                case ConsoleKey.Q:
                    Console.WriteLine("Are you sure? Y/N");
                    var confirm = GetConfirmation();

                    if (confirm)
                    {
                        isModify = false;
                        Console.Clear();
                    }
                    break;

                default:
                    // Default should never happen
                    Console.WriteLine("This MUST never happen!");
                    break;
            }
        }

        OrderService.Orders.RemoveAll(x => x.OrderId == order.OrderId);
        OrderService.Orders.Add(order);
    }

    public static void PrintCurrentOrder(List<MenuItem> order)
    {
        PrintHeader();

        if (order.Count < 1)
            Console.WriteLine("Nothing ordered yet");
        else
        {
            Console.WriteLine("Current order:");
            foreach (var item in order)
                Console.WriteLine($"\t{item.Name}\t${item.Price}");
        }
    }

    public static List<MenuItem> AddDrinks(List<MenuItem> order)
    {
        PrintHeader();

        Console.WriteLine("Drinks Menu:");
        foreach (var drinkItem in DrinksMenu)
            Console.WriteLine($"\t{drinkItem.Name}\t${drinkItem.Price}");

        Console.WriteLine();

        Console.WriteLine("Specify item:");
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || !DrinksMenu.Any(x => x.Name == input))
        {
            Console.WriteLine("Bad input. Try again...");
            input = Console.ReadLine();
        }

        var item = DrinksMenu.Find(x => x.Name == input);

        order.Add(item);

        return order;
    }

    public static List<MenuItem> AddFood(List<MenuItem> order)
    {
        PrintHeader();

        Console.WriteLine("Food Menu:");
        foreach (var foodItem in FoodMenu)
            Console.WriteLine($"\t{foodItem.Name}\t${foodItem.Price}");
        Console.WriteLine("\n");

        Console.WriteLine("Specify item:");
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || !FoodMenu.Any(x => x.Name == input))
        {
            Console.WriteLine("Bad input. Try again...");
            input = Console.ReadLine();
        }

        var item = FoodMenu.Find(x => x.Name == input);

        order.Add(item);

        return order;
    }

    public static void PrintHeader()
    {
        Console.Clear();
        Console.WriteLine("'NameHere' Restaurant\n");

        if (CurrentUser is not null)
            Console.WriteLine($"Hello, {CurrentUser.Name}");

        if (CurrentTable is not null)
            Console.WriteLine($"Selected table: {CurrentTable.TableNumber}");

        Console.WriteLine("\n");
    }

    public static void PrintStartMenu()
    {
        PrintHeader();
        Console.WriteLine("Please select your next step...\n");
        Console.WriteLine("1. Select a table");
        if (CurrentTable is not null)
        {
            Console.WriteLine("2. Start an order");
            Console.WriteLine("3. Modify order");
            Console.WriteLine("4. Remove table form selection");
            Console.WriteLine("5. Print receipt");
        }
        Console.WriteLine("\n9. Logout");
        Console.WriteLine("\n\n");
        Console.WriteLine("Q. Quit");
    }

    public static bool GetConfirmation()
    {
        // Confirmation logic
        var result = false;

        var sel = Console.ReadKey(true);

        while (sel.Key is not (ConsoleKey.Y or ConsoleKey.N))
            sel = Console.ReadKey(true);

        result = sel.Key switch
        {
            ConsoleKey.Y => true,
            ConsoleKey.N => false,
            _ => result
        };

        return result;
    }

    public static void Logout()
    {
        if (CurrentUser is null)
            Console.WriteLine("Please login first!");
        else
        {
            Console.WriteLine($"{CurrentUser.Name} has just logged out!");
            CurrentUser = null;
        }
    }

    public static Table SelectTable()
    {
        GetAvailableTables();
        Console.WriteLine("\nEnter table number:");
        var parse = int.TryParse(Console.ReadLine(), out var tableNumber);
        while (!parse)
        {
            Console.WriteLine("Please enter a number!");
            parse = int.TryParse(Console.ReadLine(), out tableNumber);
        }

        return TableService.GetTable(tableNumber);
    }

    public static void GetAvailableTables()
    {
        PrintHeader();
        Console.WriteLine("\n");
        foreach (var table in TableAvailability)
        {
            Console.WriteLine(
                $"\tTable {table.Key} is {table.Value}. Seat count: {TableService.GetTableSeatCount(table.Key)} seats"
            );
        }
    }

    public static void GoBack()
    {
        // Wait for correct key press
        var k = Console.ReadKey(true);
        while (k.Key != ConsoleKey.Q)
        {
            k = Console.ReadKey(true);
        }
    }
}
