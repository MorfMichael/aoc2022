List<Monkey> monkeys = File.ReadAllText("level11.in").Split("\r\n\r\n").Select(t => new Monkey(t.Split("\r\n").ToArray())).ToList();

/*
 Monkey 0:
  Starting items: 62, 92, 50, 63, 62, 93, 73, 50
  Operation: new = old * 7
  Test: divisible by 2
    If true: throw to monkey 7
    If false: throw to monkey 1
 */

for (int i = 0; i < 20; i++)
{
    foreach (var monkey in monkeys)
    {
        monkey.Turn(monkeys);
    }

    foreach (var monkey in monkeys)
    {
        monkey.Print();
    }
    Console.ReadKey();
}

var most = monkeys.OrderByDescending(x => x.Times).Take(2).ToList();
Console.WriteLine(most[0].Times * most[1].Times);



class Monkey
{
    public Monkey(string[] lines)
    {
        string id = lines[0].Replace("Monkey ", "").Replace(":", "");
        Id = int.Parse(id);
        lines[1].Replace("Starting items: ", "").Trim().Split(", ").Select(int.Parse).ToList().ForEach(Items.Enqueue);
        var operationSplit = lines[2].Split();
        Operation = operationSplit[^2];
        OperationValue = int.TryParse(operationSplit[^1], out var opvalue) ? opvalue : null;
        string divisible = lines[3].Split()[^1];
        Divisible = int.Parse(divisible);

        string tmid = lines[4].Split()[^1];
        string fmid = lines[5].Split()[^1];
        TrueMonkeyId = int.Parse(tmid);
        FalseMonkeyId = int.Parse(fmid);
    }

    public int Id { get; set; }
    public Queue<int> Items { get; set; } = new();
    public string Operation { get; set; }
    public int? OperationValue { get; set; }
    public int Divisible { get; set; }
    public int Times { get; set; }

    public int TrueMonkeyId { get; set; }
    public int FalseMonkeyId { get; set; }

    public void Turn(List<Monkey> monkeys)
    {
        while (Items.Any())
        {
            Times++;
            int value = Items.Dequeue();
            int _new = Operation switch
            {
                "/" => value / (OperationValue ?? value),
                "*" => value * (OperationValue ?? value),
                "+" => value + (OperationValue ?? value),
                "-" => value - (OperationValue ?? value),
                _ => value,
            };

            _new = (int)Math.Floor(_new / 3d);

            if (_new % Divisible == 0)
            {
                monkeys.FirstOrDefault(t => t.Id == TrueMonkeyId)?.Items.Enqueue(_new);
            }
            else
            {
                monkeys.FirstOrDefault(t => t.Id == FalseMonkeyId)?.Items.Enqueue(_new);
            }
        }
    }

    public void Print()
    {
        Console.WriteLine($"Monkey {Id}: " + string.Join(", ", Items));
    }
}