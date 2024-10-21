using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAICore;

public interface IThreadedManager
{
    event EventHandler<ExceptionEventArgs> OnException;

    double FramesPerSecond { get; set; }
    int MillisecondsPerFrame { get; set; }
    double MeasuredFramesPerSecond { get; }
    int FrameCount { get; }
    bool IsRunning { get; }
    bool CancellationPending { get; }

    void Start();

    void Stop(bool wait);
}
/// <summary>
/// Base Class for top-level components of the application. Wraps around a thread and contains properties to control the number of cycles (called frames) which are executed per second
/// </summary>
public abstract class ThreadedManager : IThreadedManager
{
    private Thread _thread;
    private int _msPerFrame = 0;
    private double _fps = Double.NaN;

    private Queue<long> _frameTicks = new Queue<long>();

    public event EventHandler<ExceptionEventArgs>? OnException;

    protected ThreadedManager()
    {
        for (int i = 0; i < 20; i++) _frameTicks.Enqueue(0);
        _thread = new Thread(ThreadAction);
    }

    /// <summary>
    /// Sets or gets the desired number of frames which should get executed. Note that the minimum time per frame is 1ms, so any number higher then 1001 fps will be treated as if there is no wait time.
    /// </summary>
    public double FramesPerSecond
    {
        get { return _fps; }
        set
        {
            _fps = value < 0 ? 0 : value;
            _msPerFrame = (Double.IsInfinity(_fps) || Double.IsNaN(_fps) ) ? 0 : _msPerFrame = (int)(1000.0 / _fps);
        }
    }

    public int MillisecondsPerFrame
    {
        get { return _msPerFrame; }
        set
        {
            _msPerFrame = value < 0 ? 0 : value;
            if (_msPerFrame == 0)
            {
                _fps = Double.PositiveInfinity;
            }
            else
            {
                _fps = 1000.0 / _msPerFrame;
            }
        }
    }

    /// <summary>
    /// The measured numbers of frames per second
    /// </summary>
    public double MeasuredFramesPerSecond
    {
        get;
        private set;
    }

    /// <summary>
    /// The number of cycles or frames the thread has completed
    /// </summary>
    public int FrameCount { get; private set; }
    public bool IsRunning { get; private set; }
    public bool CancellationPending { get; private set; }

    public void Start()
    {
        IsRunning = true;
        Setup();
        _thread.Start();
    }

    /// <summary>
    /// Stops the execution. Note, that once aborted the thread cannot be restarted again
    /// </summary>
    /// <param name="wait"></param>
    public void Stop(bool wait)
    {
        CancellationPending = true;
        if (wait)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (IsRunning)
            {
                Wait(sw);
                sw.Restart();
            }
        }
    }

    protected abstract void Setup();

    protected abstract void Loop();


    /// <summary>
    /// Returns only after stopwatch.ElapsedMilliseconds is bigger or equal to _msPerFrame
    /// </summary>
    /// <param name="stopwatch"></param>
    private void Wait(Stopwatch stopwatch)
    {
        if (_msPerFrame < 1) return;

        while ((_msPerFrame - stopwatch.ElapsedMilliseconds) > 16)
        {
            // TestThreadSleepBehaviour();
            Thread.Sleep(1);
        }

        while (stopwatch.ElapsedMilliseconds < _msPerFrame)
        {
            Thread.Sleep(0);
        }

        // TODO: Experiment with ResetEvents to get the behaviour with less CPU overload.
    }

    private void UpdateMeasurementFps(long elapsedTicks)
    {
        _frameTicks.Enqueue(elapsedTicks);
        var last10 = _frameTicks.Dequeue();
        var avgTicks = _frameTicks.Average();
        MeasuredFramesPerSecond = avgTicks <= 1 ? double.PositiveInfinity : ((double)Stopwatch.Frequency) / avgTicks;
    }

    private static void Test_ThreadSleepBehaviour()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Queue<long> elapsed = new Queue<long>();
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(1);
            elapsed.Enqueue(stopwatch.ElapsedMilliseconds);
        }

        // Result: elapsed has usually values like (10, 26, 42, 58, ...) in steps of roughly 16, even
        // though Sleep-Parameter is set to 1ms. This is probably different on different machines.
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ThreadAction()
    {
        var stopwatch = Stopwatch.StartNew();
        while (!CancellationPending)
        {
            try
            {
                Loop();
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, new ExceptionEventArgs(ex));
                break;
            }

            Wait(stopwatch);

            UpdateMeasurementFps(stopwatch.ElapsedTicks);
            FrameCount++;
            stopwatch.Restart();
        }

        IsRunning = false;
    }
}

