using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace RxStockViewer
{
    /// <summary>
    /// listen to a tickStream and pick interested columns to dislay
    /// </summary>
    public class StockViewModel : INotifyPropertyChanged
    {
        public DataTable StockTable { get; set; }
        private readonly HashSet<int> _interestedColumns = new HashSet<int>();
        public long TicksPerSecond { get; set; }

        public StockViewModel(IEnumerable<int> interestedColumns, IObservable<Tick> tickStream, IObservable<long> tickCountStream)
        {
            _interestedColumns.UnionWith(interestedColumns);

            StockTable = CreateDataTable(_interestedColumns);

            tickStream.ObserveOn(DispatcherScheduler.Current)
                .Where(tick=>_interestedColumns.Contains(tick.AttributeId))
                .Subscribe(ProcessTick);

            tickCountStream
                .Sample(TimeSpan.FromMilliseconds(1000), DispatcherScheduler.Current)
                .Subscribe(ProcessTickCount);
        }

        private void ProcessTick(Tick tick)
        {
            StockTable.Rows[tick.StockId][tick.AttributeId.ToString()] = tick.NewValue;
        }

        private void ProcessTickCount(long newCount)
        {
            TicksPerSecond = newCount;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TicksPerSecond)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static DataTable CreateDataTable(IEnumerable<int> columnIds)
        {
            var dataTable = new DataTable();
            var column = dataTable.Columns.Add("StockId");
            foreach (var columnId in columnIds)
            {
                dataTable.Columns.Add(columnId.ToString());
            }

            dataTable.PrimaryKey = new[] { column };

            for (int stockId = 0; stockId < 100; stockId++)
            {
                var row = dataTable.NewRow();
                row["StockId"] = stockId;
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
    }
}
