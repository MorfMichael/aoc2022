string[] lines = File.ReadAllLines("level8.in");

List < (int row, int column, int score)> scores = new();

for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];
    if (string.IsNullOrWhiteSpace(line)) continue;

    for (int j = 0; j < line.Length; j++)
    {
        int tree = int.Parse(line[j].ToString());

        if (i == 0 || j == 0 || i == lines.Length - 1 || j == line.Length - 1) continue;
        else
        {
            int left = 0;
            int x = j-1;
            while (x >= 0 && int.Parse(line[x].ToString()) < tree)
            {
                left++;
                x--;
            }
            if (x > 0) left++;
            
            int right = 0;
            x = j+1;
            while (x < line.Length && int.Parse(line[x].ToString()) < tree)
            {
                right++;
                x++;
            }
            if (x < line.Length) right++;

            int top= 0;
            x = i-1;
            while (x >= 0 && int.Parse(lines[x][j].ToString()) < tree)
            {
                top++;
                x--;
            }
            if (x > 0) top++;


            int bottom = 0;
            x = i+1;
            while (x < lines.Length && int.Parse(lines[x][j].ToString()) < tree)
            {
                bottom++;
                x++;
            }
            if (x < lines.Length) bottom++;

            scores.Add((i,j, left*right*top*bottom));
        }
    }
}



Console.WriteLine(scores.Max(x => x.score));
