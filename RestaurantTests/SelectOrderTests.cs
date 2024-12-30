namespace RestaurantTests;

public class SelectOrderTests
{
    private static List<Order> _orders;
    private static string _currentUser;

    [SetUp]
    public void Setup()
    {
        _orders = new List<Order>
        {
            new()
            {
                OrderId = 1,
                WaiterName = "John",
                Table = new Table { TableNumber = 10 }
            },
            new()
            {
                OrderId = 2,
                WaiterName = "Jane",
                Table = new Table { TableNumber = 5 }
            },
            new()
            {
                OrderId = 3,
                WaiterName = "John",
                Table = new Table { TableNumber = 12 }
            }
        };

        _currentUser = "John";

        SelectOrderHelper.SetOrders(_orders);
        SelectOrderHelper.SetCurrentUserName(_currentUser);
    }

    [Test]
    public void SelectOrder_ValidOrderId_ReturnsCorrectOrder()
    {
        // Arrange
        var input = "1"; // Valid order ID for the current user
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var selectedOrder = SelectOrderHelper.SelectOrder();

        // Assert
        Assert.IsNotNull(selectedOrder);
        Assert.That(selectedOrder.OrderId, Is.EqualTo(1));
        Assert.That(selectedOrder.WaiterName, Is.EqualTo("John"));
    }

    [Test]
    public void SelectOrder_InvalidOrderId_ReturnsNull()
    {
        // Arrange
        var input = "99"; // Invalid order ID
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var selectedOrder = SelectOrderHelper.SelectOrder();

        // Assert
        Assert.That(selectedOrder, Is.Null);
    }

    [Test]
    public void SelectOrder_NonNumericInput_ShowsErrorAndReturnsNull()
    {
        // Arrange
        var input = "abc"; // Non-numeric input
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var selectedOrder = SelectOrderHelper.SelectOrder();

        // Assert
        Assert.IsNull(selectedOrder);
        StringAssert.Contains(
            "Please enter a number corresponding to the order ID!",
            outputStream.ToString()
        );
    }

    [Test]
    public void SelectOrder_EmptyInput_ShowsErrorAndReturnsNull()
    {
        // Arrange
        var input = ""; // Empty input
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var selectedOrder = SelectOrderHelper.SelectOrder();

        // Assert
        Assert.That(selectedOrder, Is.Null);
        StringAssert.Contains("Bad input. Try again", outputStream.ToString());
    }
}

// Helper class to allow setting Orders and CurrentUser for testing
public static class SelectOrderHelper
{
    public static List<Order> Orders { get; private set; }
    public static string CurrentUserName { get; private set; }

    public static void SetOrders(List<Order> orders)
    {
        Orders = orders;
    }

    public static void SetCurrentUserName(string userName)
    {
        CurrentUserName = userName;
    }

    public static Order SelectOrder()
    {
        PrintHeader();

        var selection = Orders.Where(x => x.WaiterName == CurrentUserName).ToList();

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
        {
            Console.WriteLine("Please enter a number corresponding to the order ID!");
            return null;
        }

        return selection.Find(x => x.OrderId == id);
    }

    private static void PrintHeader()
    {
        Console.WriteLine("=== Select Order ===");
    }
}

//==========================================//
// Helpers
public class Order
{
    public int OrderId { get; set; }
    public string WaiterName { get; set; }
    public Table Table { get; set; }
}

public class Table
{
    public int TableNumber { get; set; }
}
