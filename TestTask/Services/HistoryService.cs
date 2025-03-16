using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using TestTask.Models;

namespace TestTask.Services
{
    public class HistoryService : IHistoryLoader
    {
        public ObservableCollection<HistoryPosition> GenerateTradeHistory()
        {
            var faker = new Faker<HistoryPosition>()
                .RuleFor(h => h.PostId, f => Guid.NewGuid())
                .RuleFor(h => h.Ticker, f => f.PickRandom("BTCUSDT", "ETHUSDT", "XRPUSDT", "SOLUSDT", "DOGEUSDT"))
                .RuleFor(h => h.Side, f => f.PickRandom("BUY", "SELL"))
                .RuleFor(h => h.Quantity, f => f.Random.Int(1, 1000))
                .RuleFor(h => h.OpenPrice, f => f.Random.Decimal(1, 100))
                .RuleFor(h => h.ClosePrice, f => f.Random.Decimal(1, 100))
                .RuleFor(h => h.OpenTime, f => DateTime.UtcNow.AddMinutes(-f.Random.Int(1, 100)))
                .RuleFor(h => h.CloseTime, (f, h) => h.OpenTime.AddMinutes(-60));

            var count = new Random().Next(50, 200);
            return new ObservableCollection<HistoryPosition>(faker.Generate(count));
        }
    }
}
