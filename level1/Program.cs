List<List<int>> input = File.ReadAllText("level1.in").Split("\r\n\r\n").Select(x => x.Split("\r\n").Select(int.Parse).ToList()).ToList();

Console.WriteLine(input.OrderByDescending(t => t.Sum()).FirstOrDefault().Sum());
Console.WriteLine(input.OrderByDescending(t => t.Sum()).Take(3).Sum(x => x.Sum()));