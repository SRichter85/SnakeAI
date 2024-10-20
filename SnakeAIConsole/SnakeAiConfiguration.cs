using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole;

public class SnakeAiConfiguration
{
    private List<ThreadedManager> _components = new List<ThreadedManager>(4);
    
    public SnakeAiConfiguration()
    {
        Game = new Game() { FramesPerSecond = 20 };
        Console = new ConsoleManager(this) { FramesPerSecond = 50 };
        Control = new ControlManager(this) { FramesPerSecond = 50 };
        Sound = new SoundManager(this) { FramesPerSecond = 50 };

        _components.Add(Game);
        _components.Add(Console);
        _components.Add(Control);
        _components.Add(Sound);

        _components.ForEach(c => c.OnException += Component_OnException);

        IsRunning = false;
    }

    public Game Game { get; }
    public ConsoleManager Console { get; }

    public ControlManager Control { get; }

    public SoundManager Sound { get; }

    public bool IsRunning { get; private set; }

    public void Start()
    {
        _components.ForEach(c => c.Start());
        IsRunning = true;
    }

    public void Stop()
    {
        _components.ForEach(c => c.Stop(true));
        IsRunning = false;
    }

    private void Component_OnException(object? sender, ExceptionEventArgs e)
    {
        Stop();
        System.Console.BackgroundColor = ConsoleColor.DarkRed;
        System.Console.ForegroundColor = ConsoleColor.White;
        System.Console.WriteLine("An Error occured during execution:");
        System.Console.WriteLine(e.Exception.Message);
    }
}
