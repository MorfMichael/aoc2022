using System.Text;
using System.Text.RegularExpressions;

List<(List<object> left, List<object> right, int Index)> packets = File.ReadAllText("sample.in").Split("\r\n\r\n").Select((t, i) => (Split: t.Split("\r\n"), Index: i + 1)).Select(t => (left: Parse(t.Split[0]), right: Parse(t.Split[1]), Index: t.Index)).ToList();

int count = 0;
foreach (var packet in packets)
{
    if (Compare(packet.left, packet.right)) count++;
}

bool Compare(List<object> left, List<object> right)
{
    for (int i = 0; i < left.Count; i++)
    {
        if (i > right.Count) return false;

        if (left[i].GetType() == typeof(int) && right[i].GetType() == typeof(int))
        {
        }
    }

    return true;
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