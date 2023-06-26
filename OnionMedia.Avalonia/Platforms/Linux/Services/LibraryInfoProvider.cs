using System.IO;
using OnionMedia.Core.Models;
using OnionMedia.Core.Services;

namespace OnionMedia.Avalonia.Linux.Services;

public class LibraryInfoProvider : IDataCollectionProvider<LibraryInfo>
{
    private readonly IPathProvider pathProvider;
    public LibraryInfoProvider(IPathProvider pathProvider)
    {
        this.pathProvider = pathProvider;
    }
    
    public LibraryInfo[] GetItems() => new LibraryInfo[] 
    {
        new("FluentAvaloniaUI", "amwx", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/amwx/FluentAvalonia", 2020),
        new("Avalonia", ".NET Foundation and Contributors", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://avaloniaui.net/"),
        new("Avalonia.Xaml.Behaviors", "Wiesław Šoltés", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/AvaloniaUI/Avalonia.Xaml.Behaviors"),
        new("AsyncImageLoader.Avalonia", "SKProCH", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/AvaloniaUtils/AsyncImageLoader.Avalonia", 2021),
        new("Avalonia.RangeSlider", "Dmitry Nizhebovsky", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/DmitryNizhebovsky/Avalonia.RangeSlider", 2022),
        new("FluentAvalonia.ProgressRing", "ymg2006", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/ymg2006/FluentAvalonia.ProgressRing", 2022),
        new("FFmpeg", "FFmpeg 64-bit", "GNU GPL v3", Path.Combine(pathProvider.InstallPath, "ExternalBinaries", "ffmpeg+yt-dlp", "FFmpeg_LICENSE"), "https://github.com/FFmpeg/FFmpeg/"),
        new("yt-dlp", "yt-dlp", "Unlicense", Path.Combine(pathProvider.LicensesDir, "yt-dlp.txt"), "https://github.com/yt-dlp/yt-dlp"),
        new("CommunityToolkit", ".NET Foundation and Contributors", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/CommunityToolkit/WindowsCommunityToolkit"),
        new("FFMpegCore", "Vlad Jerca", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/rosenbjerg/FFMpegCore", 2023),
        new("Newtonsoft.Json", "James Newton-King", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/JamesNK/Newtonsoft.Json", 2007),
        new("DesktopNotifications.FreeDesktop", "Luis von der Eltz", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/pr8x/DesktopNotifications", 2021),
        new("TagLib#", "mono", "LGPL v2.1", Path.Combine(pathProvider.LicensesDir, "TagLibSharp.txt"), "https://github.com/mono/taglib-sharp"),
        new("xFFmpeg.NET", "Tobias Haimerl(cmxl)", "MIT License", Path.Combine(pathProvider.LicensesDir, "mit-license"), "https://github.com/cmxl/FFmpeg.NET", 2018),
        new("YoutubeDLSharp", "Bluegrams", "BSD 3-Clause License", Path.Combine(pathProvider.LicensesDir, "YoutubeDLSharp.txt"), "https://github.com/Bluegrams/YoutubeDLSharp"),
        new("YoutubeExplode", "Tyrrrz", "LGPL v3", Path.Combine(pathProvider.LicensesDir, "YoutubeExplode.txt"), "https://github.com/Tyrrrz/YoutubeExplode")
    };
}