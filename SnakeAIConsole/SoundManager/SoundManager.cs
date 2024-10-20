using SnakeAICore;
using System.Media;

namespace SnakeAIConsole;

public class SoundManager : ThreadedManager
{
    private SoundPlayer? _onKill;
    private SoundPlayer? _onEat;
    private int _lastLength = 0;
    public SoundManager(Game game)
    {
        Game = game;
    }

    public Game Game { get; }

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
            _onEat?.Play();
        }
        else if (tailLength < _lastLength)
        {
            _onKill?.Play();
        }
        _lastLength = tailLength;
    }
}
