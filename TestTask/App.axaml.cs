using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using TestTask.Services;
using TestTask.ViewModels;
using TestTask.Views;

namespace TestTask
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

        }

        public override void OnFrameworkInitializationCompleted()
        {
            var collection = new ServiceCollection()
                .AddSingleton<IHistoryLoader, HistoryService>()
                .AddSingleton<PaginatedDataGridViewModel>()
                .AddSingleton<MainWindowViewModel>();

            var services = collection.BuildServiceProvider();

            ServiceProvider = services;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = services.GetRequiredService<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}