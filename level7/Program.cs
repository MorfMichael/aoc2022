string[] lines = File.ReadAllLines("level7.in");

int count = 0;
int max = 100000;

Container current = null;
List<Container> containers = new List<Container>();
for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    if (line.StartsWith("$ cd"))
    {
        string dir = line.Split(" ")[2];
        if (dir == "..") current = current.Parent;
        else
        {
            var existing = current?.Containers.FirstOrDefault(x => x.Name == dir);
            if (existing != null)
                current = existing;
            else
            {
                current = new Container(current, dir);
                containers.Add(current);
            }
        }
    }

    if (line.StartsWith("dir"))
    {
        string dir = line.Split(" ")[1];
        if (!current.Containers.Any(t => t.Name == dir))
        {
            var child = new Container(current, dir);
            current.Containers.Add(child);
            containers.Add(child);
        }
    }

    string[] split = line.Split(' ');
    if (int.TryParse(split[0], out var size))
    {
        current.Files.Add((Name: split[1], Size: size));
    }
}

int space = 70000000;
int required = 30000000;

int cur = containers.FirstOrDefault().Sum();
int limit = cur - (space - required);
Console.WriteLine(limit);
Console.WriteLine(containers.OrderBy(t => t.Sum()).FirstOrDefault(x => x.Sum() >= limit).Sum());

class Container
{
    public Container(Container parent, string name, params (string Name, int Size)[] files)
    {
        Parent = parent;
        Name = name;
        Files = new(files);
        Containers = new List<Container>();
    }

    public Container Parent { get; set; }
    public string Name { get; set; }
    public List<(string Name, int Size)> Files { get; set; }
    public List<Container> Containers { get; set; }

    public int Sum() => Files.Sum(x => x.Size) + Containers.Sum(x => x.Sum());

    public override string ToString() => $"{Name} | Files: {string.Join(",", Files.Select(t => t.Name))} | {string.Join(",", Containers.Select(t => t.Name))}";
}