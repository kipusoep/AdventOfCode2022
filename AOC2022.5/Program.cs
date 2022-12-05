using System.Text.RegularExpressions;
// ReSharper disable SuggestBaseTypeForParameter

await Assignment1();
Console.WriteLine();
await Assignment2();

static async Task Assignment2()
{
    var (stackColumns, instructionLines) = await GetStackColumnsAndInstructions();
    var stacks = ParseCratesStacks(stackColumns);
    var instructions = ParseInstructions(instructionLines, stacks).ToList();

    foreach (var instruction in instructions)
    {
        var crates = new List<char>();
        for (var i = 0; i < instruction.Amount; i++)
        {
            crates.Add(instruction.FromStack.Crates.Pop());
        }
        crates.Reverse();
        crates.ForEach(x => instruction.ToStack.Crates.Push(x));
    }

    Console.WriteLine("Top crates: {0}.", string.Join(string.Empty, stacks.Select(x => x.Crates.First())));
}

static async Task Assignment1()
{
    var (stackColumns, instructionLines) = await GetStackColumnsAndInstructions();
    var stacks = ParseCratesStacks(stackColumns);
    var instructions = ParseInstructions(instructionLines, stacks).ToList();

    foreach (var instruction in instructions)
    {
        for (var i = 0; i < instruction.Amount; i++)
        {
            var crate = instruction.FromStack.Crates.Pop();
            instruction.ToStack.Crates.Push(crate);
        }
    }

    Console.WriteLine("Top crates: {0}.", string.Join(string.Empty, stacks.Select(x => x.Crates.First())));
}

static async Task<(List<string> stackColumns, List<string> instructions)> GetStackColumnsAndInstructions()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var stackColumns = lines.TakeWhile(x => !string.IsNullOrEmpty(x)).ToList();
    var instructions = lines.Skip(stackColumns.Count + 1).ToList();

    return (stackColumns, instructions);
}

static List<CratesStack> ParseCratesStacks(List<string> stackColumns)
{
    var amountOfStacks = (int)Math.Ceiling(stackColumns.First().Length / 4d);
    var stacks = Enumerable.Range(0, amountOfStacks).Select(x => new CratesStack(x + 1)).ToList();
    foreach (var stackRow in stackColumns.Take(stackColumns.Count - 1).Reverse())
    {
        for (var i = 0; i < amountOfStacks; i++)
        {
            var start = i * 4;
            var end = start + 3;
            var crate = Convert.ToChar(stackRow.Substring(start + 1, end - start - 2));
            if (crate != ' ')
            {
                stacks[i].Crates.Push(crate);
            }
        }
    }

    return stacks;
}

static IEnumerable<Instruction> ParseInstructions(List<string> instructions, List<CratesStack> cratesStacks)
{
    var instructionRegex = new Regex("move (?<amount>\\d+) from (?<from>\\d+) to (?<to>\\d+)", RegexOptions.Compiled);

    foreach (var instruction in instructions)
    {
        var match = instructionRegex.Match(instruction);
        var amount = int.Parse(match.Groups["amount"].Value);
        var from = int.Parse(match.Groups["from"].Value);
        var to = int.Parse(match.Groups["to"].Value);

        var fromStack = cratesStacks.Single(x => x.Number == from);
        var toStack = cratesStacks.Single(x => x.Number == to);

        yield return new Instruction
        {
            FromStack = fromStack,
            ToStack = toStack,
            Amount = amount,
        };
    }
}

internal class CratesStack
{
    public int Number { get; }
    public Stack<char> Crates { get; } = new();

    public CratesStack(int number)
    {
        Number = number;
    }
}

internal class Instruction
{
    public CratesStack FromStack { get; init; }
    public CratesStack ToStack { get; init; }
    public int Amount { get; init; }
}