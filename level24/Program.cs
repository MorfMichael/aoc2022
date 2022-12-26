using System.Runtime.CompilerServices;
using System.Text;

string[] lines = File.ReadAllLines("level24.in");

int width = lines[0].Length;
int height = lines.Length;

(int x, int y) end = (lines[0].Length - 2, lines.Length - 1);

List<(int x, int y, int direction)> blizzards = new();
HashSet<(int x, int y)> wall = new();

char[] b = new char[4] { '<', '>', '^', 'v' };

for (int y = 0; y < lines.Length; y++)
{
    string line = lines[y];

    if (string.IsNullOrWhiteSpace(line)) continue;

    for (int x = 0; x < line.Length; x++)
    {
        if (b.Contains(line[x]))
        {
            blizzards.Add((x, y, line[x] switch
            {
                '>' => 0,
                'v' => 1,
                '<' => 2,
                '^' => 3,
            }));
        }

        if (line[x] == '#') wall.Add((x, y));
    }
}

HashSet<(int x, int y, int minute)> queue = new();
queue.Add((1, 0, 0));

while (queue.Any())
{
    var cur = queue.OrderBy(t => t.minute).FirstOrDefault();
    queue.Remove(cur);

    if (cur.x == end.x && cur.y == end.y)
    {
        Console.WriteLine(cur.minute);
        return;
    }

    HashSet<(int x, int y)> blizzs = blizzards.Select(t => Blizzard(t.x, t.y, t.direction, cur.minute + 1)).ToHashSet();

    var neighbours = PossibleNeighbours(cur.x, cur.y, blizzs);

    foreach (var n in neighbours)
    {
        queue.Add((n.x, n.y, cur.minute + 1));
    }
}

(int x, int y)[] PossibleNeighbours(int x, int y, HashSet<(int x, int y)> blizz)
{
    var neighbours = new (int x, int y)[]
    {
        (x,y), (x-1, y), (x+1,y), (x, y-1), (x,y+1)
    };

    var result = neighbours.Where(t => t.x >= 0 && t.x < width && t.y >= 0 && t.y < height && !wall.Contains(t) && !blizz.Contains(t)).ToArray();
    return result;
}

(int x, int y) Blizzard(int x, int y, int direction, int minute)
{
    int w = width - 2;
    int h = height - 2;

    if (direction == 0) // right
    {
        int nx = (x - 1 + minute) % w + 1;
        return (nx, y);
    }
    else if (direction == 1) // down
    {
        int ny = (y - 1 + minute) % h + 1;
        return (x, ny);
    }
    else if (direction == 2) // left
    {
        int nx = ((x - 1 + w - (minute % w)) % w) + 1;
        return (nx, y);
    }
    else if (direction == 3) // up
    {
        int ny = ((y - 1 + h - (minute % h)) % h) + 1;
        return (x, ny);
    }

    return (x, y);
}