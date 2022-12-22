using System.Security.Cryptography.X509Certificates;

string input = File.ReadAllText("level22.in");

var split = input.Split("\r\n\r\n");
var part1 = split[0].Split("\r\n");

int width = part1.Max(x => x.Length);

List<(int x, int y, char ch)> map = new();

for (int y = 1; y <= part1.Length; y++)
{
    string line = part1[y-1];
    for (int x = 1; x <= width; x++)
    {
        if (line.Length <= x-1) map.Add((x,y,' '));
        else map.Add((x, y, line[x-1]));
    }
    Console.WriteLine();
}

string[] instructions = split[1].Replace("R", ";R;").Replace("L", ";L;").Split(";");

int direction = 0;
var start = map.OrderBy(t => t.y).ThenBy(t => t.x).Where(t => t.ch == '.').First();
var cur = (start.x, start.y);

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
                if (direction == 0)
                {
                    var n = map.Where(t => t.y == cur.y).OrderBy(t => t.x).FirstOrDefault(t => t.ch != ' ');
                    if (n.ch == '#') break;
                    else if (n.ch == '.') cur = (n.x, n.y);
                }
                else if (direction == 1)
                {
                    var n = map.Where(t => t.x == cur.x).OrderBy(t => t.y).FirstOrDefault(t => t.ch != ' ');
                    if (n.ch == '#') break;
                    else if (n.ch == '.') cur = (n.x, n.y);
                }
                else if (direction == 2)
                {
                    var n = map.Where(t => t.y == cur.y).OrderByDescending(t => t.x).FirstOrDefault(t => t.ch != ' ');
                    if (n.ch == '#') break;
                    else if (n.ch == '.') cur = (n.x, n.y);
                }
                else if (direction == 3)
                {
                    var n = map.Where(t => t.x == cur.x).OrderByDescending(t => t.y).FirstOrDefault(t => t.ch != ' ');
                    if (n.ch == '#') break;
                    else if (n.ch == '.') cur = (n.x, n.y);
                }
            }
            else if (next.ch == '#') break;
            else if (next.ch == '.') cur = move;
            
        }
    }
    else
    {
        var turn = inst switch
        {
            "R" => direction+1,
            "L" => direction-1,
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
        2 => (x-1,y),
        3 => (x, y-1),
    };
}