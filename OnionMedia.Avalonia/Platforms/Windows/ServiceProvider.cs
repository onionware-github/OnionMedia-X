using System.IO;
using Jab;
using OnionMedia.Avalonia.States;
using OnionMedia.Avalonia.Windows.Services;
using OnionMedia.Core.Models;
using OnionMedia.Core.Services;
using OnionMedia.Core.ViewModels;
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
        var rscDir = Path.Combine(GetService<IPathProvider>().InstallPath, "Resources");
        string path = JsonResourceLoader.GetCurrentLanguagePath(rscDir);
        string fallback = Path.Combine(rscDir, "en-us");
        JsonStringResourceService = new JsonResourceLoader(path, fallback);
    }
    public IStringResourceService JsonStringResourceService { get; }
}