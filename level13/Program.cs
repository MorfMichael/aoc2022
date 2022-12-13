using System.Text;
using System.Text.RegularExpressions;

List<(List<object> left, List<object> right, int Index)> packets = File.ReadAllText("level13.in").Split("\r\n\r\n").Select((t, i) => (Split: t.Split("\r\n"), Index: i + 1)).Select(t => (left: Parse(t.Split[0]), right: Parse(t.Split[1]), Index: t.Index)).ToList();

int count = 0;
foreach (var packet in packets)
{
    bool? compare = Compare(packet.left, packet.right);
    if (compare.HasValue && compare.Value) count += packet.Index;

    Console.ForegroundColor = compare ?? false ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(packet.Index);

    Console.ResetColor();
    Console.WriteLine(Print(packet.left));
    Console.WriteLine(Print(packet.right));
    //Console.ReadKey();
}
Console.WriteLine(count);

bool? Compare(List<object> left, List<object> right)
{
    for (int i = 0; i < left.Count; i++)
    {
        if (i >= right.Count) return false;

        int? l = left[i] as int?;
        int? r = right[i] as int?;
        List<object> ll = left[i] as List<object>;
        List<object> rr = right[i] as List<object>;

        if (l.HasValue && r.HasValue)
        {
            Console.WriteLine("compare " + l + " vs " + r);
            if (l.Value > r.Value) return false;
            if (l.Value < r.Value) return true;
        }

        if (ll != null && rr != null)
        {
            Console.WriteLine("compare " + Print(ll) + " vs " + Print(rr));
            var a = Compare(ll, rr);
            if (a.HasValue) return a.Value;
        }

        if (ll != null && r.HasValue)
        {
            Console.WriteLine("compare " + Print(ll) + " vs " + r);
            var b = Compare(ll, new() { r.Value });
            if (b.HasValue) return b.Value;
        }

        if (l.HasValue && rr != null)
        {
            Console.WriteLine("compare " + l + " vs " + Print(rr));
            var c = Compare(new() { l }, rr);
            if (c.HasValue) return c.Value;
        }
    }

    if (left.Count < right.Count) return true;

    return null;
}

string Print(List<object> list)
{
    List<string> result = new();

    foreach (var item in list)
    {
        if (item is int v) result.Add(v.ToString());
        else if (item is List<object> l) result.Add(Print(l));
    }

    return "[" + string.Join(",", result) + "]";
}

List<object> Parse(string input)
{
    Stack<List<object>> start = new();
    List<object> current = null;
    foreach (var s in input.Split(','))
    {
        string edit = s;
        while (edit.StartsWith("["))
        {
            if (current != null)
                start.Push(current);
            current = new List<object>();
            edit = edit[1..];
        }

        if (int.TryParse(edit, out var integer)) current.Add(integer);
        else
        {
            int i = 0;
            string number = string.Empty;
            while (edit.Any() && char.IsDigit(edit[0]))
            {
                number += edit[0];
                edit = edit[1..];
            }
            if (number.Length > 0)
                current.Add(int.Parse(number));

            while (edit.Length > 0 && edit[0] == ']')
            {
                if (start.Any())
                {
                    var prev = current;
                    current = start.Pop();
                    current.Add(prev);
                }
                edit = edit[1..];
            }
        }
    }

    return current;
}