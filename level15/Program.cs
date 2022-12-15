using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

string[] lines = File.ReadAllLines("level15.in");

List<(long sx, long sy, long bx, long by, long d)> scanners = new();
HashSet<(long x, long y)> beacons = new();
HashSet<(long x, long y)> not = new();

long check = 2000000;

foreach (var line in lines)
{
    var split = line.Split();
    long sx = long.Parse(split[2][2..][..^1]);
    long sy = long.Parse(split[3][2..][..^1]);
    long bx = long.Parse(split[^2][2..][..^1]);
    long by = long.Parse(split[^1][2..]);
    long d = ManhattanDistance(sx, sy, bx, by);
    beacons.Add((bx, by));

    var s = (sx, sy, bx, by, d);
    scanners.Add(s);
}

var bla = GetRanges(2000000).OrderBy(t => t.a).ThenBy(x => x.b).ToList();
var merged = MergeRanges(bla);
long res = merged.Sum(x => x.b > x.a ? x.b - x.a : x.a - x.b);
Console.WriteLine(res);

IEnumerable<(long a, long b)> GetRanges(long y)
{
    foreach (var scanner in scanners)
    {
        if (y > scanner.sy + scanner.d || y < scanner.sy - scanner.d) continue;
        long myy = Math.Abs(scanner.sy - y);
        var r = (x1: scanner.sx - (scanner.d - myy), x2: scanner.sx + (scanner.d - myy));
        Console.WriteLine(scanner + "; " + r + " -> " + (r.x2 > r.x1 ? r.x2 - r.x1 : r.x1 - r.x2));
        yield return r;
    }
}

IEnumerable<(long a, long b)> MergeRanges(List<(long a, long b)> ranges)
{
    HashSet<(long a, long b)> result = new();

    var queue = new Queue<(long a, long b)>(ranges);
    var cur = queue.Dequeue();
    while (queue.Any())
    {
        var next = queue.Dequeue();

        if (cur.a <= next.a && cur.b >= next.b) continue;
        else if (cur.a <= next.b && cur.b >= next.a) cur.b = next.b;
        else
        {
            yield return cur;
            cur = next;
        }
    }

    yield return cur;
}

void AddRange(long sx, long sy, long bx, long by)
{
    long value = ManhattanDistance(sx, sy, bx, by);
}

static long ManhattanDistance(long x1, long y1, long x2, long y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);