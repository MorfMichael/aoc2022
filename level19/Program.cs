string[] lines = File.ReadAllLines("level19.in");

int count = 1;

string[] resources = new string[] { "ore", "clay", "obsidian", "geode" };

Dictionary<int, int[][]> blueprints = new();

for (int i = 0; i < 3; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    var bp = GetCosts(line);

    var v = Calculate(bp.bots, bp.maxspend, new(), 32, new int[] { 1, 0, 0, 0 }, new int[] { 0, 0, 0, 0 });

    count *= v;
}

Console.WriteLine(count);


((int ramt, int rtype)[][] bots, int[] maxspend) GetCosts(string line)
{
    var split = line.Split(new char[] { ':', '.' }, StringSplitOptions.RemoveEmptyEntries);
    var blueprint = split[0].Split();
    int id = int.Parse(blueprint[1]);

    (int ramt, int rtype)[][] bots = new (int ramt, int rtype)[4][];
    int[] maxspend = new int[3] { 0, 0, 0 };

    string[] bsplit = split[1..];
    for (int b = 0; b < bsplit.Length; b++)
    {
        string r = bsplit[b];
        var rsplit = r.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(t => t != "and").ToArray();

        string type = rsplit[1];
        var costs = rsplit[4..].Chunk(2).Select(t => (ramt: int.Parse(t[0]), rtype: Array.IndexOf(resources, t[1]))).ToArray();
        bots[b] = costs;
        foreach (var c in costs)
        {
            maxspend[c.rtype] = Math.Max(maxspend[c.rtype], c.ramt);
        }
    }

    return (bots, maxspend);
}

int Calculate((int ramt, int rtype)[][] bp, int[] maxspend, Dictionary<string, int> cache, int time, int[] bots, int[] amt)
{
    if (time == 0)
        return amt[3];

    var key = $"{time};{string.Join(",", bots)};{string.Join(",", amt)}";
    if (cache.ContainsKey(key)) return cache[key];

    int max = amt[3] + bots[3] * time;

    for (int i = 0; i < bp.Length; i++) // recipes
    {
        if (i != 3 && bots[i] >= maxspend[i])
            continue;

        int wait = 0;
        bool br = false;

        foreach (var res in bp[i])
        {
            if (bots[res.rtype] == 0)
            {
                br = true;
                break;
            }

            wait = Math.Max(wait, (int)Math.Ceiling((double)(res.ramt - amt[res.rtype]) / bots[res.rtype]));
        }

        if (!br)
        {
            var remtime = time - wait - 1;
            if (remtime <= 0) continue;

            var nbots = bots.ToArray();
            var namt = amt.Zip(bots).Select(zip => zip.First + zip.Second * (wait + 1)).ToArray();

            foreach (var res in bp[i])
            {
                namt[res.rtype] -= res.ramt;
            }
            nbots[i] += 1;

            for (int j = 0; j < 3; j++)
            {
                namt[j] = Math.Min(namt[j], maxspend[j] * remtime);
            }

            int x = Calculate(bp, maxspend, cache, remtime, nbots, namt);
            if (x > max)
            {
                //Console.WriteLine(string.Join(",", nbots));
                max = x;
            }
        }
    }

    cache.Add(key, max);
    return max;
}


int NameToIndex(string name) => name switch
{
    "ore" => 0,
    "clay" => 1,
    "obsidian" => 2,
    "geode" => 3,
};

string IndexToName(int index) => index switch
{
    0 => "ore",
    1 => "clay",
    2 => "obsidian",
    3 => "geode",
};