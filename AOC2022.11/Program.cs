using NCalc;

await Assignment1();
Console.WriteLine();
await Assignment2();

static async Task Assignment2()
{
    await CalculateMonkeyBusiness(10000, false);
}

static async Task Assignment1()
{
    await CalculateMonkeyBusiness(20, true);
}

static async Task CalculateMonkeyBusiness(int rounds, bool useRelief)
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var chunks = lines.Chunk(7).ToList();
    var monkeys = new List<Monkey>();
    foreach (var chunk in chunks)
    {
        var number = int.Parse(chunk[0].Replace("Monkey", string.Empty).Replace(":", string.Empty).Trim());
        var items = chunk[1];
        var operation = chunk[2];
        var test = chunk[3];
        var testTrue = chunk[4];
        var testFalse = chunk[5];

        var monkey = new Monkey(number);
        items.Replace("Starting items:", string.Empty).Trim().Split(", ").Select(int.Parse).ToList().ForEach(x => monkey.Items.Enqueue(x));
        operation = operation[(operation.IndexOf('=') + 2)..];
        monkey.Operation = old =>
        {
            var expression = new Expression(operation)
            {
                Parameters =
                {
                    ["old"] = old,
                },
            };
            return (long)expression.Evaluate();
        };
        monkey.DivisibleBy = int.Parse(test.Replace("Test: divisible by", string.Empty).Trim());
        var trueMonkey = int.Parse(testTrue.Replace("If true: throw to monkey", string.Empty).Trim());
        var falseMonkey = int.Parse(testFalse.Replace("If false: throw to monkey", string.Empty).Trim());
        monkey.Test = item => item % monkey.DivisibleBy == 0 ? trueMonkey : falseMonkey;
        monkeys.Add(monkey);
    }

    var factor = monkeys.Aggregate(1, (c, m) => c * m.DivisibleBy);

    for (var round = 1; round <= rounds; round++)
    {
        foreach (var monkey in monkeys)
        {
            while (monkey.Items.TryDequeue(out var item))
            {
                monkey.Inspections++;
                item = monkey.Operation(item);
                if (useRelief)
                {
                    item /= 3;
                }
                else
                {
                    item %= factor;
                }

                var monkeyToThrowTo = monkey.Test(item);
                monkeys.Single(x => x.Number == monkeyToThrowTo).Items.Enqueue(item);
            }
        }
    }

    monkeys = monkeys.OrderByDescending(x => x.Inspections).ToList();
    var monkeyBusiness = monkeys.Take(2).Aggregate(1l, (x, y) => x * y.Inspections);

    Console.WriteLine("The level of monkey business is: {0}.", monkeyBusiness);
}

internal class Monkey
{
    public int Number { get; }
    public Queue<long> Items { get; set; } = new();
    public Func<long, long> Operation { get; set; }
    public int DivisibleBy { get; set; }
    public Func<long, int> Test { get; set; }
    public long Inspections { get; set; }

    public Monkey(int number)
    {
        Number = number;
    }

    public override string ToString()
    {
        return $"Monkey {Number}";
    }
}