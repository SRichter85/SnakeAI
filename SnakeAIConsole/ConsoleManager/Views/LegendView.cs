using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole
{
    public class LegendView : ConsoleArea
    {
        public LegendView(Point topLeft, int width) : base(topLeft, new Size(width, 8))
        {
            FillBackground(Theme.MenuItem);
            Write(0, 1, Theme.MenuItem, new string(' ', width/2 - 5) + "SNAKE A.I.");
            Write(0, 3, Theme.MenuItem, " [A]/[D]: Steuerung der Schlange 1");
            Write(0, 4, Theme.MenuItem, " [I]/[P]: Steuerung der Schlange 2");
            Write(0, 5, Theme.MenuItem, " [W]/[S]: Steuerung des Menüs");
            Write(0, 6, Theme.MenuItem, " [Enter]: Menü betätigen");
        }
    }
}
