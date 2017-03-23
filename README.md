# RxStockViewer
Sample WPF StockViewer Application with Rx.Net

This sample application demonstrates how Rx.Net can help in displaying streaming data. From remote server to a View on the screen, there are 3 steps as illustrated below

![Dataflow Diagram](RxStockViewer.png)

1. SteamProvider pull a server and generate a Rx.Net IObservable stream
2. SteamAggregator aggregates all IObservable streams into a single one for centralized processing
3. The View splits the Single stream into different ones according to interest of presentation

Each StreamProvider, StreamAggregate, View run in its own thread. This is a typical setup in a real-world Stock viewing application.

This application can also be a good performance test skeleton for WPF DataGrid. It calculates ticks/second processed and displays it on the View.


