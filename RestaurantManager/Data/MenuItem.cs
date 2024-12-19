namespace RestaurantManager;

public class MenuItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

public class FoodItem : MenuItem { }

public class DrinkItem : MenuItem { }
