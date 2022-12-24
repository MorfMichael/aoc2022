using System.ComponentModel;

string[] lines = File.ReadAllLines("level23.in");

HashSet<(int x, int y)> elves = new();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    for (int j = 0; j < line.Length; j++)
    {
        if (line[j] == '#') elves.Add((j, i));
    }
}

Print();

for (int i = 0; i < 10; i++)
{
    List<((int x, int y) olde, (int x, int y) newe)> moved = new();
    foreach (var elve in elves)
    {
        var m = Move(elve.x, elve.y, i);
        moved.Add((elve, m));
    }

    var allowed = moved.GroupBy(t => t.newe).Where(t => t.Count() == 1).Select(t => t.First()).ToList();

    foreach (var al in allowed)
    {
        elves.Remove(al.olde);
        elves.Add(al.newe);
    }

    //Console.WriteLine($"step {i + 1}");
    //Print();
}

int x1 = elves.Min(e => e.x);
int x2 = elves.Max(e => e.x);
int y1 = elves.Min(e => e.y);
int y2 = elves.Max(e => e.y);

int count = 0;

for (int y = y1; y <= y2; y++)
{
    for (int x = x1; x <= x2; x++)
    {
        if (!elves.Contains((x, y))) count++;
    }
}

Console.WriteLine(count);

(int x, int y) Move(int x, int y, int step)
{
    List<(bool check, (int x, int y) move)> checks = new();

    checks.Add((!GetNorth(x, y).Any(n => elves.Contains(n)), (x,y-1)));
    checks.Add((!GetSouth(x, y).Any(s => elves.Contains(s)), (x,y+1)));
    checks.Add((!GetWest(x, y).Any(w => elves.Contains(w)), (x-1,y)));
    checks.Add((!GetEast(x, y).Any(e => elves.Contains(e)), (x+1,y)));

    if (checks.All(t => t.check)) return (x, y);

    for (int i = 0; i < 4; i++)
    {
        int idx = (step+i) % 4;
        if (checks[idx].check) return checks[idx].move;
    }

    return (x, y);
}

(int x, int y)[] GetNorth(int x, int y) => new (int x, int y)[] { (x - 1, y - 1), (x, y - 1), (x + 1, y - 1) };
(int x, int y)[] GetSouth(int x, int y) => new (int x, int y)[] { (x - 1, y + 1), (x, y + 1), (x + 1, y + 1) };
(int x, int y)[] GetWest(int x, int y) => new (int x, int y)[] { (x - 1, y - 1), (x - 1, y), (x - 1, y + 1) };
(int x, int y)[] GetEast(int x, int y) => new (int x, int y)[] { (x + 1, y - 1), (x + 1, y), (x + 1, y + 1) };

void Print()
{
    int margin = 3;
    int x1 = elves.Min(e => e.x) - margin;
    int x2 = elves.Max(e => e.x) + margin;
    int y1 = elves.Min(e => e.y) - margin;
    int y2 = elves.Max(e => e.x) + margin;

    for (int y = y1; y <= y2; y++)
    {
        for (int x = x1; x <= x2; x++)
        {
            Console.Write(elves.Contains((x, y)) ? "#" : ".");
        }
        Console.WriteLine();
    }
}
