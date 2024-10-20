using System.ComponentModel.Design;

namespace SnakeAICore;

public class Game : ThreadedManager
{

    public Game()
    {
        Board = new GameBoard(this);
        Snake = new Snake(this);
    }

    public GameBoard Board { get; }

    public SyncStack<Food> Food { get; } = new SyncStack<Food>();

    public Snake Snake { get; }

    public void SetFoodCount(int cnt)
    {
        if (cnt < 0) return;

        int diff = Food.Count - cnt;
        //lock(Food._lock)
        //{
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    Food.Pop();
                }
            }
            else if (diff < 0)
            {
                for (int i = diff; i < 0; i++)
                {
                    Food.Push(new Food(this));
                }
            }
        //}
    }

    protected override void Setup()
    {
        SetFoodCount(10);
    }

    protected override void Loop()
    {
        Snake.UpdatePosition();
        Snake.CheckCollision(Snake);
        lock(Food._lock)
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
