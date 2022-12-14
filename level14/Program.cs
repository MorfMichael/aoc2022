string[] lines = File.ReadAllLines("level14.in");

HashSet<(int x, int y)> blocked = new();

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
int floor = max + 2;

(int x, int y) cur = (500, 0);

while (true)
{
    var newcur = Move(cur);
    if (newcur == null || newcur.HasValue && newcur.Value.y == floor) // rest
    {
        if (cur.x == 500 && cur.y == 0) { count++; break; }
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

(int x, int y)? Move((int x, int y) value)
{
    if (!blocked.Contains((value.x, value.y+1))) return (value.x,value.y+1);

    if (!blocked.Contains((value.x-1, value.y+1))) return (value.x - 1, value.y + 1);

    if (!blocked.Contains((value.x+1, value.y+1))) return (value.x + 1, value.y + 1);

    return null;
}