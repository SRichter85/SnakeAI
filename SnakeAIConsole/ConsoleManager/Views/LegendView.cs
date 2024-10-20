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
        public LegendView(Point topLeft, int width) : base(topLeft, new Size(width, 7))
        {
            FillBackground(Theme.MenuItem);
            Write(0, 1, Theme.MenuItem, new string(' ', width/2 - 5) + "SNAKE A.I.");
            Write(0, 3, Theme.MenuItem, " [A]/[D]: Steuerung der Schlange");
            Write(0, 4, Theme.MenuItem, " [W]/[S]: Steuerung des Menüs");
            Write(0, 5, Theme.MenuItem, " [Enter]: Menü betätigen");
        }
    }
}
