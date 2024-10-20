using SnakeAICore;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;

namespace SnakeAIConsole;

class Program
{
    private static bool _running = true;
    private static string endMessage = string.Empty;

    static void Main(string[] args) {
        var game = new Game() { FramesPerSecond = 2000 };
        var console = new ConsoleManager(game) { FramesPerSecond = 100 };
        var control = new ControlManager(game, console) { FramesPerSecond = 50 };
        var sound = new SoundManager(game) { FramesPerSecond = 50 };

        console.OnProgramStop += Console_OnProgramStop;
        var components = new List<ThreadedManager> { game, console, control, sound };
        foreach (var component in components) {
            component.OnException += Component_OnException;
            component.Start();
        }

        while (_running) Thread.Sleep(1000);

        Queue<long> closeTimes = new Queue<long>();
        foreach (var component in components)
        {
            Stopwatch sw = Stopwatch.StartNew();
            component.Stop(true);
            closeTimes.Enqueue(sw.ElapsedMilliseconds);
        }

        if (!string.IsNullOrWhiteSpace(endMessage)) {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(endMessage);
        }
    }

    private static void Console_OnProgramStop(object? sender, EventArgs e) {
        _running = false;
    }

    private static void Component_OnException(object? sender, ExceptionEventArgs e) {
        _running = false;
        endMessage = "Ein Fehler ist auftetreten:\n" + e.Exception.Message;
    }
}
