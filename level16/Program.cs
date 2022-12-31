// (c) by yt hyper-neutrino
string[] lines = File.ReadAllLines("level16.in");

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

List<string> nonempty = new();
Dictionary<string, Dictionary<string, int>> distances = new();

foreach (var valve in valves)
{
    if (valve.Key != "AA" && valve.Value == 0)
        continue;

    if (valve.Key != "AA")
        nonempty.Add(valve.Key);

    distances.Add(valve.Key, new() { { valve.Key, 0 } });

    HashSet<string> visited = new() { valve.Key };

    Queue<(int distance, string valve)> queue = new();
    queue.Enqueue((0, valve.Key));

    while (queue.Any())
    {
        (int distance, string pos) = queue.Dequeue();
        foreach (var n in next[pos])
        {
            if (visited.Contains(n))
                continue;

            visited.Add(n);

            if (valves[n] > 0)
                distances[valve.Key].Add(n, distance + 1);
            queue.Enqueue((distance + 1, n));
        }
    }

    distances[valve.Key].Remove(valve.Key);
}

Dictionary<string, int> indices = nonempty.Select((t,i) => (t,i)).ToDictionary(x => x.t, x => x.i);
Dictionary<(int time, string valve, int bitmask), int> cache = new();

int Flow(int time, string valve, int bitmask)
{
    if (cache.ContainsKey((time, valve, bitmask)))
        return cache[(time, valve, bitmask)];

    int max = 0;

    foreach (var n in distances[valve])
    {
        int bit = 1 << indices[n.Key];

        if ((bitmask & bit) > 0)
            continue;

        int remtime = time - distances[valve][n.Key] - 1;
        
        if (remtime <= 0)
            continue;

        max = Math.Max(max, Flow(remtime, n.Key, bitmask | bit) + valves[n.Key] * remtime);
    }

    return max;
}

Console.WriteLine(Flow(30, "AA", 0));