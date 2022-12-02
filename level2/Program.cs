using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

string[] input = File.ReadAllLines("level2.in");

int score = 0;

List<(string a, string b, int score)> test = new()
{
    ("A", "X", 3),
    ("B", "X", 1),
    ("C", "X", 2),
    ("A", "Y", 4),
    ("B", "Y", 5),
    ("C", "Y", 6),
    ("A", "Z", 8),
    ("B", "Z", 9),
    ("C", "Z", 7),
};

foreach (var line in input)
{
    var split = line.Split(" ");
    var a = split[0];
    var b = split[1];

    int s = test.FirstOrDefault(x => x.a == a && x.b == b).score;

    if (s == 0)
    {
        Console.WriteLine("wrong");
    }
    score += s;
}

Console.WriteLine(score);

// A = Rock
// B = Paper
// C = Siccors

// x loose
// y draw
// z win

//X = Rock 1
// Y = Paper 2
// Z = Siccsors 3

