using System.Net;
using System.Net.Mail;
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
        return JsonSerializer.Deserialize<List<FoodItem>>(jsonData, JsonSerializerOptions.Default);
    }

    public static List<DrinkItem> LoadDrinkMenu()
    {
        var jsonData = File.ReadAllText(@$"{MenuJsons}\Drink.json");
        return JsonSerializer.Deserialize<List<DrinkItem>>(jsonData, JsonSerializerOptions.Default);
    }

    public static void SaveRestaurantReceipt(Order order)
    {
        Directory.CreateDirectory(RestaurantReceipts);

        var filePath = Path.Combine(RestaurantReceipts, $"Order{order.OrderId}.json");
        File.Create(filePath).Close();

        var receiptData = JsonSerializer.Serialize(
            order,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(filePath, receiptData);
    }

    public static void SendReceiptViaEmail(string recipient, Order order)
    {
        var filePath =
            $@"C:\Users\koral\RiderProjects\Restaurant\RestaurantManager\EmailReceipts\CustomerReceipt";
        var senderEmail = "from@emial.com";
        var senderPassword = "a02aed49ac2fa4";
        var smtpHost = "sandbox.smtp.mailtrap.io";
        var smtpPort = 2525; // Common for TLS

        var objectToSend = new
        {
            Restaurant = "NameHere",
            order.OrderTime,
            order.Items,
            order.Total,
            order.WaiterName,
            ReceiptPrinted = DateTime.Now,
        };

        File.WriteAllText(filePath, JsonSerializer.Serialize(objectToSend));

        try
        {
            using var email = new MailMessage();
            email.From = new MailAddress(senderEmail);
            email.To.Add(recipient);
            email.Subject = $"'NameHere' Restaurant receipt for order {order.OrderId}";
            email.Body =
                "We are sending you your receipt regarding your order at 'NameHere' Restaurant";
            email.IsBodyHtml = false;

            if (File.Exists(filePath))
                email.Attachments.Add(new Attachment(filePath));

            using SmtpClient smtp = new SmtpClient(smtpHost, smtpPort);
            smtp.Credentials = new NetworkCredential("5ed09c7f3f51fb", senderPassword);
            smtp.EnableSsl = true;
            smtp.Send(email);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}
