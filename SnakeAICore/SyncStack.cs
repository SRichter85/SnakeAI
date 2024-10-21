using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAICore;

// NOTE: Not yet implemented
public class SyncStack<T> : IEnumerable<T>
{
    public object _lock = new object();
    private bool[] _changed = Array.Empty<bool>();

    private Stack<T> _collection = new Stack<T>();
    
    private SyncStack<T>? _syncSrc;
    private int _syncId = -1;

    public SyncStack(SyncStack<T>? src = null)
    {
        _syncSrc = src;
    }

    public int Count { get { return _collection.Count; } }

    public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

    public void Sync()
    {
        if (_syncSrc == null) return;
        SyncHelper(_syncSrc, ref _syncId);
    }

    private void SyncHelper(SyncStack<T> copyFrom, ref int threadId)
    {
        if (threadId < 0)
        {
            threadId = NewChanged();
        }

        if (_changed[threadId])
        {
            lock (_lock)
            {
                _collection.Clear();
                lock (copyFrom._lock)
                {
                    foreach (var item in copyFrom) _collection.Push(item);
                }
            }
        }

        _changed[threadId] = false;
    }

    public T Pop()  {
        lock(_lock)
        {
            SetChanged();
            return _collection.Pop();
        }
        //Monitor.Enter(_lock);
        //var retval = _collection.Pop();
        //Monitor.Exit(_lock);
        // retval;
    }

    public IEnumerable<T> PopRange(int cnt)
    {
        lock(_lock)
        {
            SetChanged();
            Stack<T> retVal = new Stack<T>(cnt);
            retVal.Push(_collection.Pop());
            return retVal;
        }
    }

    public void Push(T item)
    {
        lock(_lock)
        {
            SetChanged();
            _collection.Push(item);
        }
    }

    public void PushRange(IEnumerable<T> items)
    {
        lock (_lock)
        {
            SetChanged();
            foreach (var item in items)
            {
                _collection.Push(item);
            }
        }
    }

    public SyncStack<T> Copy()
    {
        var retVal = new SyncStack<T>();
        lock(_lock)
        {
            retVal.PushRange(this);
        }

        return retVal;
    }

    private void SetChanged()
    {
        int l = _changed.Length;
        for(int i = 0; i < l; i++) _changed[i] = true;
    }

    private int NewChanged()
    {
        lock(_lock)
        {
            var tmp = _changed;
            _changed = new bool[tmp.Length + 1];
            _changed[tmp.Length] = true;
            tmp.CopyTo(_changed, 0);
        }

        return _changed.Length - 1;
    }

    IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
}
