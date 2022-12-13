using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

List<(List<object> Packet, int Index, bool Divider)> packets = File.ReadAllText("level13.in").Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select((t, i) => (Packet: Parse(t), Index: i + 1, Divider: false)).ToList();

packets.Add((Parse("[[2]]"), packets.Count, true));
packets.Add((Parse("[[6]]"), packets.Count, true));

var divider = packets.OrderBy(t => t.Packet, new PacketComparer()).Select((t, i) => (Packet: t, Index: i + 1)).Where(t => t.Packet.Divider).ToList();
Console.WriteLine(divider[0].Index * divider[1].Index);

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

class PacketComparer : IComparer<List<object>>
{
    public int Compare(List<object> x, List<object> y) => LocalCompare(x, y);
    int LocalCompare(List<object> left, List<object> right)
    {
        for (int i = 0; i < left.Count; i++)
        {
            if (i >= right.Count) return 1;

            int? l = left[i] as int?;
            int? r = right[i] as int?;
            List<object> ll = left[i] as List<object>;
            List<object> rr = right[i] as List<object>;

            if (l.HasValue && r.HasValue)
            {
                //Console.WriteLine("compare " + l + " vs " + r);
                if (l.Value > r.Value) return 1;
                if (l.Value < r.Value) return -1;
            }

            if (ll != null && rr != null)
            {
                //Console.WriteLine("compare " + Print(ll) + " vs " + Print(rr));
                var a = Compare(ll, rr);
                if (a != 0) return a;
            }

            if (ll != null && r.HasValue)
            {
                //Console.WriteLine("compare " + Print(ll) + " vs " + r);
                var b = Compare(ll, new() { r.Value });
                if (b != 0) return b;
            }

            if (l.HasValue && rr != null)
            {
                //Console.WriteLine("compare " + l + " vs " + Print(rr));
                var c = Compare(new() { l }, rr);
                if (c != 0) return c;
            }
        }

        if (left.Count < right.Count) return -1;

        return 0;
    }
}