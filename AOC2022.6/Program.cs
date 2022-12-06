// ReSharper disable SuggestBaseTypeForParameter

await Assignment1();
Console.WriteLine();
await Assignment2();

static async Task Assignment2()
{
    var line = (await File.ReadAllLinesAsync("input/assignment-1.txt")).Single();

    GetMarkerPosition(line, 14);
}

static async Task Assignment1()
{
    var line = (await File.ReadAllLinesAsync("input/assignment-1.txt")).Single();

    GetMarkerPosition(line, 4);
}

static void GetMarkerPosition(string input, int length)
{
    for (var i = 0; i < input.Length; i++)
    {
        var marker = input[i..(i + length)];
        if (marker.Distinct().Count() == length)
        {
            Console.WriteLine("The marker is: {0}. Position: {1}.", marker, i + length);
            break;
        }
    }
}