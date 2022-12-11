static class Program
{
    private static int _cycles = 0;
    private static int _x = 1;
    private static int _total = 0;

    private static async Task Main(string[] args)
    {
        await Assignment1();
        Console.WriteLine();
        _cycles = 0;
        _x = 1;
        await Assignment2();
    }

    private static async Task Assignment2()
    {
        var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
        var crtPos = 0;
        foreach (var line in lines)
        {
            DrawPixel(_x, ref crtPos);
            if (line == "noop")
            {
                IncrementCycles();
            }
            else if (line.StartsWith("addx "))
            {
                var amount = int.Parse(line[5..]);
                IncrementCycles();
                DrawPixel(_x, ref crtPos);
                IncrementCycles();
                _x += amount;
            }
        }
    }

    private static void DrawPixel(int spritePos, ref int crtPos)
    {
        var spritePixels = Enumerable.Range(spritePos - 1, 3).ToList();
        Console.Write(spritePixels.Contains(crtPos) ? "#" : ".");
        if ((crtPos + 1) % 40 == 0)
        {
            Console.Write(Environment.NewLine);
            crtPos = 0;
        }
        else
        {
            crtPos++;
        }
    }

    private static async Task Assignment1()
    {
        var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
        foreach (var line in lines)
        {
            if (line == "noop")
            {
                IncrementCycles(true);
            }
            else if (line.StartsWith("addx "))
            {
                var amount = int.Parse(line[5..]);
                IncrementCycles(true);
                IncrementCycles(true);
                _x += amount;
            }
        }
    }

    private static void IncrementCycles(bool writeCycleInfo = false)
    {
        _cycles++;
        if (writeCycleInfo && (_cycles - 20 == 0 || (_cycles - 20) % 40 == 0))
        {
            _total += (_cycles * _x);
            Console.WriteLine("Cycles: {0}. X: {1}. Multiplied: {2}. Total: {3}.", _cycles, _x, _cycles * _x, _total);
        }
    }
}