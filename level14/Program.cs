string[] lines = File.ReadAllLines("level14.in");

List<(int x, int y)> blocked = new();

foreach (var line in lines)
{
    var entries = line.Split(" -> ").Select(t => t.Split(',')).Select(t => (x: int.Parse(t[0]), y: int.Parse(t[1]))).ToList();

    var start = entries.First();
    foreach (var entry in entries)
    {
        if (entry == start) continue;

        (int xs, int xe) = start.x < entry.x ? (start.x, entry.x) : (entry.x, start.x);
        (int ys, int ye) = start.y < entry.y ? (start.y, entry.y) : (entry.y, start.y);

        for (int x = xs; x <= xe; x++) blocked.Add((x, ys));
        for (int y = ys; y <= ye; y++) blocked.Add((xs, y));

        start = entry;
    }
}

int count = 0;
int max = blocked.Max(b => b.y);
(int x, int y) cur = (500, 0);

while (true)
{
    if (cur.y > max) break;

    var newcur = Move(cur.x, cur.y);
    if (newcur == null) // rest
    {
        blocked.Add(cur);
        cur = (500, 0);
        count++;
    }
    else
    {
        cur = newcur.Value;
    }
}

Console.WriteLine(count);

//for (int y = 10; y < 150; y++)
//{
//    for (int x = 480; x < 580; x++)
//    {
//        var b = blocked.FirstOrDefault(m => m.x == x && m.y == y);
//        Console.Write(b != default ? '#' : '.');
//    }
//    Console.WriteLine();
//}

(int x, int y)? Move(int x, int y)
{
    if (!blocked.Any(t => t.x == x && t.y == y + 1)) return (x,y+1);

    if (!blocked.Any(t => t.x == x - 1 && t.y == y + 1)) return (x - 1, y + 1);

    if (!blocked.Any(t => t.x == x + 1 && t.y == y + 1)) return (x + 1, y + 1);

    return null;
}