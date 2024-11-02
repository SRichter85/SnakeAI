using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGui;

/// <summary>
/// Represents four coordinates. The interpretation of the values can be different, a Coord4 object could represent a rectangle or the surrounding values for Margin, Padding, etc.
/// </summary>
public struct Rect
{
    private int _left;
    private int _top;
    private int _width;
    private int _height;


    public Rect(int left, int top, int width, int height)
    {
        _left = left;
        _top = top;
        _width = width;
        _height = height;
    }

    public Rect(int width, int height) : this(0,0, width, height) { }

    public Rect() : this(0, 0, 0, 0) { }

    public int Top { get { return _top; } }

    public int Left { get { return _left; } }

    public int Right { get { return _left + _width; } }

    public int Bottom { get { return _top + _height; } }

    public int Width { get { return _width; } }

    public int Height { get { return _height; } }

    /// <summary>
    /// Creates a new rect by wrapping the rect with the thickness values
    /// </summary>
    /// <param name="a"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Rect operator + (Rect a, Thickness t) => new Rect(a.Left - t.Left, a.Top - t.Top, a.Width + t.Left + t.Right, a.Height + t.Top + t.Bottom);

    /// <summary>
    /// Creates a new rect which surrounds both rects. Acts like an 'expanded' union
    /// </summary>
    /// <param name="a"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Rect operator +(Rect a, Rect b)
    {
        Rect retVal = new Rect();
        retVal._left = Math.Min(a._left, b._left);
        retVal._top = Math.Min(a._top, b._top);
        retVal._width = Math.Max(a.Right, b.Right) - retVal.Left;
        retVal._height = Math.Max(a.Bottom, b.Bottom) - retVal.Top;
        return retVal;
    }

    public static Rect Surround(params Rect[] rectangles)
    {
        if(rectangles == null) return new Rect();
        if(rectangles.Length == 0) return new Rect();
        Rect retVal = rectangles.First();
        foreach (var r in rectangles.Skip(1)) retVal += r;
        return retVal;
    }

    
    public override string ToString()
    {
        return $"({_left}, {_top}) - ({Right}, {Bottom})";
    }
}
