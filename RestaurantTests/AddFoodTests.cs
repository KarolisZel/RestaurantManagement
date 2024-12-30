namespace RestaurantTests;

public class AddFoodTests
{
    private static List<MenuItem> _foodMenu;

    [SetUp]
    public void Setup()
    {
        _foodMenu = new List<MenuItem>
        {
            new() { Name = "Burger", Price = 5.00 },
            new() { Name = "Pizza", Price = 8.00 },
            new() { Name = "Salad", Price = 4.50 }
        };

        AddFoodHelper.SetFoodMenu(_foodMenu);
    }

    [Test]
    public void AddFood_ValidInput_AddsFoodToOrder()
    {
        // Arrange
        var initialOrder = new List<MenuItem>();
        var input = "Burger";
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var updatedOrder = AddFoodHelper.AddFood(initialOrder);

        // Assert
        Assert.That(updatedOrder.Count, Is.EqualTo(1));
        Assert.That(updatedOrder[0].Name, Is.EqualTo("Burger"));
        Assert.That(updatedOrder[0].Price, Is.EqualTo(5.00));
    }

    [Test]
    public void AddFood_InvalidInput_DoesNotAddFood()
    {
        // Arrange
        var initialOrder = new List<MenuItem>();
        var input = "InvalidFood";
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var updatedOrder = AddFoodHelper.AddFood(initialOrder);

        // Assert
        Assert.That(updatedOrder.Count, Is.EqualTo(0));
    }

    [Test]
    public void AddFood_EmptyInput_DoesNotAddFood()
    {
        // Arrange
        var initialOrder = new List<MenuItem>();
        var input = string.Empty;
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var updatedOrder = AddFoodHelper.AddFood(initialOrder);

        // Assert
        Assert.That(updatedOrder.Count, Is.EqualTo(0));
    }
}

//==========================================//
// Helpers
public static class AddFoodHelper
{
    public static List<MenuItem> FoodMenu { get; private set; }

    public static void SetFoodMenu(List<MenuItem> foodMenu)
    {
        FoodMenu = foodMenu;
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
            return order;
        }

        var item = FoodMenu.Find(x => x.Name == input);

        order.Add(item);

        return order;
    }

    private static void PrintHeader()
    {
        Console.WriteLine("=== Add Food ===");
    }
}
