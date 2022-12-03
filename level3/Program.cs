string[] input = File.ReadAllLines("level3.in");

List<string[]> groups = input.Chunk(3).ToList();

List<char> shared = new();

foreach (var group in groups)
{
    List<char> chars = group.SelectMany(t => t.ToList()).Distinct().ToList();
    var contains = chars.Where(t => group.All(x => x.Contains(t))).Distinct().ToList();
    shared.AddRange(contains);
}

int count = 0;
foreach (var c in shared)
{
    int value = char.IsLower(c) ? (int)c - 96 : (int)c - 64 + 26;
    Console.WriteLine($"{c}: {value}");
    count += value;
}

Console.WriteLine(count);