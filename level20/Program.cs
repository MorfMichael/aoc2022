using System.Security.Cryptography.X509Certificates;

string[] lines = File.ReadAllLines("level20.in");

List<Entry> entries = new();
List<Entry> move = new();
for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    int value = int.Parse(line);

    var entry = new Entry(value);
    entries.Add(entry);
    move.Add(entry);
}

Console.WriteLine(string.Join(",", move));

foreach (var entry in entries)
{
    int curIdx = move.IndexOf(entry);
    int newIdx = curIdx + entry.Value;

    while (newIdx >= lines.Length) newIdx -= lines.Length - 1;
    while (newIdx < 0) newIdx = lines.Length - 1 + newIdx;

    if (newIdx == 0) newIdx = lines.Length - 1;

    move.Remove(entry);
    move.Insert(newIdx, entry);

    //Console.WriteLine(string.Join(",", move.Select(t => t.Value)));
    //Console.ReadKey();
}

var zero = move.FirstOrDefault(t => t.IsZero);
int a = (move.IndexOf(zero)+1000) % lines.Length;
int b = (move.IndexOf(zero)+2000) % lines.Length;
int c = (move.IndexOf(zero)+3000) % lines.Length;

Console.WriteLine(move[a]);
Console.WriteLine(move[b]);
Console.WriteLine(move[c]);

int sum = move[a].Value + move[b].Value + move[c].Value;
Console.WriteLine(sum);


class Entry
{
    public Entry(int value)
    {
        Value = value;
    }

    public int Value { get; set; }

    public bool IsZero => Value == 0;

    public override string ToString() => Value.ToString();
}