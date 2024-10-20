using SnakeAICore;

namespace SnakeAIConsole;

public class ControlManager : ThreadedManager {

    public ControlManager(SnakeAiConfiguration configuration) {
        Configuration = configuration;
    }

    public SnakeAiConfiguration Configuration { get; }

    public Game Game => Configuration.Game;

    public ConsoleManager ConsoleManager => Configuration.Console;

    protected override void Setup() {
    }

    protected override void Loop() {

        if (!Console.KeyAvailable) return;

        var keyPressed = Console.ReadKey(true);
        switch (keyPressed.Key) {
            case ConsoleKey.A:
            case ConsoleKey.LeftArrow:
                Game.Snake.TurnLeft();
                break;
            case ConsoleKey.D:
            case ConsoleKey.RightArrow:
                Game.Snake.TurnRight();
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
