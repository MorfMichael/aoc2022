string[] lines = File.ReadAllLines("level17.in");

List<(int x, int y)[]> rocks = new()
{
    new (int x, int y)[] { (0,0), (1,0), (2, 0), (3,0) }, // -
    new (int x, int y)[] { (1,0), (0,1), (1, 1), (2,1), (1,2) }, // +
    new (int x, int y)[] { (0,0), (1,0), (2, 0), (2,1), (2,2) }, // Lr
    new (int x, int y)[] { (0,0), (0,1), (0, 2), (0,3) }, // I
    new (int x, int y)[] { (0,0), (1,0), (0, 1), (1,1) }, // []
};

HashSet<(int x, int y)> map = new();

List<char> instructions = new List<char>();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    for (int j = 0; j < line.Length; j++)
    {
        instructions.Add(line[j]);
    }
}

int iteration = 0;
int rcount = 0;
(int x, int y)[] rock = null;
(int x, int y) cur = (0, 0);
bool down = false;
while (true)
{
    if (rcount > 2022)
    {
        Console.WriteLine(map.Max(x => x.y) + 1);
        return;
    }

    if (rock == null)
    {
        cur.x = 2;
        cur.y = map.Any() ? map.Max(m => m.y) + 4 : 3;
        rock = rocks[rcount%rocks.Count];
        rcount++;
        down = false;
    }

    if (down)
    {
        if (Collision(cur.x, cur.y - 1, rock) == 0) cur.y--;
        else
        {
            foreach (var r in rock)
            {
                map.Add((cur.x + r.x, cur.y + r.y));
            }
            rock = null;

            //Print();
            //Console.ReadLine();
        }
    }
    else
    {
        var inst = instructions[iteration++ % instructions.Count];
        int ix = inst switch { '<' => -1, '>' => 1 };
        if (Collision(cur.x + ix, cur.y, rock) == 0) cur.x += ix;
    }
    down = !down;
}

int Collision(int x, int y, (int x, int y)[] rock)
{
    foreach (var r in rock)
    {
        int rx = x + r.x, ry = y + r.y;
        if (map.Contains((rx, ry))) return 1; // rock
        if (rx < 0 || rx > 6) return 2; // wall
        if (ry < 0) return 3; // floor
    }
    
    return 0;
}

void Print()
{
    if (!map.Any()) return;

    int my = map.Max(m => m.y) + 3;

    for (int y = my; y >= 0; y--)
    {
        for (int x = 0; x < 7; x++)
        {
            if (map.Contains((x, y))) Console.Write('#');
            else Console.Write('.');
        }
        Console.WriteLine();
    }
}