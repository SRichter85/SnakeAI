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
    
    public HighscoreView? Highscore { get; private set; }

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

        Highscore = new HighscoreView(Configuration, 1, game.BottomRight.Y + 1, game.BottomRight.X - 1);
        _areas.Add(Highscore);


        SetFontSize(8);
        Console.Title = "Snake A.I.";
        Console.WindowHeight = Highscore.BottomRight.Y + 1;
        Console.WindowWidth = Highscore.BottomRight.X + 1;

        Console.CursorVisible = false;
        SetFontSize(Configuration.Settings.FontSize);
    }

    public void SetFontSize(short fontSize)
    {
        ConsoleFontHelper.SetFont(ConsoleFontHelper.Font.Consolas, fontSize, fontSize, true);
    }
    protected override void Loop()
    {
        _areas.ForEach(x => x.Refresh());
    }



}