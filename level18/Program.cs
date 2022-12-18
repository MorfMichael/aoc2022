string[] lines = File.ReadAllLines("level18.in");

HashSet<(int x, int y, int z)> blocks = new();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    var b = line.Split(',').Select(int.Parse).ToArray();
    blocks.Add((b[0], b[1], b[2]));
}


int sum = 0;
foreach (var block in blocks)
{
    var n = new (int x, int y, int z)[]
    {
        (0,0,-1), (0,0,1),
        (-1,0,0), (1,0,0),
        (0,-1,0), (0,1,0)
    };

    int sides = 6 - n.Select(t => (block.x + t.x, block.y + t.y, block.z + t.z)).Where(x => blocks.Contains(x)).Count();
    sum += sides;
}


Console.WriteLine(sum);
