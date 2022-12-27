using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

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

Console.WriteLine(Flow("AA", new string[0], 30));

//int vmax = valves.Max(x => x.Value);

//int max = 0;

//PriorityQueue<Game, int> queue = new(Comparer<int>.Create((a, b) => b - a));
//queue.Enqueue(new Game("AA", new(), 30), 0);

//while (queue.Count > 0)
//{
//    Game cur = default;
//    int sum = 0;
//    if (queue.TryPeek(out var game, out int s))
//    {
//        cur = queue.Dequeue();
//        sum = s;
//    }

//    if (cur.left <= 0)
//    {
//        if (sum > max)
//        {
//            max = sum;
//            Console.WriteLine(sum);
//        }
//        continue;
//    }

//    int flow = !cur.open.Contains(cur.valve) ? (cur.left - 1) * valves[cur.valve] : 0;
//    HashSet<string> nopen = new HashSet<string>(cur.open);
//    nopen.Add(cur.valve);
//    foreach (var n in next[cur.valve])
//    {
//        if (flow > 0)
//            queue.Enqueue(new Game(n, nopen, cur.left - 2), flow + sum);

//        queue.Enqueue(new Game(n, cur.open, cur.left - 1), sum);
//    }
//}


int Flow(string valve, string[] open, int left)
{
    if (left <= 0) return 0;

    int max = 0;

    int flow = !open.Contains(valve) ? (left - 1) * valves[valve] : 0;
    string[] nopen = new string[open.Length + 1];
    for (int i = 0; i < open.Length; i++) nopen[i] = open[i];
    nopen[^1] = valve;
    foreach (var n in next[valve])
    {
        if (flow > 0)
            max = Math.Max(max, flow + Flow(n, nopen, left - 2));

        max = Math.Max(max, Flow(n, open, left - 1));
    }

    return max;
}



record struct Game(string valve, HashSet<string> open, int left);
