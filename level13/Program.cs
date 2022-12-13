using System.Text;
using System.Text.RegularExpressions;

List<(string[] Packets, int Index)> packets = File.ReadAllText("sample.in").Split("\r\n\r\n").Select((t, i) => (Packets: t.Split("\r\n"), Index: i + 1)).ToList();

Console.WriteLine(packets.Count);


string value = "[1,[2,[3,[4,[5,6,0]]]],8,9]";

Stack<List<object>> start = new();
List<object> current = null;
foreach (var s in value.Split(','))
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