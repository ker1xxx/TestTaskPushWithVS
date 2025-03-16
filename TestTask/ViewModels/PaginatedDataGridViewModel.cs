

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.ViewModels
{
    public class PaginatedDataGridViewModel : ReactiveObject
    {
        private IHistoryLoader _historyLoader = App.ServiceProvider.GetRequiredService<IHistoryLoader>();

        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _numPageButtons = 5;
        private ObservableCollection<HistoryPosition> _pagedData;
        private List<HistoryPosition> _allData = new();

        public ObservableCollection<HistoryPosition> PagedData
        {
            get => _pagedData;
            private set => this.RaiseAndSetIfChanged(ref _pagedData, value);
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value < 3) value = 3;
                else if (value > 50) value = 50;
                this.RaiseAndSetIfChanged(ref _pageSize, value);
                GoToPage(1);
            }
        }

        public int NumPageButtons
        {
            get => _numPageButtons;
            set
            {
                if (value < 3) value = 3;
                else if (value > 10) value = 10;
                this.RaiseAndSetIfChanged(ref _numPageButtons, value);
                UpdatePageNumbers();
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            private set
            {
                if (value < 1 || value > TotalPages) return;
                this.RaiseAndSetIfChanged(ref _currentPage, value);
            }
        }

        private ObservableCollection<PageModel> _pageNumbers = new();
        public ObservableCollection<PageModel> PageNumbers
        {
            get => _pageNumbers;
            set => this.RaiseAndSetIfChanged(ref _pageNumbers, value);
        }

        public ReactiveCommand<Unit, Unit> GenerateTradeHistoryCommand { get; }
        public ReactiveCommand<int, Unit> GoToPageCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToFirstPageCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToPreviousPageCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToNextPageCommand { get; }
        public ReactiveCommand<Unit, Unit> GoToLastPageCommand { get; }
        public bool CanGoPrevious => _currentPage > 1;
        public bool CanGoNext => _currentPage < TotalPages;
        public int TotalPages => (_allData.Count + _pageSize - 1) / _pageSize;

        public PaginatedDataGridViewModel()
        {
            PagedData = new ObservableCollection<HistoryPosition>();
            GenerateTradeHistoryCommand = ReactiveCommand.Create(GenerateTradeHistory);
            GoToPageCommand = ReactiveCommand.Create<int>(GoToPage);
            GoToFirstPageCommand = ReactiveCommand.Create(() => GoToPage(1));
            GoToPreviousPageCommand = ReactiveCommand.Create(() => GoToPage(_currentPage - 1), this.WhenAnyValue(vm => vm.CanGoPrevious));
            GoToNextPageCommand = ReactiveCommand.Create(() => GoToPage(_currentPage + 1), this.WhenAnyValue(vm => vm.CanGoNext));
            GoToLastPageCommand = ReactiveCommand.Create(() => GoToPage(TotalPages));
            GenerateTradeHistory();
        }

        private void GenerateTradeHistory()
        {
            _allData = new ObservableCollection<HistoryPosition>(_historyLoader.GenerateTradeHistory()).ToList();
            GoToPage(1);
        }

        private void GoToPage(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > TotalPages) return;

            _currentPage = pageNumber;
            PagedData = new ObservableCollection<HistoryPosition>(_allData
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize));

            UpdatePageNumbers();

            this.RaisePropertyChanged(nameof(CanGoPrevious));
            this.RaisePropertyChanged(nameof(CanGoNext));
            Console.WriteLine($"Текущая страница: {_currentPage}, Количество записей: {PagedData.Count}");
        }

        private void UpdatePageNumbers()
        {
            int halfButtons = _numPageButtons / 2;
            int startPage = Math.Max(1, _currentPage - halfButtons);
            int endPage = Math.Min(TotalPages, startPage + _numPageButtons - 1);

            if (endPage - startPage + 1 > _numPageButtons)
            {
                endPage = startPage + _numPageButtons - 1;
            }

            PageNumbers.Clear();
            foreach (var page in Enumerable.Range(startPage, endPage - startPage + 1))
            {
                PageNumbers.Add(new PageModel
                {
                    PageNumber = page,
                    CanGoToPage = _currentPage != page
                });
            }
        }
    }
}