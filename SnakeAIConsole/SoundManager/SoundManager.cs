using SnakeAICore;
using System.Media;

namespace SnakeAIConsole;

public class SoundManager : ThreadedManager
{
    private SoundPlayer? _onKill;
    private SoundPlayer? _onEat;
    private int _lastLength = 0;
    public SoundManager(SnakeAiConfiguration configuration)
    {
        Configuration = configuration;
    }

    public SnakeAiConfiguration Configuration { get; }

    public Game Game => Configuration.Game;

    protected override void Setup()
    {
        _onKill = new SoundPlayer("SoundManager/onKill.wav");
        _onEat = new SoundPlayer("SoundManager/onEat.wav");
    }

    protected override void Loop()
    {
        var tailLength = Game.Snake.TailLength;
        if (tailLength > _lastLength)
        {
            //Console.Beep();
            _onEat?.Stop();
            _onEat?.Play();
        }
        else if (tailLength < _lastLength)
        {
            _onKill?.Stop();
            _onKill?.Play();
        }
        _lastLength = tailLength;
    }
}
