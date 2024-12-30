namespace RestaurantTests;

public class AddDrinksTests
{
    private static List<MenuItem> _drinksMenu;

    [SetUp]
    public void Setup()
    {
        _drinksMenu = new List<MenuItem>
        {
            new() { Name = "Coke", Price = 1.50 },
            new() { Name = "Pepsi", Price = 1.50 },
            new() { Name = "Water", Price = 1.00 }
        };

        AddDrinksHelper.SetDrinksMenu(_drinksMenu);
    }

    [Test]
    public void AddDrinks_ValidInput_AddsDrinkToOrder()
    {
        // Arrange
        var initialOrder = new List<MenuItem>();
        var input = "Coke";
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var updatedOrder = AddDrinksHelper.AddDrinks(initialOrder);

        // Assert
        Assert.That(updatedOrder.Count, Is.EqualTo(1));
        Assert.That(updatedOrder[0].Name, Is.EqualTo("Coke"));
        Assert.That(updatedOrder[0].Price, Is.EqualTo(1.50));
    }

    [Test]
    public void AddDrinks_InvalidInput_DoesNotAddDrink()
    {
        // Arrange
        var initialOrder = new List<MenuItem>();
        var input = "InvalidDrink";
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var updatedOrder = AddDrinksHelper.AddDrinks(initialOrder);

        // Assert
        Assert.That(updatedOrder.Count, Is.EqualTo(0));
    }

    [Test]
    public void AddDrinks_EmptyInput_DoesNotAddDrink()
    {
        // Arrange
        var initialOrder = new List<MenuItem>();
        var input = string.Empty;
        var inputStream = new StringReader(input + Environment.NewLine);
        Console.SetIn(inputStream);

        var outputStream = new StringWriter();
        Console.SetOut(outputStream);

        // Act
        var updatedOrder = AddDrinksHelper.AddDrinks(initialOrder);

        // Assert
        Assert.That(updatedOrder.Count, Is.EqualTo(0));
    }
}

//==========================================//
// Helpers
public static class AddDrinksHelper
{
    private static List<MenuItem> DrinksMenu { get; set; }

    public static void SetDrinksMenu(List<MenuItem> drinksMenu)
    {
        DrinksMenu = drinksMenu;
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
            return order;
        }

        var item = DrinksMenu.Find(x => x.Name == input);

        order.Add(item);

        return order;
    }

    private static void PrintHeader()
    {
        Console.WriteLine("=== Add Drinks ===");
    }
}

public class MenuItem
{
    public string Name { get; set; }
    public double Price { get; set; }
}
