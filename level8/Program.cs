string[] lines = File.ReadAllLines("level8.in");

int count = 0;

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];
    if (string.IsNullOrWhiteSpace(line)) continue;

    for (int j = 0; j < line.Length; j++)
    {
        int tree = int.Parse(line[j].ToString());

        if (i == 0 || j == 0 || i == lines.Length - 1 || j == line.Length - 1) count++;
        else
        {
            var left = line[0..j].Select(x => int.Parse(x.ToString())).ToArray();
            var right = line[(j+1)..].Select(x => int.Parse(x.ToString())).ToArray();
            var top = lines[0..i].Select(x => int.Parse(x[j].ToString())).ToArray();
            var bottom = lines[(i+1)..].Select(x => int.Parse(x[j].ToString())).ToArray();

            if (left.All(x => x < tree) || right.All(x => x < tree) || top.All(x => x < tree) || bottom.All(x => x < tree)) count++;
        }
    }
}

Console.WriteLine(count);
