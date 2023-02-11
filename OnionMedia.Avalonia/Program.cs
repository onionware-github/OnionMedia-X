using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using FFMpegCore;
using OnionMedia.Core;
using OnionMedia.Core.Services;

namespace OnionMedia.Avalonia
{
	internal class Program
	{
		// Initialization code. Don't use any Avalonia, third-party APIs or any
		// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
		// yet and stuff might break.
		[STAThread]
		public static async Task Main(string[] args)
		{
			Ioc.Default.ConfigureServices(App.DefaultServiceProvider);
			IoC.Default.InitializeServices(App.DefaultServiceProvider);
			GlobalFFOptions.Configure(options => options.BinaryFolder = IoC.Default.GetService<IPathProvider>().ExternalBinariesDir);
			await App.DefaultServiceProvider.GetService<IFFmpegStartup>().InitializeFormatsAndCodecsAsync();
			BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
		} 

		// Avalonia configuration, don't remove; also used by visual designer.
		public static AppBuilder BuildAvaloniaApp()
			=> AppBuilder.Configure<App>()
				.UsePlatformDetect()
				.LogToTrace()
				.UseReactiveUI();
	}
}
