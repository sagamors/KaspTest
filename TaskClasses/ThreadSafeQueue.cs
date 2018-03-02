using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace KaspTest
{
    /// <summary>
    /// 1.Надо сделать очередь с операциями push(T) и T pop(). 
    /// Операции должны поддерживать обращение с разных потоков. 
    /// Операция push всегда вставляет и выходит. Операция pop ждет пока не появится новый элемент. 
    /// В качестве контейнера внутри можно использовать только стандартную очередь (Queue). 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Count = {Count}")]
     public class ThreadSafeQueue<T>
    {
        private SpinLock _locker = new SpinLock();
        private object _queueLocker = new object();
        private object _popLocker = new object();
        private Queue<T> _queue = new Queue<T>();
        public int Count { get { return _queue.Count; } }

        public void Push(T value)
        {
            bool gotLock = false;

            lock (_queueLocker)
            {
                try
                {
                    _locker.Enter(ref gotLock);
                    _queue.Enqueue(value);
                }
                finally
                {
                    if (gotLock)
                    {
                        _locker.Exit();
                    }
                }
                Monitor.Pulse(_queueLocker);
            }
        }

        public T Pop()
        {
            T result = default(T);
            bool gotLock = false;

            lock (_popLocker)
            {
                lock (_queueLocker)
                {
                    if (_queue.Count == 0) Monitor.Wait(_queueLocker);

                    try
                    {
                        _locker.Enter(ref gotLock);
                        _queue.TryDequeue(out result);
                    }
                    finally
                    {
                        if (gotLock)
                        {
                            _locker.Exit();
                        }
                    }
                }
            }

            return result;
        }
    }
}
