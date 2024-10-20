using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole;

internal class GameStatusView : ConsoleArea
{

    public GameStatusView(ConsoleManager console, Point topLeft, int width) : base(topLeft, new Size(width, 6))
    {
        Console = console;

    }

    public ConsoleManager Console { get; }

    public override void Refresh()
    {
        WriteLine(0, Theme.Default, true, $"Schlangenlänge : {Console.Game.Snake.TailLength,15}");
        WriteLine(1, Theme.Default, true, $"Futteranzahl   : {Console.Game.FoodCount,15}");

        WriteLine(2, Theme.Default, true, $"Gui Frames     : {Console.FrameCount,15}");
        WriteLine(3, Theme.Default, true, $"GUI Frames/sec : {Console.MeasuredFramesPerSecond,15:n2}");
        WriteLine(4, Theme.Default, true, $"Game Frames    : {Console.Game.FrameCount,15}");
        WriteLine(5, Theme.Default, true, $"Game Frames/sec: {Console.Game.MeasuredFramesPerSecond,15:n2}");

        base.Refresh();
    }
}
