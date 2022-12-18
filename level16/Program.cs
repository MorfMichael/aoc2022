using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

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

int minute = 0;
int sum = 0;
Queue<(string[] valve, int cost)> queue = new();
HashSet<string> open = new();

string cur = "AA";
bool move = true;

while (++minute <= 30)
{
    Console.WriteLine("== Minute " + minute + " ==");
    int releasing = open.Sum(x => valves[x]);
    sum += releasing;
    Console.WriteLine(open.Any() ? string.Join(",", open) + " releasing " + releasing : "No vavles open!");

    if (move || next[cur].Any(t => valves[t] > valves[cur]))
    {
        var permutation = Permutation(new(), cur).OrderByDescending(t => t.Sum(x => x.pressure)).ToList()[0];
        cur = permutation[0].valve;
        Console.WriteLine("move to " + cur);
        move = false;
    }
    else
    {
        if (valves[cur] > 0)
        {
            open.Add(cur);
            Console.WriteLine(cur + " opened!");
        }
        move = true;
    }
}

IEnumerable<List<(string valve, int pressure)>> Permutation(List<(string valve, int pressure)> previous, string cur)
{
    foreach (var n in next[cur])
    {
        if (open.Contains(n)) continue;

        previous.Add((n, valves[n]));

        yield return previous.ToList();

        foreach (var child in Permutation(previous.ToList(), n))
            yield return child;
    }
}