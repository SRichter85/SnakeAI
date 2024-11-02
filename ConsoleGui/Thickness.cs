namespace ConsoleGui;

public struct Thickness
{
    private int _top;
    private int _left;
    private int _bottom;
    private int _right;
    public Thickness(int top, int left, int bottom, int right)
    {
        _top = top;
        _left = left;
        _bottom = bottom;
        _right = right;
    }

    public Thickness(int vertical, int horizontal) : this(vertical, horizontal, vertical, horizontal) { }

    public Thickness(int around) : this(around, around) { }

    public int Top { get { return _top; } }

    public int Left { get { return _left; } }

    public int Bottom { get { return _bottom; } }

    public int Right { get { return _right; } }

    public override string ToString()
    {
        return $"[{_top}, {_right}, {_bottom}, {_left}]";
    }
}
