string text = File.ReadAllText("level6.in");

int count = 0;

//int start = 4; // part1
int start = 14; // part2

for (int i = 0; i < text.Length; i++)
{
    count++;
    if (i >= start)
    {
        string part = text[(i - start)..i];
        if (part.Distinct().Count() == part.Length) break;
    }
}


Console.WriteLine(count-1);