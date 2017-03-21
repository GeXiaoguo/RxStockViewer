using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace RxStockViewer
{
    public struct Tick
    {
        public int StockId { get; set; }
        public int AttributeId { get; set; }
        public decimal NewValue { get; set; }
    }

    /// <summary>
    /// listen to a stock feed server on its dedicated thread and 
    ///  convert all ticks into a FeedStream
    /// </summary>
    class StreamProvider
    {
        static int _randSeed;
        readonly Random _rand = new Random(_randSeed++);

        private Subject<Tick> _feedStream = new Subject<Tick>();

        public IObservable<Tick> FeedStream => _feedStream;

        public void StartFeed()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var tick = PullForTick();
                    _feedStream.OnNext(tick);
                }
            });
        }

        /// <summary>
        /// listen for a stock feed from a server in this function
        /// </summary>
        /// <returns></returns>
        private Tick PullForTick()
        {
            Thread.Sleep(50);
            return new Tick()
            {
                StockId = _rand.Next(0, 100),
                AttributeId = _rand.Next(0, 50),
                NewValue = ((decimal)_rand.Next(1, 10000)) / 100
            };
        }
    }
}
