namespace RestaurantManager;

public class TableService
{
    public List<Table> Tables { get; set; } = new();
    public static Dictionary<int, string> TableAvailability { get; set; } = new();

    public Table GetAvailableTable(int seats)
    {
        return Tables.FirstOrDefault(t => t.IsAvailable && t.SeatCount >= seats);
    }

    private void CreateTablesList()
    {
        for (int i = 0; i < 11; i++)
        {
            if (i < 6)
                Tables.Add(new Table(i + 1, 4));

            Tables.Add(new Table(i + 1, 2));
        }
    }

    public void CreateTableAvailability()
    {
        if (Tables.Count == 0)
        {
            Console.WriteLine("Table list empty. Creating Tables...");
            CreateTablesList();
        }

        foreach (var table in Tables)
        {
            TableAvailability.Add(table.TableNumber, table.IsAvailable ? "Available" : "Taken");
        }
    }
}
