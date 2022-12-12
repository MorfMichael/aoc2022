string[] lines = File.ReadAllLines("level12.in");

List<Point> points = new();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    for (int j = 0; j < line.Length; j++)
    {
        int height = line[j] - 'a' + 1;
        if (line[j] == 'S') height = 1;
        if (line[j] == 'E') height = 26;

        points.Add(new Point(j, i, line[j], height, line[j] == 'S', line[j] == 'E'));
    }
}

var start = points.FirstOrDefault(x => x.start);
var end = points.FirstOrDefault(x => x.end);

List<(int x, int y, int height, int count)> steps = new() { (start.x, start.y, start.height, 0) };
HashSet<(int x, int y)> visited = new() { };

var cur = steps.FirstOrDefault();

while (steps.Any())
{
    cur = steps.OrderBy(t => t.count).FirstOrDefault();
    steps.Remove(cur);

    if (visited.Contains((cur.x, cur.y))) continue;
    visited.Add((cur.x, cur.y));

    if (cur.x == end.x && cur.y == end.y) break;

    foreach (var next in new (int x, int y)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
    {
        var p = points.FirstOrDefault(p => p.x == cur.x + next.x && p.y == cur.y + next.y && p.height <= cur.height + 1);
        if (p != null)
        {
            steps.Add((p.x, p.y, p.height, cur.count + 1));
        }
    }
}

Console.WriteLine(cur.count);

record Point(int x, int y, char value, int height, bool start, bool end);