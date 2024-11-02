using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_ConsolePrototypes;

public struct ConsoleFrameChild
{
    public int X;
    public int Y;
    public ConsoleFrame F;
}

public class ConsoleFrame
{
    private ConsolePixelStack[] _pixels;

    private char[] _data;

    public ConsoleFrame(int width, int height)
    {
        _data = new char[width * height];
        _pixels = new ConsolePixelStack[width * height];
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                _pixels[h * width + w] = new ConsolePixelStack(w, h);
            }
        }

        Width = width;
        Height = height;
    }

    private List<ConsoleFrameChild> Children { get; } = new List<ConsoleFrameChild>();

    public int Width { get; }
    public int Height { get; }

    public void AddChild(ConsoleFrame child, int x, int y)
    {
        Children.Add(new ConsoleFrameChild { X = x, Y = y, F = child });
    }

    public void Refresh()
    {
        Children.ForEach(x => x.F.Refresh());
        Console.SetCursorPosition(0, 0);
        foreach (var p in _pixels.Where(x => x.HasChanged))
        {
            p.HasChanged = false;
            Console.SetCursorPosition(p.X, p.Y);
            if(p.Value != 0) Console.Write(p.Value);
        }
    }

    public void Write(int x, int y, string text)
    {
        var idx = y * Width + x;
        foreach (var c in text)
        {
            _pixels[idx++].Set(c);
        }
    }
}

public class ConsolePixelStack
{
    private Stack<ConsoleFrame> _frames = new Stack<ConsoleFrame>();

    private char _buffer = '\0';

    public ConsolePixelStack(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public IEnumerable<ConsoleFrame> Frames { get { return _frames; } }

    public void AppendFrame(ConsoleFrame frame) => _frames.Push(frame);

    public char Value { get; private set; } = '\0';

    public bool HasChanged { get; set; } = false;

    public void Set(char value)
    {
        if(Value != value)
            { Value = value;
            HasChanged = true;
        }
    }
}
