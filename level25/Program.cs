using System.Security.Cryptography.X509Certificates;
using System.Text;

string[] lines = File.ReadAllLines("level25.in");

char[] characters = new char[5] { '0', '1', '-', '=', '2' };
long sum = 0;

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    sum += Calculate(line);
}


//36692419278947
Console.WriteLine(sum);
Console.WriteLine(Reverse(sum));

long Calculate(string line)
{
    long cur = 0;

    for (int i = 0; i < line.Length; i++)
    {
        int power = line.Length - 1 - i;
        long pow = (long)Math.Pow(5, power);

        cur += line[i] switch
        {
            '2' => 2 * pow,
            '1' => pow,
            '0' => 0,
            '-' => -pow,
            '=' => -2 * pow,
        };
    }

    return cur;
}

string Reverse(long number)
{
    int size = 0;

    while (Math.Pow(5, size) * 2 < number) size++;

    string one = "1" + new string('0', size);
    string two = "2" + new string('0', size);

    List<(string value, long number, long diff, int next)> possible = new()
    {
        (one, Calculate(one), Math.Abs(Calculate(one)-number), size-1),
        (two, Calculate(two), Math.Abs(Calculate(two)-number), size-1),
    };

    while (possible.Any())
    {
        var cur = possible.OrderBy(t => t.diff).FirstOrDefault();
        possible.Remove(cur);

        if (cur.number == number)
        {
            return cur.value;
        }

        if (cur.next <= 0) continue;

        foreach (var ch in characters)
        {
            StringBuilder sb = new StringBuilder(cur.value);
            sb[cur.value.Length - cur.next] = ch;

            string value = sb.ToString();
            long no = Calculate(value);
            long diff = Math.Abs(no - number);

            possible.Add((value, no, diff, cur.next - 1));
        }
    }

    return "failed";
}