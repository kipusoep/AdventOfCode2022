// ReSharper disable SuggestBaseTypeForParameter

await Assignment1();
Console.WriteLine();
await Assignment2();

static async Task Assignment2()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var fileSystem = GetFileSystem(lines);
    var fileSystemAsList = new List<FSDirectory>()
    {
        fileSystem,
    };
    var dirsFlattened = Traverse(fileSystemAsList, directory => directory.Directories).ToList();
    var totalSpace = 70000000;
    var spaceAvailable = totalSpace - fileSystem.TotalSize;
    var spaceToFreeUp = 30000000 - spaceAvailable;
    var dirToRemove = dirsFlattened.Where(x => x.TotalSize >= spaceToFreeUp).OrderBy(x => x.TotalSize).First();

    Console.WriteLine("Total size of the directory to remove: {0}.", dirToRemove.TotalSize);
}

static async Task Assignment1()
{
    var lines = await File.ReadAllLinesAsync("input/assignment-1.txt");
    var fileSystem = GetFileSystem(lines);
    var fileSystemAsList = new List<FSDirectory>()
    {
        fileSystem,
    };
    var dirsFlattened = Traverse(fileSystemAsList, directory => directory.Directories).ToList();
    var dirsBelow100000 = dirsFlattened.Where(x => x.TotalSize <= 100000).ToList();
    var sum = dirsBelow100000.Sum(x => x.TotalSize);

    Console.WriteLine("Total size of directories smaller than 100000: {0}.", sum);
}

static FSDirectory GetFileSystem(string[] instructions)
{
    var rootDirectory = new FSDirectory("/", null);
    var currentDirectory = rootDirectory;
    foreach (var instruction in instructions)
    {
        if (instruction.StartsWith("$"))
        {
            switch (instruction)
            {
                case "$ ls":
                    break;
                case "$ cd /":
                    currentDirectory = rootDirectory;
                    break;
                case "$ cd ..":
                    currentDirectory = currentDirectory.ParentDirectory;
                    break;
                default:
                    {
                        if (instruction.StartsWith("$ cd "))
                        {
                            currentDirectory = currentDirectory.Directories.Single(x => x.Name == instruction[5..]);
                        }

                        break;
                    }
            }
        }
        else
        {
            if (instruction.StartsWith("dir "))
            {
                currentDirectory.Directories.Add(new FSDirectory(instruction[4..], currentDirectory));
            }
            else
            {
                currentDirectory.Files.Add(new FSFile(instruction[(instruction.IndexOf(' ') + 1)..], int.Parse(instruction[..instruction.IndexOf(' ')])));
            }
        }
    }

    return rootDirectory;
}

static IEnumerable<T> Traverse<T>(IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
{
    var stack = new Stack<T>(items);
    while (stack.Any())
    {
        var next = stack.Pop();
        yield return next;
        foreach (var child in childSelector(next))
            stack.Push(child);
    }
}

public class FSDirectory
{
    public FSDirectory(string name, FSDirectory? parentDirectory)
    {
        Name = name;
        ParentDirectory = parentDirectory;
    }

    public string Name { get; }
    public FSDirectory? ParentDirectory { get; }
    public List<FSFile> Files { get; } = new();
    public List<FSDirectory> Directories { get; } = new();
    public int TotalSize => Files.Sum(x => x.Size) + Directories.Sum(x => x.TotalSize);

    public override string ToString()
    {
        return Name;
    }
}

public class FSFile
{
    public string Name { get; }
    public int Size { get; }

    public FSFile(string name, int size)
    {
        Name = name;
        Size = size;
    }

    public override string ToString()
    {
        return Name;
    }
}