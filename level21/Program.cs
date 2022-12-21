string[] lines = File.ReadAllLines("level21.in");

int count = 0;

List<(string var, string a, string op, string b)> operations = new();
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

        operations.Add((name, a, op, b));
    }
}

while (operations.Any())
{
    var op = operations.First();
    long res = GetValue(op.var);

    if (op.var == "root")
    {
        Console.WriteLine(res);
        return;
    }
}



long GetValue(string variable)
{
    if (register.ContainsKey(variable)) return register[variable];
    else
    {
        var op = operations.FirstOrDefault(t => t.var == variable);
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

        operations.Remove(op);
        register.Add(variable, result);
        return result;
    }
}