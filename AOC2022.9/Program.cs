await Assignment1();
Console.WriteLine();

static async Task Assignment1()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
}