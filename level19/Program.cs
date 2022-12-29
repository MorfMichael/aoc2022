string[] lines = File.ReadAllLines("level19.in");

int count = 0;

List<Blueprint> blueprints = new();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    blueprints.Add(new Blueprint(line));

    count++;
}

Console.WriteLine(count);

Dictionary<string, int> init = new()
{
    { "ore", 0 },
    { "clay", 0 },
    { "obsidian", 0 },
    { "geode", 0 },
};

foreach (var bp in blueprints)
{
    HashSet<(int minute, string collecting, string resources)> seen = new();

    Queue<(int minute, List<Robot> collecting, Dictionary<string, int> resources)> queue = new();
    queue.Enqueue((24, new() { new Robot("ore", new()) }, CopyResources(init)));

    long i = 0;
    while (queue.Any())
    {
        var cur = queue.Dequeue();

        var s = (cur.minute, string.Join(",", cur.collecting), string.Join(",", cur.resources));

        if (seen.Contains(s))
            continue;
        
        seen.Add(s);

        if (i % 10000 == 0)
            Console.WriteLine((bp.Id, bp.Geode, seen.Count));

        if (cur.minute <= 0)
        {
            //Console.WriteLine(string.Join(", ", cur.collecting.GroupBy(t => t.Type).Select(t => $"{t.Count()} {t.Key}")));
            //Console.WriteLine($"Blueprint {bp.Id}: {geode}");
            //Console.WriteLine(string.Join(",", cur.resources));
            bp.Geode = Math.Max(bp.Geode, cur.resources["geode"]);
            continue;
        }

        //if (cur.resources["ore"] >= cur.minute * bp.Robots.Max(t => t.Cost["ore"]))
        //{
        //    cur.resources["ore"] = cur.minute * bp.Robots.Max(t => t.Cost["ore"]);
        //}

        //if (cur.resources["clay"] >= cur.minute * bp.Robots[2].Cost["clay"])
        //    cur.resources["clay"] = cur.minute * bp.Robots[2].Cost["clay"];

        //if (cur.resources["obsidian"] >= cur.minute * bp.Robots[3].Cost["obsidian"])
        //    cur.resources["obsidian"] = cur.minute * bp.Robots[3].Cost["obsidian"];

        var options = bp.Robots.Where(t => t.CanSpend(cur.resources)).ToList();

        // collect
        foreach (var r in cur.collecting)
        {
            cur.resources[r.Type]++;
        }
        
        queue.Enqueue((cur.minute - 1, cur.collecting, cur.resources));
        // add options to queue
        foreach (var option in options)
        {
            var newcollecting = new List<Robot>(cur.collecting);
            newcollecting.Add(option);
            var newresources = CopyResources(cur.resources);
            option.Spend(newresources);
            queue.Enqueue((cur.minute - 1, newcollecting, newresources));
        }
        i++;
    }

    Console.WriteLine($"Blueprint {bp.Id}: {bp.Geode}");
}


Console.WriteLine(blueprints.Sum(x => x.Id * x.Geode));


Dictionary<string, int> CopyResources(Dictionary<string, int> resources) => resources.ToDictionary(t => t.Key, t => t.Value);
string PrintRobots(IEnumerable<Robot> robots) => string.Join("|", robots.GroupBy(t => t.Type).Select(t => $"{t.Count()} {t.Key}"));

class Blueprint
{
    public Blueprint(string line)
    {
        var split = line.Split(new char[] { ':', '.' }, StringSplitOptions.RemoveEmptyEntries);
        var blueprint = split[0].Split();
        Id = int.Parse(blueprint[1]);
        Robots = new List<Robot>();
        foreach (var r in split[1..])
        {
            var rsplit = r.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(t => t != "and").ToArray();

            string type = rsplit[1];
            var costs = rsplit[4..].Chunk(2).ToDictionary(x => x[1], x => int.Parse(x[0]));

            Robots.Add(new Robot(type, costs));
        }
    }

    public int Id { get; set; }
    public List<Robot> Robots { get; set; }

    public int Geode { get; set; }
}

class Robot
{
    public Robot(string type, Dictionary<string, int> cost)
    {
        Type = type;
        Cost = cost;
    }

    public string Type { get; set; }

    public int Weight => Type switch
    {
        "ore" => 0,
        "clay" => 1,
        "obsidian" => 2,
        "geode" => 3,
    };

    public Dictionary<string, int> Cost { get; set; }

    public bool CanSpend(Dictionary<string, int> resources) => Cost.All(c => resources[c.Key] >= c.Value);

    public void Spend(Dictionary<string, int> resources)
    {
        foreach (var c in Cost)
        {
            resources[c.Key] -= c.Value;
        }
    }

    public int Takes(Dictionary<string, int> resources, List<Robot> collecting)
    {
        if (Cost.All(c => resources[c.Key] >= c.Value))
        {
            return 0;
        }

        bool possible = Cost.All(c => collecting.Any(r => r.Type == c.Key));
        if (possible)
        {
            int minute = Cost.Select(c => (c.Key, Remaining: c.Value - resources[c.Key])).Max(t => t.Remaining / collecting.Count(x => x.Type == t.Key));
            return minute;
        }

        return -1;
    }

    public override string ToString() => Type;
}


