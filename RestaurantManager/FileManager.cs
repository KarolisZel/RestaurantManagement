using System.Text.Json;

namespace RestaurantManager;

static class FileManager
{
    private const string MenuJsons =
        @"C:\Users\koral\RiderProjects\Restaurant\RestaurantManager\Jsons";

    private const string RestaurantReceipts =
        @"C:\Users\koral\RiderProjects\Restaurant\RestaurantManager\RestaurantReceipts";

    public static List<FoodItem> LoadFoodMenu()
    {
        var jsonData = File.ReadAllText(@$"{MenuJsons}\Food.json");
        return JsonSerializer.Deserialize<List<FoodItem>>(jsonData);
    }

    public static List<DrinkItem> LoadDrinkMenu()
    {
        var jsonData = File.ReadAllText(@$"{MenuJsons}\Food.json");
        return JsonSerializer.Deserialize<List<DrinkItem>>(jsonData);
    }

    public static void SaveRestaurantReceipt(Order order)
    {
        Directory.CreateDirectory(RestaurantReceipts);

        var filePath = Path.Combine(RestaurantReceipts, $"{order.OrderId}.json");
        File.Create(filePath).Close();

        var receiptData = JsonSerializer.Serialize(
            order,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(filePath, receiptData);
    }
}
