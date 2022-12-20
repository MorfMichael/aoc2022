string[] lines = File.ReadAllLines("level16.ex");

Dictionary<string, int> valves = new();
Dictionary<string, string[]> next = new();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    var split = line.Split();
    string valve = split[1];
    int pressure = int.Parse(split[4][5..^1]);
    string[] n = split[9..].Select(t => t.Replace(",", "")).ToArray();

    valves.Add(valve, pressure);
    next.Add(valve, n);
}

Dictionary<(string, HashSet<string>), int> calc = new();

int Flow(string cur, HashSet<string> open, int left)
{
    if (left <= 0) return 0;

    if (calc.ContainsKey((cur,open))) return calc[(cur, open)];

    int max = 0;

    if (!open.Contains(cur))
    {
        var nopen = open.Append(cur).ToHashSet();
        int flow = valves[cur] * (left - 1);

        foreach (var n in next[cur])
        {
            if (flow > 0)
                max = Math.Max(max, flow + Flow(n, nopen, left - 2));
            max = Math.Max(max, Flow(n, open, left - 1));
        }
    }

    if (!calc.ContainsKey((cur,open))) calc.Add((cur,open), max);
    return max;
}

Console.WriteLine(Flow("AA", new(), 30));