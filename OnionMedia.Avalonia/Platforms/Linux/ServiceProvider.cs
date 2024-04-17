using System.IO;
using Jab;
using OnionMedia.Avalonia.States;
using OnionMedia.Avalonia.Linux.Services;
using OnionMedia.Core.Services;
using OnionMedia.Core.Services.Implementations;
using OnionMedia.Core.ViewModels;
using OnionMedia.Core.Models;
using OnionMedia.Services;

namespace OnionMedia.Avalonia;

[ServiceProvider]
//Core Services
[Singleton<IDataCollectionProvider<LibraryInfo>, LibraryInfoProvider>]
[Singleton(typeof(IDialogService), typeof(DialogService))]
[Singleton(typeof(IDownloaderDialogService), typeof(DownloaderDialogService))]
[Singleton(typeof(IThirdPartyLicenseDialog), typeof(ThirdPartyLicenseDialog))]
[Singleton(typeof(IConversionPresetDialog), typeof(ConversionPresetDialog))]
[Singleton(typeof(IFiletagEditorDialog), typeof(FiletagEditorDialog))]
[Singleton(typeof(ICustomPresetSelectorDialog), typeof(CustomPresetSelectorDialog))]
[Singleton(typeof(IDispatcherService), typeof(DispatcherService))]
[Singleton(typeof(INetworkStatusService), typeof(NetworkStatusService))]
[Singleton(typeof(IUrlService), typeof(UrlService))]
[Singleton(typeof(ITaskbarProgressService), typeof(TaskbarProgressService))]
[Singleton(typeof(IToastNotificationService), typeof(ToastNotificationService))]
[Singleton(typeof(IStringResourceService), Instance = nameof(JsonStringResourceService))]
[Singleton(typeof(ISettingsService), typeof(SettingsService))]
[Singleton(typeof(IPathProvider), typeof(PathProvider))]
[Singleton(typeof(IVersionService), typeof(VersionService))]
[Singleton(typeof(IWindowClosingService), typeof(WindowClosingService))]
[Singleton(typeof(IPCPower), typeof(LinuxPowerService))]
[Singleton(typeof(IFFmpegStartup), typeof(FFmpegStartup))]
//Views and ViewModels
[Singleton(typeof(MediaViewModel))]
[Singleton(typeof(YouTubeDownloaderViewModel))]
[Transient(typeof(SettingsViewModel))]
//States
[Singleton(typeof(LicenseDialogState))]
sealed partial class ServiceProvider
{
    public ServiceProvider()
    {
        string path = JsonResourceLoader.GetCurrentLanguagePath(Path.Combine(GetService<IPathProvider>().InstallPath, "Resources"));
        JsonStringResourceService = new JsonResourceLoader(path);
    }
    public IStringResourceService JsonStringResourceService { get; }
}