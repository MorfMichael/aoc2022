string[] lines = File.ReadAllLines("level10.in");

int x = 1;
int cycle = 1;
Dictionary<int, int> signal = new();
int[] checks = new[] { 20, 60, 100, 140, 180, 220 };
int[] sprite = new[] { 0, 1, 2 };
List<string> crt = new();

string row = "";
for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    var split = line.Split();
    string instruction = split[0];

    if (instruction == "noop")
    {
        cycle++;
        Draw();
    }
    else
    {
        int value = int.Parse(split[1]);
        cycle++;
        Draw();
        cycle++;
        x += value;
        sprite = new[] { x - 1, x, x + 1 };
        Draw();
    }
}

Console.WriteLine(string.Join(Environment.NewLine,crt));

void Draw()
{
    row += sprite.Contains((cycle-1)%40) ? "#" : ".";
    if (row.Length == 40)
    {
        crt.Add(row);
        row = "";
    }
}