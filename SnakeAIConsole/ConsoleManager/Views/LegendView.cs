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
        public LegendView(Point topLeft, Size size) : base(topLeft, size)
        {

            FillBackground(Theme.MenuItem);
            Write(0, 0, Theme.MenuItem, "         LEGENDE: ");
            Write(0, 1, Theme.MenuItem, "[A]/[D]: Steuerung der Schlange");
            Write(0, 2, Theme.MenuItem, "[W]/[S]: Steuerung des Menüs");
            Write(0, 3, Theme.MenuItem, "[Enter]: Menü betätigen");
            Write(0, 5, Theme.MenuItem, "Für eine optimale Darstellung ist es empfohlen, als Schriftart 'Raster Fonts' auszuwählen mit der Grösse 8x8");

        }
    }
}
