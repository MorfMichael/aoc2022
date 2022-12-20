using System.Security.Cryptography.X509Certificates;

string[] lines = File.ReadAllLines("level20.in");

List<Entry> entries = new();
List<Entry> move = new();
for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    long value = long.Parse(line);// * 811589153;

    var entry = new Entry(value);
    entries.Add(entry);
    move.Add(entry);
}

Console.WriteLine(string.Join(",", move));

//for (int i = 0; i < 10; i++)
//{
foreach (var entry in entries)
{
    int curIdx = move.IndexOf(entry);
    long newIdx = curIdx + entry.Value;

    if (newIdx >= lines.Length) newIdx = (newIdx % lines.Length) + 1;
    if (newIdx < 0) newIdx = (lines.Length-1) - Math.Abs(newIdx) % lines.Length;

    if (newIdx == 0) newIdx = lines.Length - 1;

    //Console.WriteLine($"{curIdx} + {entry.Value} = {curIdx + entry.Value} -> {newIdx}");

    move.Remove(entry);
    move.Insert((int)newIdx, entry);

    //Console.WriteLine(string.Join(",", move.Select(t => t.Value)));
    //Console.ReadKey();
}
//}

var zero = move.FirstOrDefault(t => t.IsZero);
int a = (move.IndexOf(zero) + 1000) % lines.Length;
int b = (move.IndexOf(zero) + 2000) % lines.Length;
int c = (move.IndexOf(zero) + 3000) % lines.Length;

Console.WriteLine(a);
Console.WriteLine(b);
Console.WriteLine(c);

Console.WriteLine(move[a]);
Console.WriteLine(move[b]);
Console.WriteLine(move[c]);

long sum = move[a].Value + move[b].Value + move[c].Value;
Console.WriteLine(sum);


class Entry
{
    public Entry(long value)
    {
        Value = value;
    }

    public long Value { get; set; }

    public bool IsZero => Value == 0;

    public override string ToString() => Value.ToString();
}