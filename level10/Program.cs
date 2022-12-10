string[] lines = File.ReadAllLines("level10.in");

int x = 1;
int cycle = 1;
Dictionary<int, int> signal = new();
int[] checks = new[] { 20, 60, 100, 140, 180, 220 };

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    var split = line.Split();
    string instruction = split[0];

    if (instruction == "noop")
    {
        cycle++;
        CycleCheck();
    }
    else
    {
        int value = int.Parse(split[1]);
        cycle++;
        CycleCheck();
        cycle++;
        x += value;
        CycleCheck();
    }
}

Console.WriteLine(signal.Sum(x => x.Value));



void CycleCheck()
{

    if (checks.Contains(cycle))
    {
        int s = cycle * x;
        Console.WriteLine($"{cycle}: {s}");
        Console.ReadKey();
        signal.Add(cycle, s);
    }
}