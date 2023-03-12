using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using FFMpegCore;
using OnionMedia.Avalonia.ViewModels;
using OnionMedia.Avalonia.Views;
using OnionMedia.Core;
using OnionMedia.Core.Models;
using OnionMedia.Core.Services;
using OnionMedia.Services;

namespace OnionMedia.Avalonia
{
    public sealed partial class App : Application
    {
        public static MainWindow MainWindow { get; private set; }
        public static readonly SoftwareVersion Version = new(2, 0, 0, 0);

        internal static readonly ServiceProvider DefaultServiceProvider = new();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    
                };
                MainWindow = (MainWindow)desktop.MainWindow;
                if (DefaultServiceProvider.GetService<IWindowClosingService>() is WindowClosingService closingService)
                    closingService.RegisterWindow(MainWindow);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}