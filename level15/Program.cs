using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

string[] lines = File.ReadAllLines("level15.in");

List<(int sx, int sy, int bx, int by, int d)> scanners = new();
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
    int d = ManhattanDistance(sx, sy, bx, by);
    beacons.Add((bx, by));

    var s = (sx, sy, bx, by, d);
    //Console.WriteLine(s);
    scanners.Add(s);
    //AddRange(sx, sy, bx, by);
}

/*
3,3
4,4
.......
...#...
..###..
.##S##.
..##B..
...#...
.......
*/
long bla = Range(2000000).ToList().Sum(x => x.x2 > x.x1 ? x.x2 - x.x1 : x.x1 - x.x2);
Console.WriteLine(bla);

IEnumerable<(int x1, int x2)> Range(int y)
{
    foreach (var scanner in scanners)
    {
        if (y > scanner.sy + scanner.d || y < scanner.sy - scanner.d) continue;
        int myy = Math.Abs(scanner.sy - y);
        var r = (x1: scanner.sx - (scanner.d - myy), x2: scanner.sx + (scanner.d - myy));
        Console.WriteLine(scanner + "; " + r + " -> " + (r.x2 > r.x1 ? r.x2 - r.x1 : r.x1 - r.x2));
        yield return r;
    }
}

void AddRange(int sx, int sy, int bx, int by)
{
    int value = ManhattanDistance(sx, sy, bx, by);
}

static int ManhattanDistance(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);