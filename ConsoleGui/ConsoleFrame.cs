using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui;

public abstract class ConsoleFrame
{
    public ConsoleFrame(SizeOptions sizeOptions)
    {
        SizeOptions = sizeOptions;
    }

    public ConsoleFrame() : this(new SizeOptions())
    { }

    public SizeOptions SizeOptions { get; private set; }

    internal void Initialize()
    {
        OnInitializing();
    }

    internal void Refresh()
    {
        OnRefreshing();
    }

    protected virtual Rect CalculateMinSize()
    {
        return new Rect();
    }

    protected virtual void CalculateMaxSize(Rect availableSpace)
    {
    }

    protected virtual void ArrangeElements()
    {
    }

    protected abstract void OnInitializing();

    protected abstract void OnRefreshing();
}

public struct SizeOptions
{
    public enum HorizontalAlignments
    {
        Left,
        Center,
        Right,
    }

    public enum VerticalAlignments
    {
        Top,
        Center,
        Bottom
    }

    public SizeOptions()
    {
    }

    public Magnitude Width { get; set; } = new Magnitude();

    public Magnitude Height { get; set; } = new Magnitude();

    public Thickness Padding { get; set; } = new Thickness();

    public Thickness Margin { get; set; } = new Thickness();

    public HorizontalAlignments HorizontalAlignment { get; set; } = HorizontalAlignments.Left;

    public VerticalAlignments VerticalAlignment { get; set; } = VerticalAlignments.Top;
}

public struct Magnitude
{
    public enum Modes
    {
        Minimal,
        Absolute,
        Relative
    }

    public Magnitude()
    { }

    public int Value { get; set; } = 0;

    public Modes Mode { get; set; } = Modes.Minimal;

    public static implicit operator Magnitude(int value)
    {
        return new Magnitude() { Value = value, Mode = Modes.Absolute };
    }
}
