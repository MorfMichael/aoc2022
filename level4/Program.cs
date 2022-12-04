string[] input = File.ReadAllLines("level4.in");

int count = 0;
foreach (var line in input)
{
    var split = line.Split(",");
    var left = split[0].Split("-").Select(int.Parse).ToArray();
    var right = split[1].Split("-").Select(int.Parse).ToArray();

    if (left[0] >= right[0] && left[1] <= right[1]) count++;
    else if (right[0] >= left[0] && right[1] <= left[1]) count++;
}

Console.WriteLine(count);