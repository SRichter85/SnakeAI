using System.ComponentModel.Design;

namespace SnakeAICore;

public class Game : ThreadedManager
{
    private Stack<Food> _foodStack = new Stack<Food>();
    private object _foodLock = new object();
    public Game()
    {
        Board = new GameBoard(this);
        Snake = new Snake(this);
    }

    public GameBoard Board { get; }

    public IEnumerable<Food> Food
    {
        get
        {
            lock (_foodLock) return new List<Food>(_foodStack);
        }
    }

    public int FoodCount { get { return _foodStack.Count; } }

    public Snake Snake { get; }

    public void SetFoodCount(int cnt)
    {
        if (cnt < 0) return;

        int diff = FoodCount - cnt;
        lock (_foodLock)
        {
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    _foodStack.Pop();
                }
            }
            else if (diff < 0)
            {
                for (int i = diff; i < 0; i++)
                {
                    _foodStack.Push(new Food(this));
                }
            }
        }
    }

    protected override void Setup()
    {
        SetFoodCount(10);
    }

    protected override void Loop()
    {
        Snake.UpdatePosition();
        Snake.CheckCollision(Snake);
        lock(_foodLock)
        {
            foreach (var f in Food)
            {
                Snake.CheckCollision(f);
                f.UpdateState();
            }
        }

        Snake.UpdateState();
    }
}
