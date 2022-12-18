using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

string[] lines = File.ReadAllLines("level18.in");

HashSet<(int x, int y, int z)> blocks = new();
var neighbour = new (int x, int y, int z)[]
 {
        (0,0,-1), (0,0,1),
        (-1,0,0), (1,0,0),
        (0,-1,0), (0,1,0),
 };


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
    if (Outside((block.x-1, block.y, block.z))) sum++;
    if (Outside((block.x+1, block.y, block.z))) sum++;
    if (Outside((block.x, block.y-1, block.z))) sum++;
    if (Outside((block.x, block.y+1, block.z))) sum++;
    if (Outside((block.x, block.y, block.z-1))) sum++;
    if (Outside((block.x, block.y, block.z+1))) sum++;
}

Console.WriteLine(sum);


bool Outside((int x, int y, int z) block)
{
    HashSet<(int x, int y, int z)> visited = new();
    Queue<(int x, int y, int z)> queue = new();
    queue.Enqueue(block);

    while (queue.Any())
    {
        var cur = queue.Dequeue();

        if (blocks.Contains(cur)) continue;
        if (visited.Contains(cur)) continue;
        visited.Add(cur);

        if (visited.Count > 5000) return true;

        foreach (var n in neighbour)
            queue.Enqueue((cur.x + n.x, cur.y + n.y, cur.z + n.z));
    }

    return false;
}