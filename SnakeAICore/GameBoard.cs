using System.Drawing;

namespace SnakeAICore;

/// <summary>
/// Represents information about the game board itself
/// </summary>
public class GameBoard
{
    /// <summary>
    /// The random generator
    /// </summary>
    private Random _random = new Random();

    /// <summary>
    /// Creates a new GameBoard
    /// </summary>
    /// <param name="game">The owner of this board</param>
    internal GameBoard(Game game)
    {
        Game = game;
    }

    /// <summary>
    /// The game for this board
    /// </summary>
    public Game Game { get; }

    /// <summary>
    /// The size for this board
    /// </summary>
    public Size Size { get; } = new Size(50, 50);

    /// <summary>
    /// Returns a new, random point within the board's dimension and which is not occupied by a snake or food.
    /// </summary>
    /// <returns></returns>
    internal Point CreatePoint()
    {
        Point retVal = new Point(0,0);

        while (true)
        {
            retVal = new Point(_random.Next(0, Size.Width), _random.Next(0, Size.Height));
            if (!IsPointOccupied(retVal)) break;
        }

        return retVal;
    }

    /// <summary>
    /// Returns a new point on the board, but makes sure that the values are within the board's dimension.
    /// </summary>
    /// <param name="x">the desired x coordinate</param>
    /// <param name="y">the desired y coordinate</param>
    /// <returns></returns>
    internal Point CreatePoint(int x, int y) => new Point(CycleNumber(x, Size.Width), CycleNumber(y, Size.Height));

    /// <summary>
    /// Checks if a point on the board is already occupied by a game object
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool IsPointOccupied(Point point)
    {
        var snake = Game.Snake;
        if(snake != null)
        {
            if (point == Game.Snake.Head) return true;

            var tails = Game.Snake.Tails;
            if (tails.Any(x => x == point)) return true;
        }

        var foods = Game.Food;
        if(foods.Any(x => x.Position == point)) return true;

        return false;
    }


    private int CycleNumber(int number, int max)
    {
        if (number < 0) return max - 1;
        if (number >= max) return 0;
        return number;
    }
}
