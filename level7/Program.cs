string[] lines = File.ReadAllLines("level7.in");

int count = 0;
int max = 100000;

Container previous = null;
Container current = null;
for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    if (line.StartsWith("$ cd"))
    {
        string dir = line.Split(" ")[2];
        if (dir == "..") current = previous;
        else current = current?.Containers.FirstOrDefault(x => x.Name == dir) ?? new Container(current, dir);
    }

    if (line == "$ ls") continue;

    if (line.StartsWith("dir"))
    {
        string dir = line.Split(" ")[1];
        current.Containers.Add(new Container(current, dir));
    }

    string[] split = line.Split(' ');
    if (int.TryParse(split[0], out var size))
    {
        var file = new Entry(split[1],size);
        if (!current.Files.Any(t => t.Name == file.Name)) current.Files.Add(file);
    }

    previous = current;
}

while (current.Parent != null)
{
    current = current.Parent;
}

Console.WriteLine(Calculate(current).Sum());

IEnumerable<int> Calculate(Container container)
{
    int sum = 0;
    foreach (var c in current.Containers.SelectMany(Calculate))
    {
        sum += c;
    }

    if (current.Sum() + sum <= max)
        yield return current.Sum() + sum;
}


Console.WriteLine(current.Sum());

class Container
{
    public Container(Container parent, string name, params Entry[] files)
    {
        Parent = parent;
        Name = name;
        Files = new(files);
        Containers = new List<Container>();
    }

    public Container Parent { get; set; }
    public string Name { get; set; }
    public List<Entry> Files { get; set; }
    public List<Container> Containers { get; set; }

    public int Sum() => Files.Sum(x => x.Size);
}

class Entry
{
    public Entry(string name, int size)
    {
        Name = name;
        Size = size;
    }
    public string Name { get; set; }
    public int Size { get; set; }
}