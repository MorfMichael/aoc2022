string[] lines = File.ReadAllLines("sample.in");

List<(int x, int y)[]> rocks = new()
{
    new (int x, int y)[] { (0,0), (1,0), (2, 0), (3,0) }, // -
    new (int x, int y)[] { (1,0), (0,1), (1, 1), (2,1) }, // +
    new (int x, int y)[] { (0,0), (1,0), (2, 0), (2,1), (2,2) }, // Lr
    new (int x, int y)[] { (0,0), (0,1), (0, 2), (0,3) }, // I
    new (int x, int y)[] { (0,0), (1,0), (0, 1), (1,1) }, // []
};

HashSet<(int x, int y)> map = new();

List<char> instructions = new List<char>();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    for (int j = 0; j < line.Length; j++)
    {
        instructions.Add(line[j]);
    }
}