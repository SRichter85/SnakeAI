using SnakeAICore;
using System.Xml.Linq;

namespace SnakeAIConsole;

public class ControlManager : ThreadedManager {

    private Snake?[] _playerSnakes = new Snake?[2];

    public ControlManager(SnakeAiConfiguration configuration) {
        Configuration = configuration;
    }

    public SnakeAiConfiguration Configuration { get; }

    public Game Game => Configuration.Game;

    public ConsoleManager ConsoleManager => Configuration.Console;

    internal void ActivateSnake(int playerId)
    {
        if (playerId < 0 || playerId > 1) return;
        var snake = Game.Snakes.Get(playerId);
        snake.IsActive = true;
        _playerSnakes[playerId] = snake;
    }

    internal void DeactivateSnake(int playerId)
    {
        if (playerId < 0 || playerId > 1) return;

        var snake = _playerSnakes[playerId];
        _playerSnakes[playerId] = null;
        if (snake != null) snake.IsActive = false;
    }

    internal int GetPlayerId(Snake snake)
    {
        for (int idx = 0; idx < _playerSnakes.Length; idx++)
        {
            if(_playerSnakes[idx] == snake) return idx;
        }

        return -1;
    }

    protected override void Setup() {
    }

    protected override void Loop() {

        if (!Console.KeyAvailable) return;

        var keyPressed = Console.ReadKey(true);
        switch (keyPressed.Key) {
            case ConsoleKey.A:
            case ConsoleKey.LeftArrow:
                _playerSnakes[0]?.TurnLeft();
                break;
            case ConsoleKey.D:
            case ConsoleKey.RightArrow:
                _playerSnakes[0]?.TurnRight();
                break;
            case ConsoleKey.I:
                _playerSnakes[1]?.TurnLeft();
                break;
            case ConsoleKey.P:
                _playerSnakes[1]?.TurnRight();
                break;
            case ConsoleKey.S:
            case ConsoleKey.DownArrow:
                ConsoleManager.Menu?.MoveDown();
                break;
            case ConsoleKey.W:
            case ConsoleKey.UpArrow:
                ConsoleManager.Menu?.MoveUp();
                break;
            case ConsoleKey.Enter:
                ConsoleManager.Menu?.SelectedItem.Execute();
                break;
        }
    }

}
