using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SnakeAIConsole;
public class ConsoleArea {
    private struct Run {
        public int X;
        public int Y;
        public string T;
        public ThemeItem C;
    }

    private readonly Queue<Run> _runs = new Queue<Run>();

    public ConsoleArea(int x, int y, int width, int height) : this(new Point(x,y), new Size(width,height))
    {  }

    public ConsoleArea(Point topLeft, Size size) {
        TopLeft = topLeft;
        Size = size;
        BottomRight = new Point {
            X = TopLeft.X + Size.Width,
            Y = TopLeft.Y + Size.Height
        };
    }

    public Point TopLeft { get; }
    public Size Size { get; }

    public Point BottomRight { get; }

    public void FillBackground(ThemeItem color) {
        Write(0, 0, color, new string(' ', Size.Width * Size.Height));
    }

    /// <summary>
    /// Writes a single line without wordwrap (but fills the line with empty spaces if text is shorter)
    /// </summary>
    /// <param name="line"></param>
    /// <param name="color"></param>
    /// <param name="alignRight"></param>
    /// <param name="text"></param>
    public void WriteLine(int line, ThemeItem color, bool alignRight, string text)
    {
        if (line >= Size.Height) return;
        if (text.Length > Size.Width)
        {
            text = text.Substring(0, Size.Width);
        }
        else if (text.Length < Size.Width)
        {
            var emptySpaces = new string(' ', Size.Width - text.Length);
            text = alignRight ? emptySpaces + text : text + emptySpaces;
        }

        Write(0, line, color, text);
    }

    /// <summary>
    /// Writes text into a line and fills the complete line
    /// </summary>
    /// <param name="line"></param>
    /// <param name="color"></param>
    /// <param name="text"></param>
    public void WriteLine(int line, ThemeItem color, string text) => WriteLine(line, color, false, text);

    public void Write(int x, int y, ThemeItem color, char c) {
        if (x >= Size.Width || y >= Size.Height) return; // do not write if position is out of area
        _runs.Enqueue(new Run { X = x, Y = y, T = c.ToString(), C = color });
    }

    public void Write(int x, int y, ThemeItem color, string text) {
        if (x > Size.Width || y > Size.Height) return; // do not write if position is out of area

        // split text in chunks of right size
        int maxLength = Size.Width - x;
        if (text.Length > maxLength) {
            Write(x, y, color, text.Substring(0, maxLength));
            Write(0, y + 1, color, text.Substring(maxLength));
        } else {
            var r = new Run { X = x, Y = y, T = text, C = color };
            _runs.Enqueue(r);
        }
    }

    public virtual void Refresh() {
        while (_runs.Any()) {
            var r = _runs.Dequeue();
            Console.ForegroundColor = r.C.Foreground;
            Console.BackgroundColor = r.C.Background;
            Console.SetCursorPosition(r.X + TopLeft.X, r.Y + TopLeft.Y);
            Console.Write(r.T);
        }
    }
}
