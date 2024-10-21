using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole;

public class Settings : StorableData<Settings>
{
    public string UserName { get; set; } = "User";

    public int FoodCount { get; set; } = 10;

    public int GameSpeed { get; set; } = 32;

    public short FontSize { get; set; } = 12;

    public List<Highscore> Highscores { get; set; } = new List<Highscore>(101);

    public void AppendHighscore(Snake snake, int frameCount)
    {
        var list = Highscores;
        if (list == null) return;

        var entry = new Highscore
        {
            UserName = UserName,
            Date = DateTime.Now,
            SnakeLength = snake.TailLength,
            FrameCount = frameCount
        };


        for (int idx = 0; idx < list.Count; idx++)
        {
            bool swap = false;
            var compare = list[idx];

            if (entry.SnakeLength > compare.SnakeLength) swap = true;
            else if (entry.SnakeLength == compare.SnakeLength && entry.FrameCount >= compare.FrameCount) swap = true;

            if (swap)
            {
                list[idx] = entry;
                entry = compare;
            }
        }

        if (list.Count < 100) list.Add(entry);
        while (list.Count > 100) list.RemoveAt(100);
    }
}

public class Highscore
{
    public string UserName { get; set; }
    public DateTime Date { get; set; }

    public int SnakeLength { get; set; }

    public int FrameCount { get; set; }
}
