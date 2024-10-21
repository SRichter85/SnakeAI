using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole;

public class HighscoreView : ConsoleArea
{
    private List<Highscore> _highscores = new List<Highscore>();
    private bool dataChanged = true;

    public HighscoreView(SnakeAiConfiguration configuration, int x, int y, int width) : base(x, y, width, 14)
    {
        Configuration = configuration;

        Write(0, 0, Theme.Default, "--------------");
        Write(0, 1, Theme.Default, "| HIGHSCORES |");
        Write(0, 2, Theme.Default, "--------------");
        Write(0, 3, Theme.Default, $"{"PL.",3} {"NAME",-14}  {"DATUM",-16}{"LÄNGE",6}{"DAUER",8}");

        SetData(Configuration.Settings.Highscores);
    }

    public SnakeAiConfiguration Configuration { get; }

    public void SetData(IEnumerable<Highscore> highscores)
    {
        _highscores = new List<Highscore>(highscores); // copy for thread safety
        dataChanged = true;
    }

    public override void Refresh()
    {
        if (dataChanged)
        {
            dataChanged = false;
            var list = _highscores; // copy reference for thread safety
            for (int idx = 0; idx < Math.Min(10, list.Count); idx++)
            {
                var d = list[idx];
                Write(0, idx + 4, Theme.Default, $"{idx + 1 + ".",3} {d.UserName,-14}  {d.Date,16:dd.MM.yy hh.mm}{d.SnakeLength,6}{d.FrameCount,8}");
            }
        }


        base.Refresh();

    }
}
