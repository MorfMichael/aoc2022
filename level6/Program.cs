string[] lines = File.ReadAllLines("level6.in");

int count = 0;

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    /* write your code here */

    count++;
}


Console.WriteLine(count);