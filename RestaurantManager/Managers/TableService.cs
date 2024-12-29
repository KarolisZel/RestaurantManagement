namespace RestaurantManager;

public static class TableService
{
    public static List<Table> Tables { get; set; } = new();
    public static Dictionary<int, string> TableAvailability { get; set; } = new();

    public static Table GetAvailableTable(int seats)
    {
        try
        {
            var table = Tables.FirstOrDefault(t => t.IsAvailable && t.SeatCount >= seats);

            return table;
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine($"Table is null {e.Message}");
        }

        return null;
    }

    public static Table GetTable(int tableNumber)
    {
        return Tables.FirstOrDefault(x => x.TableNumber == tableNumber);
    }

    private static void CreateTablesList()
    {
        for (int i = 0; i < 11; i++)
        {
            Tables.Add(i < 6 ? new Table(i + 1, 4) : new Table(i + 1, 2));
        }
    }

    public static Dictionary<int, string> CreateTableAvailability()
    {
        if (Tables.Count == 0)
        {
            Console.WriteLine("Table list empty. Creating Tables...");
            CreateTablesList();

            foreach (var table in Tables)
                TableAvailability.Add(table.TableNumber, table.IsAvailable ? "Available" : "Taken");
        }
        else
        {
            foreach (var table in Tables)
                TableAvailability[table.TableNumber] = table.IsAvailable ? "Available" : "Taken";
        }
        return TableAvailability;
    }

    public static int GetTableSeatCount(int tableNumber)
    {
        return Tables.FirstOrDefault(x => x.TableNumber == tableNumber).SeatCount;
    }
}
