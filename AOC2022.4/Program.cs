await Assignment1();
Console.WriteLine();
await Assignment2();

static async Task Assignment2()
{
    List<PairOfElves> pairs = await GetPairsOfElves();

    Console.WriteLine("Number of pairs where one range overlaps the other: {0}.", pairs.Where(x => x.RangeIsOverlappingOtherRange).Count());
}

static async Task Assignment1()
{
    List<PairOfElves> pairs = await GetPairsOfElves();

    Console.WriteLine("Number of pairs where one range fully contains the other: {0}.", pairs.Where(x => x.FullRangeIsContainedByOtherRange).Count());
}


static async Task<List<PairOfElves>> GetPairsOfElves()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var pairs = new List<PairOfElves>();
    foreach (var line in lines)
    {
        var pair1 = line[..line.IndexOf(',')];
        var pair2 = line[(line.IndexOf(',') + 1)..];
        var pair1Start = int.Parse(pair1[..pair1.IndexOf('-')]);
        var pair1End = int.Parse(pair1[(pair1.IndexOf('-') + 1)..]);
        var pair2Start = int.Parse(pair2[..pair2.IndexOf('-')]);
        var pair2End = int.Parse(pair2[(pair2.IndexOf('-') + 1)..]);
        pairs.Add(new PairOfElves
        {
            Elf1Range = Enumerable.Range(pair1Start, pair1End - pair1Start + 1).ToList(),
            Elf2Range = Enumerable.Range(pair2Start, pair2End - pair2Start + 1).ToList(),
        });
    }

    return pairs;
}

internal class PairOfElves
{
    public List<int> Elf1Range = new();
    public List<int> Elf2Range = new();
    public bool FullRangeIsContainedByOtherRange => !Elf1Range.Except(Elf2Range).Any() || !Elf2Range.Except(Elf1Range).Any();
    public bool RangeIsOverlappingOtherRange => Elf1Range.Intersect(Elf2Range).Any() || Elf2Range.Intersect(Elf1Range).Any();
}