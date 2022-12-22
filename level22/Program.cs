using System.Security.Cryptography.X509Certificates;

string input = File.ReadAllText("level22.in");

var split = input.Split("\r\n\r\n");
var part1 = split[0].Split("\r\n");

int width = part1.Max(x => x.Length);

List<(int x, int y, char ch)> map = new();

for (int y = 1; y <= part1.Length; y++)
{
    string line = part1[y - 1];
    for (int x = 1; x <= width; x++)
    {
        if (line.Length <= x - 1) map.Add((x, y, ' '));
        else map.Add((x, y, line[x - 1]));
    }
    Console.WriteLine();
}

string[] instructions = split[1].Replace("R", ";R;").Replace("L", ";L;").Split(";");


Dictionary<int, (int x1, int y1, int x2, int y2)> blocks = new()
{
    { 1, (x1: 51, y1: 1, x2: 100, y2: 50) },
    { 2, (x1: 101, y1: 1, x2: 150, y2: 50) },
    { 3, (x1: 51, y1: 51, x2: 100, y2: 100) },
    { 4, (x1: 1, y1: 101, x2: 50, y2: 150) },
    { 5, (x1: 51, y1: 101, x2: 100, y2: 150) },
    { 6, (x1: 1, y1: 151, x2: 50, y2: 200) },
};

int direction = 0;
var start = map.OrderBy(t => t.y).ThenBy(t => t.x).Where(t => t.ch == '.').First();
Console.WriteLine(start);
var cur = (start.x, start.y);

/*
wrong
172120
73233
152057
165175
 * */

foreach (var inst in instructions)
{
    if (int.TryParse(inst, out var count))
    {
        for (int i = 0; i < count; i++)
        {
            var move = Move(cur.x, cur.y, direction);
            var next = map.Where(t => t.x == move.x && t.y == move.y).FirstOrDefault();

            if (next == default || next.ch == ' ')
            {
                Console.WriteLine(inst);
                var pos = GetNewPosition(cur.x, cur.y, direction);
                Console.WriteLine($"{(cur, direction)} -> {pos}");
                next = map.FirstOrDefault(p => p.x == pos.x && p.y == pos.y);
                if (next.ch == '#') break;
                if (next.ch == '.')
                {
                    cur = (pos.x, pos.y);
                    direction = pos.direction;
                }
            }
            else if (next.ch == '#')
            {
                //Console.WriteLine("WALL");
                break;
            }
            else if (next.ch == '.')
            {
                //Console.WriteLine((move,direction));
                cur = move;
            }
        }
    }
    else
    {
        Console.WriteLine("TURN " + inst);
        var turn = inst switch
        {
            "R" => direction + 1,
            "L" => direction - 1,
            _ => 0,
        };

        if (turn < 0) direction = 3;
        else if (turn > 3) direction = 0;
        else direction = turn;
    }
}

Console.WriteLine($"1000 * {cur.y} + 4 * {cur.x} + {direction} = {1000 * cur.y + 4 * cur.x + direction}");

(int x, int y) Move(int x, int y, int direction)
{
    return direction switch
    {
        0 => (x + 1, y),
        1 => (x, y + 1),
        2 => (x - 1, y),
        3 => (x, y - 1),
    };
}

(int x, int y, int direction) GetNewPosition(int x, int y, int direction)
{
    var block = blocks.FirstOrDefault(b => x >= b.Value.x1 && x <= b.Value.x2 && y >= b.Value.y1 && y <= b.Value.y2);

    if (direction == 0) // right
    {
        return block.Key switch
        {
            2 => (x: 100, y: 151-y, 2),
            3 => (x: y, 50, 3),
            5 => (x: 150, y: 151-y, 2),
            6 => (x: y-100, y: 150, 3)
        };
    }
    else if (direction == 1) // down
    {
        return block.Key switch
        {
            2 => (x: 100, y: x, 2),
            5 => (x: 50, y: x+100, 2),
            6 => (x: 51 + x, y: 1, 1)
        };
    }
    else if (direction == 2) // left
    {
        return block.Key switch
        {
            1 => (x: 1, y: 151-y, 0),
            3 => (x: y-50, y: 101, 1),
            4 => (x: 51, y: 151-y,0),
            6 => (x: y-100, y: 1, 1)
        };
    }
    else if (direction == 3) // up
    {
        return block.Key switch
        {
            1 => (x: 1, y: x+100, 0),
            2 => (x: x-100, y: 200, 3),
            4 => (x: 51, y: x+50, 0)
        };
    }

    return (x, y, direction);
}