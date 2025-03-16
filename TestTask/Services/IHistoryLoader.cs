using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Models;

namespace TestTask.Services
{
    public interface IHistoryLoader
    {
        ObservableCollection<HistoryPosition> GenerateTradeHistory();
    }
}
