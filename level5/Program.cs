using System.Collections.Generic;

string[] lines = File.ReadAllText("level5.in").Split("\r\n\r\n");

string[] map = lines[0].Split("\r\n");
string[] instructions = lines[1].Split("\r\n");

string[] m = new[]
{
    "BVSNTCHQ",
"WDBG",
"FWRTSQB",
"LGWSZJDN",
"MPDVF",
"FWJ",
"LNQBJV",
"GTRCJQSN",
"JSQCWDM",
};

// parsing after solved solution added
//int cnt = (map.Last().Length+1) / 4;
//string[] m = new string[cnt]; 

//for (int j = 0; j < cnt; j++)
//{
//    int idx = 1 + j * 4;
//    string cur = string.Empty;
//    for (int i = map.Length - 2; i >= 0; i--)
//    {
//        char ch = map[i][idx];
//        if (ch == ' ') continue;
//        cur += ch;
//    }

//    m[j] = cur;
//}

for (int i = 0; i < instructions.Length; i++)
{
    string line = instructions[i];

    if (string.IsNullOrWhiteSpace(line)) continue;
    int[] options = line.Replace("move ", "").Replace(" from ", ",").Replace(" to ", ",").Split(",").Select(int.Parse).ToArray();
    //move 3 from 6 to 2

    int source = options[1] - 1, target = options[2] - 1, count = options[0];
    //part1
    //for (int j = 0; j < count; j++)
    //{
    //    m[target] += m[source][^1];
    //    m[source] = m[source][..^1];
    //}

    //part2
    m[target] += m[source][^count..];
    m[source] = m[source][..^count];
}

Console.WriteLine(string.Join("", m.Select(t => t[^1])));
