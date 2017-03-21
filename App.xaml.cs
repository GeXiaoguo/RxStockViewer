using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace RxStockViewer
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            var aggregator = new StreamAggregator();
            aggregator.Startup();
            var priceUpdateStream = aggregator.PriceUpdateStream;
            var tickCountStream = aggregator.TotalTickCountStream;

            StartNewWindow($"Window0 - Columns: {0} - {49} ",  Enumerable.Range(0, 50).ToList(), priceUpdateStream, tickCountStream);
            StartNewWindow($"Window1 - Columns: {10} - {39} ", Enumerable.Range(10, 40).ToList(), priceUpdateStream, tickCountStream);
            StartNewWindow($"Window2 - Columns: {20} - {49} ", Enumerable.Range(20, 50).ToList(), priceUpdateStream, tickCountStream);
        }

        /// <summary>
        /// start the window on its own thread
        /// </summary>
        private void StartNewWindow(string name, List<int> interestedColumns , IObservable<Tick> priceUpdateStream, IObservable<long> tickCountStream)
        {
            var thread = new Thread(() =>
            {
                var window = new StockWindow();
                window.Title = name;
                window.DataContext = new StockViewModel(interestedColumns, priceUpdateStream, tickCountStream);
                window.Show();
                System.Windows.Threading.Dispatcher.Run();

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
