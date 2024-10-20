using SnakeAICore;
using System.Drawing;

namespace SnakeAIConsole;

public class ConsoleManager : ThreadedManager
{

    private List<ConsoleArea> _areas = new List<ConsoleArea>();

    public ConsoleManager(SnakeAiConfiguration configuration)
    {
        Configuration = configuration;
    }

    public SnakeAiConfiguration Configuration { get; }

    public Game Game => Configuration.Game;

    public MenuView? Menu { get; private set; }

    protected override void Setup()
    {

        var legend = new LegendView(new Point(1, 1), 38);
        _areas.Add(legend);

        Menu = new MenuView(Configuration, new Point(1, legend.BottomRight.Y + 1), 38);
        _areas.Add(Menu);

        var game = new GameView(new Point(Menu.BottomRight.X + 1, 1), Game);
        _areas.Add(game);

        var status = new GameStatusView(this, new Point(1, game.BottomRight.Y - 6), 38);
        _areas.Add(status);

        Console.Title = "Snake A.I.";
        Console.WindowHeight = game.BottomRight.Y + 1;
        Console.WindowWidth = game.BottomRight.X + 1;

        Console.CursorVisible = false;

        ConsoleFontHelper.SetFont(ConsoleFontHelper.Font.Consolas, 12, 12, true);

    }

    protected override void Loop()
    {
        _areas.ForEach(x => x.Refresh());
    }



}