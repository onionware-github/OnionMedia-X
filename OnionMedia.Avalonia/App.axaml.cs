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

namespace OnionMedia.Avalonia
{
	public sealed partial class App : Application
	{
		public static MainWindow MainWindow { get; private set; }
		public static readonly SoftwareVersion Version = new(2, 0, 0, 0);
		internal static readonly ServiceProvider DefaultServiceProvider = new();
		
		public override async void Initialize()
		{
			Ioc.Default.ConfigureServices(DefaultServiceProvider);
			IoC.Default.InitializeServices(DefaultServiceProvider);
			GlobalFFOptions.Configure(options => options.BinaryFolder = IoC.Default.GetService<IPathProvider>().ExternalBinariesDir);
			await DefaultServiceProvider.GetService<IFFmpegStartup>().InitializeFormatsAndCodecsAsync();
			//AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel(),
				};
				MainWindow = (MainWindow)desktop.MainWindow;
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}
