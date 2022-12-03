await Assignment1();
Console.WriteLine();
await Assignment2();

static async Task Assignment2()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var totalPriorities = 0;
    foreach (var group in lines.Chunk(3))
    {
        var elf1 = group.ElementAt(0);
        var elf2 = group.ElementAt(1);
        var elf3 = group.ElementAt(2);
        var badge = elf1.Intersect(elf2).Intersect(elf3).Single();
        var priority = GetPriority(badge);
        totalPriorities += priority;
    }

    Console.WriteLine("Sum of priorities: {0}.", totalPriorities);
}

static async Task Assignment1()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var totalPriorities = 0;
    foreach (var line in lines)
    {
        var comp1 = line[..(line.Length / 2)];
        var comp2 = line[(line.Length / 2)..];
        var duplicateItem = comp1.Intersect(comp2).Single();
        var priority = GetPriority(duplicateItem);
        totalPriorities += priority;
    }

    Console.WriteLine("Sum of priorities: {0}.", totalPriorities);
}

static int GetPriority(char item)
{
    var priority = char.ToUpper(item) - 'A' + 1;
    if (char.IsUpper(item))
    {
        priority += 26;
    }

    return priority;
}