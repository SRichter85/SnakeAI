using SnakeAICore;
using System.Drawing;

namespace SnakeAIConsole;

public class ConsoleManager : ThreadedManager {

    private List<ConsoleArea> _areas = new List<ConsoleArea>();

    public event EventHandler? OnProgramStop;

    public ConsoleManager(Game game) {
        Game = game;
    }

    public Game Game { get; private set; }

    public MenuView? Menu { get; private set; }

    protected override void Setup() {

        var legend = new LegendView(new Point(1, 1), new Size(38, 8));
        _areas.Add(legend);

        Menu = new MenuView(this, new Point(1, legend.BottomRight.Y + 1), new Size(38, Math.Min(Game.Board.Size.Height-10, 20)));
        _areas.Add(Menu);

        var game = new GameView(new Point(Menu.BottomRight.X + 1, 1), Game);
        _areas.Add(game);

        int bottomLine = Math.Max(game.BottomRight.Y, Menu.BottomRight.Y);
        var status = new GameStatusView(this, new Point(1,game.BottomRight.Y - 6), 38);
        _areas.Add(status);

        Console.WindowHeight = game.BottomRight.Y + 1;
        Console.WindowWidth = game.BottomRight.X + 1;

        Console.CursorVisible = false;

        ConsoleFontHelper.SetFont(ConsoleFontHelper.Font.Consolas, 12, 12, true);

    }

    public void RaiseOnProgramStop() {
        OnProgramStop?.Invoke(this, EventArgs.Empty);
    }

    protected override void Loop() {
        foreach (var a in _areas) a.Refresh();
    }



}