using Microsoft.VisualStudio.TestTools.UnitTesting;

class Program
{
    private const bool TestingExample = false;
    private static int _rowLength;
    private static int _colLength;

    private static async Task Main(string[] args)
    {
        await Assignment1();
        Console.WriteLine();
        await Assignment2();
    }

    private static async Task Assignment2()
    {
        var trees = await GetTrees();
        var largestViewDistance = 0;
        for (var i = 0; i < _rowLength; i++)
        {
            for (var j = 0; j < _colLength; j++)
            {
                var tree = trees[i, j];
                var viewDistances = new List<int>();
                var viewDistance = 0;
                for (var k = i - 1; k >= 0; k--)
                {
                    var otherTree = trees[k, j];
                    viewDistance++;
                    if (otherTree.Item1 >= tree.Item1)
                    {
                        break;
                    }
                }
                viewDistances.Add(viewDistance);

                viewDistance = 0;
                for (var k = j - 1; k >= 0; k--)
                {
                    var otherTree = trees[i, k];
                    viewDistance++;
                    if (otherTree.Item1 >= tree.Item1)
                    {
                        break;
                    }
                }
                viewDistances.Add(viewDistance);

                viewDistance = 0;
                for (var k = j + 1; k < _colLength; k++)
                {
                    var otherTree = trees[i, k];
                    viewDistance++;
                    if (otherTree.Item1 >= tree.Item1)
                    {
                        break;
                    }
                }
                viewDistances.Add(viewDistance);

                viewDistance = 0;
                for (var k = i + 1; k < _rowLength; k++)
                {
                    var otherTree = trees[k, j];
                    viewDistance++;
                    if (otherTree.Item1 >= tree.Item1)
                    {
                        break;
                    }
                }
                viewDistances.Add(viewDistance);

                largestViewDistance = Math.Max(largestViewDistance, viewDistances.Aggregate(1, (x, y) => x * y));
            }
        }

        Console.WriteLine("Largest view distance: {0}.", largestViewDistance);
    }

    private static async Task Assignment1()
    {
        var trees = await GetTrees();

        var treesVisible = 0;
        int highestTreeHeightInLine;
        for (var i = 0; i < _rowLength; i++)
        {
            highestTreeHeightInLine = -1;
            for (var j = 0; j < _colLength; j++)
            {
                if (!IsTreeHigher(ref trees[i, j], ref highestTreeHeightInLine, ref treesVisible))
                {
                    break;
                }
            }
        }

        if (TestingExample)
        {
            Assert.AreEqual(11, treesVisible);
        }

        for (var i = 0; i < _rowLength; i++)
        {
            highestTreeHeightInLine = -1;
            for (var j = _colLength - 1; j >= 0; j--)
            {
                if (!IsTreeHigher(ref trees[i, j], ref highestTreeHeightInLine, ref treesVisible))
                {
                    break;
                }
            }
        }

        if (TestingExample)
        {
            Assert.AreEqual(18, treesVisible);
        }

        for (var i = 0; i < _colLength; i++)
        {
            highestTreeHeightInLine = -1;
            for (var j = 0; j < _rowLength; j++)
            {
                if (!IsTreeHigher(ref trees[j, i], ref highestTreeHeightInLine, ref treesVisible))
                {
                    break;
                }
            }
        }

        if (TestingExample)
        {
            Assert.AreEqual(20, treesVisible);
        }

        for (var i = 0; i < _colLength; i++)
        {
            highestTreeHeightInLine = -1;
            for (var j = _rowLength - 1; j >= 0; j--)
            {
                if (!IsTreeHigher(ref trees[j, i], ref highestTreeHeightInLine, ref treesVisible))
                {
                    break;
                }
            }
        }

        if (TestingExample)
        {
            Assert.AreEqual(21, treesVisible);
        }

        Console.WriteLine("Number of visible trees: {0}.", treesVisible);
    }

    private static async Task<(int, bool)[,]> GetTrees()
    {
        var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
        _rowLength = lines.First().Length;
        _colLength = lines.Length;
        var trees = new (int, bool)[_rowLength, _colLength];
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            for (var j = 0; j < line.Length; j++)
            {
                trees[i, j] = (int.Parse(line[j].ToString()), false);
            }
        }

        return trees;
    }

    private static bool IsTreeHigher(ref (int height, bool counted) tree, ref int prevTreeHeight, ref int treesVisible)
    {
        if (tree.height > prevTreeHeight)
        {
            if (!tree.counted)
            {
                treesVisible++;
                tree.counted = true;
            }
            prevTreeHeight = tree.height;
        }
        return true;
    }
}