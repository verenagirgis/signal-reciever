using System;
using System.Threading;
using SignalReciver.Collections;

namespace SignalReciver.Brokerage.IBClient
{
    internal sealed class RateLimiter
    {
        private readonly TimeSpan _per;

        private SemaphoreSlim _bufferProtection;

        private readonly CircularBuffer<DateTime> _buffer;

        internal RateLimiter(int requestCount, TimeSpan per)
        {
            _per = per;
            _buffer = new CircularBuffer<DateTime>(requestCount);
            _bufferProtection = new SemaphoreSlim(1);
        }

        internal void Enter()
        {
            _bufferProtection.Wait();

            // clean oldtest
            Clean();

            if (!_buffer.IsFull)
            {
                _buffer.PushBack(DateTime.Now);
                _bufferProtection.Release();
                return;
            }

            do
            {
                var front = _buffer.Front();
                var since = DateTime.Now - front;
                var diff = _per - since;
                Console.WriteLine("Pausing for " + diff.TotalMilliseconds + " MS");
                Thread.Sleep(diff);
                Clean();
            }
            while (_buffer.IsFull);

            _buffer.PushBack(DateTime.Now);
            _bufferProtection.Release();
        }

        // this call assume the caller have protection
        private void Clean()
        {
            if (_buffer.IsEmpty)
                return;

            // clean oldtest
            do
            {
                var oldtest = _buffer.Front();
                if (DateTime.Now - oldtest > _per)
                    _buffer.PopFront();
                else
                    break;
            } while (!_buffer.IsEmpty);
        }
    }
}