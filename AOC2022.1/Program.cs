await Assignment1();
Console.WriteLine();
await Assignment2();

static async Task Assignment2()
{
    var elfInventories = await GetElfInventories();

    var orderByCarryingMost = elfInventories.OrderByDescending(x => x.Sum()).ToList();
    var top3Total = orderByCarryingMost.Take(3).Sum(x => x.Sum());

    Console.WriteLine("Top 3 total: {0}.", top3Total);
}

static async Task Assignment1()
{
    var elfInventories = await GetElfInventories();

    var carryingMost = elfInventories.OrderByDescending(x => x.Sum()).First();
    var totalCalories = carryingMost.Sum();

    Console.WriteLine("Total calories: {0}.", totalCalories);
}

static async Task<List<List<int>>> GetElfInventories()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var list = new List<List<int>>();
    var currentElfInventory = new List<int>();
    foreach (var line in lines)
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            currentElfInventory.Add(int.Parse(line));
        }
        else
        {
            list.Add(currentElfInventory);
            currentElfInventory = new List<int>();
        }
    }

    return list;
}