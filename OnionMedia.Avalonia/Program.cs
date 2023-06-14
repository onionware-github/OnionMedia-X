using Avalonia;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using CommunityToolkit.Mvvm.DependencyInjection;
using DesktopNotifications;
using DesktopNotifications.FreeDesktop;
using FFMpegCore;
using OnionMedia.Core;
using OnionMedia.Core.Services;

#if WINDOWS
using Windows.Storage;
using Windows.System;
using Microsoft.Toolkit.Uwp.Notifications;
#endif

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
            RegisterWindowsToastNotifications();
            GlobalFFOptions.Configure(options =>
                options.BinaryFolder = IoC.Default.GetService<IPathProvider>().ExternalBinariesDir);
            await App.DefaultServiceProvider.GetService<IFFmpegStartup>().InitializeFormatsAndCodecsAsync();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
#if WINDOWS
                .With(new Win32PlatformOptions
                {
                    UseWindowsUIComposition = true
                })
#elif LINUX
                .SetupLinuxNotifications()
                .With(new X11PlatformOptions
                {
                    UseDBusFilePicker = false //disables Portal filepickers from FreeDesktop
                })
#endif
                .UseReactiveUI();


        static void RegisterWindowsToastNotifications()
        {
#if WINDOWS
            try
            {
                // Listen to notification activation
                ToastNotificationManagerCompat.OnActivated += async toastArgs =>
                {
                    // Obtain the arguments from the notification
                    ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                    // Obtain any user input (text boxes, menu selections) from the notification
                    var userInput = toastArgs.UserInput;

                    Debug.WriteLine("Toast activated!");

                    if (args.Contains("action", "play"))
                    {
                        string filepath = args.Get("filepath");
                        Debug.WriteLine(filepath);

                        if (File.Exists(filepath))
                            await Launcher.LaunchUriAsync(new Uri(filepath));
                    }
                    else if (args.Contains("action", "open path"))
                    {
                        string folderpath = Path.GetDirectoryName(args.Get("folderpath"));

                        //Select the new files
                        FolderLauncherOptions folderLauncherOptions = new();
                        if (args.TryGetValue("filenames", out string filenames))
                            foreach (var file in filenames.Split('\n'))
                                folderLauncherOptions.ItemsToSelect.Add(
                                    await StorageFile.GetFileFromPathAsync(Path.Combine(folderpath, file)));

                        if (Directory.Exists(folderpath))
                            await Launcher.LaunchFolderPathAsync(folderpath, folderLauncherOptions);
                    }

                    if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated())
                        Environment.Exit(0);
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
#endif
        }
    }
#if LINUX
    public static class AppBuilderLinuxNotificationExtension
    {
        public static AppBuilder SetupLinuxNotifications(this AppBuilder builder)
        {
            INotificationManager manager;
            var runtimeInfo = builder.RuntimePlatform.GetRuntimeInfo();


            var context = FreeDesktopApplicationContext.FromCurrentProcess();
            manager = new FreeDesktopNotificationManager(context);

            //TODO Any better way of doing this?
            manager.Initialize().GetAwaiter().GetResult();

            builder.AfterSetup(b =>
            {
                if (b.Instance.ApplicationLifetime is IControlledApplicationLifetime lifetime)
                {
                    lifetime.Exit += (s, e) => { manager.Dispose(); };
                }
            });

            AvaloniaLocator.CurrentMutable.Bind<INotificationManager>().ToConstant(manager);

            return builder;
        }
    }
#endif
}