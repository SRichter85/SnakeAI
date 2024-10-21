using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAICore
{
    // NOTE: Not yet implemented

    public class SyncQueue<T> : Queue<T>
    {
        private object _lock = new object();

        public void Sync(SyncQueue<T> copyFrom)
        {
            Clear();
            lock(copyFrom._lock)
            {
                foreach (var item in copyFrom) Enqueue(item);
            }
        }
    }
}
