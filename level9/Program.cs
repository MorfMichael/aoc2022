string[] lines = File.ReadAllLines("level9.in");

int count = 0;

Rope rope = new Rope();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (string.IsNullOrWhiteSpace(line)) continue;

    var split = line.Split();
    rope.Move(split[0], int.Parse(split[1]));
}

Console.WriteLine(rope.Result);

class Rope
{
    private List<(int X, int Y)> _tail = new();
    private (int X, int Y) _start;

    public Rope()
    {
        _start = (0, 0);
        Head = new Knot(0, 0, 0);
        Tail = new Knot(1, 0, 0);
        _tail.Add((0, 0));
    }

    public Knot Tail { get; set; }
    public Knot Head { get; set; }

    public int Result => _tail.Count;

    public void Move(string direction, int count)
    {
        for (int i = 0; i < count; i++)
        {
            OneStep(direction);
        }
    }

    public void OneStep(string direction)
    {
        switch (direction)
        {
            case "L":
                Head.X--;
                break;
            case "R":
                Head.X++;
                break;
            case "U":
                Head.Y--;
                break;
            case "D":
                Head.Y++;
                break;
        }

        MoveTail(Tail, Head, true);
    }

    public void MoveTail(Knot Tail, Knot Head, bool add)
    {
        if (Head.X >= Tail.X - 1 && Head.X <= Tail.X + 1 && Head.Y >= Tail.Y - 1 && Head.Y <= Tail.Y + 1) return;

        if ((Head.X == Tail.X + 1 && Head.Y == Tail.Y - 2) ||
            (Head.X == Tail.X + 2 && Head.Y == Tail.Y - 1) ||
            (Head.X == Tail.X + 2 && Head.Y == Tail.Y - 2))
        {
            Tail.X++;
            Tail.Y--;
        }

        if (Head.X == Tail.X + 2 && Head.Y == Tail.Y)
            Tail.X++;

        if ((Head.X == Tail.X + 2 && Head.Y == Tail.Y + 1) ||
            (Head.X == Tail.X + 1 && Head.Y == Tail.Y + 2) ||
            (Head.X == Tail.X + 2 && Head.Y == Tail.Y + 2))
        {
            Tail.X++;
            Tail.Y++;
        }

        if (Head.X == Tail.X && Head.Y == Tail.Y + 2)
        {
            Tail.Y++;
        }

        if ((Head.X == Tail.X - 1 && Head.Y == Tail.Y + 2) ||
            (Head.X == Tail.X - 2 && Head.Y == Tail.Y + 1) ||
            (Head.X == Tail.X - 2 && Head.Y == Tail.Y + 2))
        {
            Tail.X--;
            Tail.Y++;
        }

        if (Head.X == Tail.X - 2 && Head.Y == Tail.Y)
        {
            Tail.X--;
        }

        if ((Head.X == Tail.X - 2 && Head.Y == Tail.Y - 1) ||
            (Head.X == Tail.X - 1 && Head.Y == Tail.Y - 2) ||
            (Head.X == Tail.X - 2 && Head.Y == Tail.Y - 2))
        {
            Tail.X--;
            Tail.Y--;
        }

        if (Head.X == Tail.X && Head.Y == Tail.Y - 2)
        {
            Tail.Y--;
        }

        if (add && !_tail.Contains((Tail.X, Tail.Y)))
            _tail.Add((Tail.X, Tail.Y));
    }
}

class Knot
{
    public Knot(int id, int x, int y)
    {
        Id = id;
        X = x;
        Y = y;
    }

    public int Id { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString() => $"{X},{Y}";
}