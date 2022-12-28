using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

string[] lines = File.ReadAllLines("level17.in");

List<(int x, long y)[]> rocks = new()
{
    new (int x, long y)[] { (0,0), (1,0), (2, 0), (3,0) }, // -
    new (int x, long y)[] { (1,0), (0,1), (1, 1), (2,1), (1,2) }, // +
    new (int x, long y)[] { (0,0), (1,0), (2, 0), (2,1), (2,2) }, // Lr
    new (int x, long y)[] { (0,0), (0,1), (0, 2), (0,3) }, // I
    new (int x, long y)[] { (0,0), (1,0), (0, 1), (1,1) }, // []
};

HashSet<(int x, long y)> map = new();

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

Dictionary<(int rock, int instruction, string pattern), (long rcount, long top)> patterns = new();

int iteration = 0;
long rcount = 0;
(int x, long y)[] rock = null;
(int x, long y) cur = (0, 0);
bool down = false;
long top = 0;

//long limit = 2022;
long limit = 1_000_000_000_000;
long added = 0;

while (true)
{
    if (rcount > limit)
    {
        if (rcount > 2022)
            Console.WriteLine(top + 1 + added);
        else
            Console.WriteLine(top + 1);
        return;
    }

    if (rock == null)
    {
        cur.x = 2;
        cur.y = map.Any() ? map.Max(m => m.y) + 4 : 3;
        rock = rocks[(int)(rcount % rocks.Count)];
        rcount++;
        down = false;
    }

    if (down)
    {
        if (Collision(cur.x, cur.y - 1, rock) == 0) cur.y--;
        else // settle
        {
            foreach (var r in rock)
            {
                map.Add((cur.x + r.x, cur.y + r.y));
            }
            top = map.Max(x => x.y);
            rock = null;

            if (rcount > 2022)
            {
                var pattern = GetPattern();
                var key = (r: (int)(rcount % rocks.Count), i: iteration, p: pattern);
                if (patterns.ContainsKey(key))
                {
                    (long orc, long oldy) = patterns[key];
                    var dy = top - oldy;
                    var rc = rcount - orc;
                    var amt = (long)Math.Floor((double)(limit - rcount) / rc);
                    added += amt * dy;
                    rcount += amt * rc;
                }
                else
                {
                    patterns.Add(key, (rcount, top));
                }
            }
        }
    }
    else
    {
        var instr = instructions[(int)(iteration)];
        iteration = (iteration + 1) % instructions.Count;
        int ix = instr switch { '<' => -1, '>' => 1 };
        if (Collision(cur.x + ix, cur.y, rock) == 0) cur.x += ix;
    }
    down = !down;
}

Console.WriteLine(top + added);

int Collision(int x, long y, (int x, long y)[] rock)
{
    foreach (var r in rock)
    {
        int rx = x + r.x;
        long ry = y + r.y;
        if (map.Contains((rx, ry))) return 1; // rock
        if (rx < 0 || rx > 6) return 2; // wall
        if (ry < 0) return 3; // floor
    }

    return 0;
}

void Print()
{
    if (!map.Any()) return;

    long my = map.Max(m => m.y) + 3;

    for (long y = my; y >= 0; y--)
    {
        for (int x = 0; x < 7; x++)
        {
            if (map.Contains((x, y))) Console.Write('#');
            else Console.Write('.');
        }
        Console.WriteLine();
    }
}

string GetPattern()
{
    long maxy = map.Max(x => x.y);
    long miny = maxy - 30;
    var pattern = map.Where(t => t.y >= miny).Select(t => (x: t.x, y: t.y - miny)).ToHashSet();
    return string.Join(";",pattern);
}