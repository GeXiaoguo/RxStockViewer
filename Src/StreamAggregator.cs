using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxStockViewer
{
    /// <summary>
    /// Start 5 StreamProviders all in their own dedicated thread
    /// Subscribe to the 5 providers in my own thread
    /// Process ticks from all providers in my own thread
    /// Generate StockValueUpdateStream which aggregate 5 streams into 1
    /// </summary>
    public class StreamAggregator
    {
        Subject<Tick> _priceUpdateStream = new Subject<Tick>();
        Subject<long> _totalTickCountStream = new Subject<long>();
        private long _totalTickCount;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public IObservable<Tick> PriceUpdateStream => _priceUpdateStream;
        public IObservable<long> TotalTickCountStream => _totalTickCountStream;
        public void Startup()
        {
            IScheduler scheduler = new EventLoopScheduler();
            SubscriptToFeedAndStart(0, scheduler);
            SubscriptToFeedAndStart(10, scheduler);
            SubscriptToFeedAndStart(20, scheduler);
            SubscriptToFeedAndStart(30, scheduler);
            SubscriptToFeedAndStart(40, scheduler);
        }
        private void SubscriptToFeedAndStart(int columnOffset, IScheduler scheduler)
        {
            var provider = new StreamProvider();
            _stopwatch.Start();

            provider.StartFeed();

            provider.FeedStream
                .ObserveOn(scheduler)
                .Subscribe(tick => ProcessTick(columnOffset, tick));
        }

        private void ProcessTick(int columnOffset, Tick tick)
        {
            if (_stopwatch.Elapsed.Seconds > 10)
            {
                _totalTickCount = 0;
                _stopwatch.Restart();
            }
            _totalTickCount++;
            _totalTickCountStream.OnNext(1000 * _totalTickCount / (_stopwatch.ElapsedMilliseconds + 1));

            tick.AttributeId += columnOffset;

            _priceUpdateStream.OnNext(tick);
        }
    }
}