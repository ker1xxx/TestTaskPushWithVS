using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.ViewModels
{
    public partial class MainWindowViewModel : ReactiveObject
    {
        private readonly IHistoryLoader _historyLoader = App.ServiceProvider.GetRequiredService<IHistoryLoader>();

        public PaginatedDataGridViewModel DataGridViewModel { get; } = App.ServiceProvider.GetRequiredService<PaginatedDataGridViewModel>();

        public ReactiveCommand<Unit, Unit> GenerateTradeHistoryCommand { get; }


        public MainWindowViewModel()
        {
            GenerateTradeHistoryCommand = DataGridViewModel.GenerateTradeHistoryCommand;
        }
    }
}
