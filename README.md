# RxStockViewer
Sample WPF StockViewer Application with [Reactive-Extensions/Rx.NET](https://github.com/Reactive-Extensions/Rx.NET "Rx.NET Github Link")

This sample application demonstrates how easy it is to use Rx.Net for pulling, aggregating, filtering and displaying streaming data. From remote server to multiple Views on the screen, the different steps are illustrated below

![Dataflow Diagram](RxStockViewer.png)

1. SteamProvider pulls a server and generates a Rx.NET IObservable stream.
2. SteamAggregator aggregates all IObservable streams and duplicates the result into a central processing thread.
3. The Views filter the single stream and duplicate the result into their own threads for display.

All StreamProviders, StreamAggregate, and Views run in their own threads. This is a typical setup in a real-world Stock viewing applications.

This application can also be a good performance test skeleton for WPF DataGrid. It calculates ticks/second processed and displays it on the View.


