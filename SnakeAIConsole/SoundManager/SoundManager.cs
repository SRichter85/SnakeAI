using SnakeAICore;
using System.ComponentModel;
using System.Media;

namespace SnakeAIConsole;

public class SoundManager : BackgroundedManager
{
    private SoundPlayer? _onKill;
    private SoundPlayer? _onEat;

    /// <summary>
    /// stores the tail length of the snakes so that the Loop function can detect changes
    /// </summary>
    private Dictionary<Snake, int> _tailLengthCache = new Dictionary<Snake, int>();

    //private List<SoundPlayingThread> _eatThreads = new List<SoundPlayingThread>();

    public SoundManager(SnakeAiConfiguration configuration)
    {
        Configuration = configuration;
    }

    public SnakeAiConfiguration Configuration { get; }

    public Game Game => Configuration.Game;

    protected override void Setup()
    {
        //for (int i = 0; i < 1; i++) _eatThreads.Add(new SoundPlayingThread(new SoundPlayer("SoundManager/onEat.wav")));
        foreach (var snake in Game.Snakes.Snakes) _tailLengthCache[snake] = 0;

        _onKill = new SoundPlayer("SoundManager/onKill.wav");
        _onEat = new SoundPlayer("SoundManager/onEat.wav");
        _onKill?.Load();
        _onEat?.Load();
    }

    protected override void Loop()
    {
        foreach (var snakeIntPair in _tailLengthCache)
        {
            if (!snakeIntPair.Key.IsActive) continue;

            var tailLength = snakeIntPair.Key.TailLength;
            if (tailLength > snakeIntPair.Value)
            {
                // var eatThread = _eatThreads.FirstOrDefault(x => !x.IsPlaying);
                //if (eatThread != null) eatThread.PlayOnce();

                //var eatThread = _eatThreads.First();
                //eatThread.StopSound();
                //eatThread.PlayOnce();

                //_onEat?.PlaySync();
                _onEat?.Play();
            }
            else if (tailLength < snakeIntPair.Value)
            {
                _onKill?.PlaySync();
                //_onKill?.Play();
            }

            _tailLengthCache[snakeIntPair.Key] = snakeIntPair.Key.TailLength;
        }
    }
}


// Comment: The sound is sometimes buggy. The SoundPlayingThread was supposed to fix this, but did not improve.
// We still keep it here, maybe we will need it later.

public class SoundPlayingThread
{
    private readonly SoundPlayer _player;
    private readonly BackgroundWorker _worker = new BackgroundWorker();
    private bool _signalPlayOnce = false;

    public SoundPlayingThread(SoundPlayer player)
    {
        _player = player;
        _player.Load();
        _worker.WorkerSupportsCancellation = true;
        _worker.DoWork += _worker_DoWork;
        _worker.RunWorkerAsync();
    }

    public bool IsPlaying { get; private set; }
    public void PlayOnce() => _signalPlayOnce = true;
    public void Stop() => _worker.CancelAsync();

    public void StopSound() => _player.Stop();

    private void _worker_DoWork(object? sender, DoWorkEventArgs e)
    {
        while (!_worker.CancellationPending)
        {
            if (_signalPlayOnce)
            {
                IsPlaying = true;
                _signalPlayOnce = false;
                _player.Play();
                IsPlaying = false;
            }

            // Thread.Sleep(1);
        }
    }
}
