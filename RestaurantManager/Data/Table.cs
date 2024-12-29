namespace RestaurantManager;

public class Table(int tableNumber, int seatCount)
{
    public int TableNumber { get; set; } = tableNumber;
    public int SeatCount { get; set; } = seatCount;
    public bool IsAvailable { get; set; } = true;

    public void MakeAvailable()
    {
        if (IsAvailable)
            Console.WriteLine($"Table {TableNumber} is already available!");

        IsAvailable = true;
    }

    public void MakeTaken()
    {
        if (!IsAvailable)
            Console.WriteLine($"Table {TableNumber} is already taken!!");

        IsAvailable = false;
    }
}
