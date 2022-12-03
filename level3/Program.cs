string[] input = File.ReadAllLines("level3.in");


List<char> shared = new();

foreach (var line in input)
{
    int split = line.Length / 2;
    string left = line[..split];
    string right = line[split..];

    var s = left.Where(right.Contains).Distinct().ToList();
    shared.AddRange(s);
}

int count = 0;
foreach (var c in shared)
{
    int value = char.IsLower(c) ? (int)c - 96 : (int)c - 64 + 26;
    Console.WriteLine($"{c}: {value}");
    count += value;
}

Console.WriteLine(count);