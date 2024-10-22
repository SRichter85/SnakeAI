using System.ComponentModel.Design;

namespace SnakeAICore;

public class Game : ThreadedManager
{
    private Stack<Food> _foodStack = new Stack<Food>();
    private object _foodLock = new object();
    public Game()
    {
        Board = new GameBoard(this);
        Snakes = new SnakePopulation(this);
    }

    public GameBoard Board { get; }

    public IEnumerable<Food> Food
    {
        get
        {
            lock (_foodLock) return new List<Food>(_foodStack); // return copy because of thread safety
        }
    }

    public int FoodCount { get { return _foodStack.Count; } }

    public SnakePopulation Snakes { get; }

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
        SetFoodCount(0);
    }

    protected override void Loop()
    {
        Snakes.UpdatePosition();

        Snakes.CheckCollision();
        lock(_foodLock)
        {
            foreach (var food in _foodStack)
            {
                Snakes.CheckCollision(food);
                food.UpdateState();
            }
        }

        Snakes.UpdateState();
    }
}
