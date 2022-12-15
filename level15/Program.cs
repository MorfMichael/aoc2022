using System.Reflection.Metadata.Ecma335;

string[] lines = File.ReadAllLines("level15.in");

List<(int sx, int sy, int bx, int by)> scanners = new();
HashSet<(int x, int y)> beacons = new();
HashSet<(int x, int y)> not = new();

int check = 2000000;

foreach (var line in lines)
{
    var split = line.Split();
    int sx = int.Parse(split[2][2..][..^1]);
    int sy = int.Parse(split[3][2..][..^1]);
    int bx = int.Parse(split[^2][2..][..^1]);
    int by = int.Parse(split[^1][2..]);

    beacons.Add((bx, by));

    var s = (sx, sy, bx, by);
    Console.WriteLine(s);
    scanners.Add(s);
    AddRange(sx, sy, bx, by);
    //Print();
}

void Print()
{
    //for (int y = 0; y <= 40; y++)
    {
        int y = 10;
        for (int x = -10; x <= 30; x++)
        {
            if (scanners.Any(t => t.sx == x && t.sy == y)) Console.Write("S");
            else if (scanners.Any(t => t.bx == x && t.by == y)) Console.Write("B");
            else if (not.Contains((x, y))) Console.Write("#");
            else Console.Write(".");
        }
        Console.WriteLine();
    }
}

Console.WriteLine(not.Count(n => n.y == check));

void AddRange(int sx, int sy, int bx, int by)
{
    int value = ManhattanDistance(sx, sy, bx, by);
    for (int y = -value; y <= value; y++)
    {
        if (sy+y != check) continue;
        for (int x = -value+Math.Abs(y); x <= value-Math.Abs(y); x++)
        {
            var n = (sx + x, sy + y);
            if (beacons.Contains(n)) continue;
            not.Add(n);
        }
    }
}

static int ManhattanDistance(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);