public abstract class BackgroundedManager : IThreadedManager
{
    private BackgroundWorker _thread;
    private int _msPerFrame = 0;
    private double _fps = Double.NaN;

    private Queue<long> _frameTicks = new Queue<long>();

    public event EventHandler<ExceptionEventArgs>? OnException;

    protected BackgroundedManager()
    {
        for (int i = 0; i < 20; i++) _frameTicks.Enqueue(0);
        _thread = new BackgroundWorker();
        _thread.DoWork += (x, y) => ThreadAction();
    }

    /// <summary>
    /// Sets or gets the desired number of frames which should get executed. Note that the minimum time per frame is 1ms, so any number higher then 1001 fps will be treated as if there is no wait time.
    /// </summary>
    public double FramesPerSecond
    {
        get { return _fps; }
        set
        {
            _fps = value < 0 ? 0 : value;
            _msPerFrame = (Double.IsInfinity(_fps) || Double.IsNaN(_fps)) ? 0 : _msPerFrame = (int)(1000.0 / _fps);
        }
    }

    public int MillisecondsPerFrame
    {
        get { return _msPerFrame; }
        set
        {
            _msPerFrame = value < 0 ? 0 : value;
            if (_msPerFrame == 0)
            {
                _fps = Double.PositiveInfinity;
            }
            else
            {
                _fps = 1000.0 / _msPerFrame;
            }
        }
    }

    /// <summary>
    /// The measured numbers of frames per second
    /// </summary>
    public double MeasuredFramesPerSecond
    {
        get;
        private set;
    }

    /// <summary>
    /// The number of cycles or frames the thread has completed
    /// </summary>
    public int FrameCount { get; private set; }
    public bool IsRunning { get; private set; }
    public bool CancellationPending { get; private set; }

    public void Start()
    {
        IsRunning = true;
        Setup();
        _thread.RunWorkerAsync();
    }

    /// <summary>
    /// Stops the execution. Note, that once aborted the thread cannot be restarted again
    /// </summary>
    /// <param name="wait"></param>
    public void Stop(bool wait)
    {
        CancellationPending = true;
        if (wait)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (IsRunning)
            {
                Wait(sw);
                sw.Restart();
            }
        }
    }

    protected abstract void Setup();

    protected abstract void Loop();


    /// <summary>
    /// Returns only after stopwatch.ElapsedMilliseconds is bigger or equal to _msPerFrame
    /// </summary>
    /// <param name="stopwatch"></param>
    private void Wait(Stopwatch stopwatch)
    {
        if (_msPerFrame < 1) return;

        while ((_msPerFrame - stopwatch.ElapsedMilliseconds) > 16)
        {
            // TestThreadSleepBehaviour();
            Thread.Sleep(1);
        }

        while (stopwatch.ElapsedMilliseconds < _msPerFrame)
        {
            Thread.Sleep(0);
        }

        // TODO: Experiment with ResetEvents to get the behaviour with less CPU overload.
    }

    private void UpdateMeasurementFps(long elapsedTicks)
    {
        _frameTicks.Enqueue(elapsedTicks);
        var last10 = _frameTicks.Dequeue();
        var avgTicks = _frameTicks.Average();
        MeasuredFramesPerSecond = avgTicks <= 1 ? double.PositiveInfinity : ((double)Stopwatch.Frequency) / avgTicks;
    }

    private static void Test_ThreadSleepBehaviour()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        Queue<long> elapsed = new Queue<long>();
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(1);
            elapsed.Enqueue(stopwatch.ElapsedMilliseconds);
        }

        // Result: elapsed has usually values like (10, 26, 42, 58, ...) in steps of roughly 16, even
        // though Sleep-Parameter is set to 1ms. This is probably different on different machines.
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ThreadAction()
    {
        var stopwatch = Stopwatch.StartNew();
        while (!CancellationPending)
        {
            try
            {
                Loop();
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, new ExceptionEventArgs(ex));
                break;
            }

            Wait(stopwatch);

            UpdateMeasurementFps(stopwatch.ElapsedTicks);
            FrameCount++;
            stopwatch.Restart();
        }

        IsRunning = false;
    }
}