using System.Runtime.CompilerServices;

string[] lines = File.ReadAllLines("level21.in");

int count = 0;
long humn = 0;

Dictionary<string, (string a, string op, string b)> operations = new();
Dictionary<string, long> register = new();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    var split = line.Split();
    string name = split[0][..^1];

    if (split.Length == 2)
    {
        register.Add(name, int.Parse(split[1]));
    }
    else
    {
        string a = split[1];
        string op = split[2];
        string b = split[3];

        operations.Add(name, (a, op, b));
    }
}


var root = operations["root"];

var parents = GetParents("humn").ToList();

for (long ii = 3_712_643_500_000; ii < 4_000_000_000_000; ii++)
{
    //long ii = 370_000_0_000_000;
    register["humn"] = ii;

    long aa = GetValue(root.a);
    long bb = GetValue(root.b);

    Console.WriteLine($"{aa} == {bb}");
    if (GetValue(root.a) == GetValue(root.b))
    {
        Console.WriteLine(ii);
        return;
    }
}

IEnumerable<string> GetParents(string variable)
{
    yield return variable;

    var found = operations.Where(x => x.Value.a == variable || x.Value.b == variable).SelectMany(t => GetParents(t.Key)).ToList();
    foreach (var f in found)
    {
        yield return f;
    }
}


long GetValue(string variable)
{
    if (register.ContainsKey(variable)) return register[variable];
    else
    {
        var op = operations[variable];

        var a = GetValue(op.a);
        var b = GetValue(op.b);

        long result = op.op switch
        {
            "+" => a + b,
            "-" => a - b,
            "/" => a / b,
            "*" => a * b,
            _ => 0
        };

        if (!parents.Contains(variable))
        {
            operations.Remove(variable);
            register.Add(variable, result);
        }

        return result;
    }
}