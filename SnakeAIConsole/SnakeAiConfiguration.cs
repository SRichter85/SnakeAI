using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole;

public class SnakeAiConfiguration
{
    public const string SettingsPath = "settings.json";

    private List<ThreadedManager> _components = new List<ThreadedManager>(4);
    private Thread _mainThread;

    public SnakeAiConfiguration()
    {
        _mainThread = Thread.CurrentThread;

        Game = new Game() { MillisecondsPerFrame = 32 };
        Console = new ConsoleManager(this) { MillisecondsPerFrame = 8 };
        Control = new ControlManager(this) { MillisecondsPerFrame = 8 };
        Sound = new SoundManager(this) { FramesPerSecond = 20 };

        _components.Add(Game);
        _components.Add(Console);
        _components.Add(Control);
        _components.Add(Sound);

        _components.ForEach(c => c.OnException += Component_OnException);
        Game.Snake.OnKilled += Snake_OnKilled;
    }

    public Game Game { get; }
    public ConsoleManager Console { get; }

    public ControlManager Control { get; }

    public SoundManager Sound { get; }

    public Settings Settings { get; private set; } = new Settings();

    public bool IsRunning => _components.Any(x => x.IsRunning);

    public void Start()
    {
        var settings = Settings.Read(SettingsPath);
        if (settings != null)
        {
            Settings = settings;
        }

        _components.ForEach(c => c.Start());

        Game.SetFoodCount(Settings.FoodCount);
        Game.MillisecondsPerFrame = Settings.GameSpeed;
    }

    public void Stop()
    {
        Settings.Write(SettingsPath);
        _components.ForEach(c => c.Stop(false));
    }

    private void Component_OnException(object? sender, ExceptionEventArgs e)
    {
        Stop();
        System.Console.BackgroundColor = ConsoleColor.DarkRed;
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine("An Error occured during execution:");
        System.Console.WriteLine(e.Exception.Message);
    }

    private void Snake_OnKilled(object? sender, EventArgs e)
    {
        var snake = sender as Snake;
        if (snake == null) return;

        Settings.AppendHighscore(snake, Game.FrameCount - snake.CreatedAtFrame);
        Console.Highscore?.SetData(Settings.Highscores);
    }
}
