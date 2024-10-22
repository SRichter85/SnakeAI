using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole;

internal class GameStatusView : ConsoleArea
{
    private Snake[] _snakes;
    public GameStatusView(ConsoleManager console, Point topLeft, int width) : base(topLeft, new Size(width, 10))
    {
        Console = console;
        _snakes = console.Game.Snakes.Snakes.ToArray();
    }

    public ConsoleManager Console { get; }

    public override void Refresh()
    {
        for (int idx = 0; idx < _snakes.Length; idx++)
        {
            WriteLine(idx, Theme.Default, true, $"Schlange {idx}     : {_snakes[idx].TailLength,15}");
        }

        WriteLine(_snakes.Length + 1, Theme.Default, true, $"Futteranzahl   : {Console.Game.FoodCount,15}");

        WriteLine(_snakes.Length + 2, Theme.Default, true, $"Gui Frames     : {Console.FrameCount,15}");
        WriteLine(_snakes.Length + 3, Theme.Default, true, $"GUI Frames/sec : {Console.MeasuredFramesPerSecond,15:n2}");
        WriteLine(_snakes.Length + 4, Theme.Default, true, $"Game Frames    : {Console.Game.FrameCount,15}");
        WriteLine(_snakes.Length + 5, Theme.Default, true, $"Game Frames/sec: {Console.Game.MeasuredFramesPerSecond,15:n2}");

        base.Refresh();
    }
}
