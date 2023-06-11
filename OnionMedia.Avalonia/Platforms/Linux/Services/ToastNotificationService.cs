using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.IO;
using Avalonia;
using OnionMedia.Core.Models;
using OnionMedia.Core.Services;
using YoutubeDLSharp.Metadata;
using DesktopNotifications;
using OnionMedia.Core.Extensions;

namespace OnionMedia.Avalonia.Platforms.Linux.Services;

sealed class ToastNotificationService : IToastNotificationService
{
    private INotificationManager? _notificationManager;

    public ToastNotificationService()
    {
        _notificationManager = AvaloniaLocator.Current.GetService<INotificationManager>();
        if (_notificationManager is not null)
            _notificationManager.NotificationActivated += OnNotificationActivated;
    }

    public void SendConversionDoneNotification(MediaItemModel mediafile, string filepath, string thumbnailpath)
    {
        _notificationManager?.ShowNotification(new()
        {
            Title = "conversionDone".GetLocalized("Resources"),
            Body = mediafile.Title + '\n' + mediafile.FileTags.Artist,
            Buttons =
            {
                new("playFile".GetLocalized("Resources"),
                    JsonSerializer.Serialize(new NotificationAction(new[] { filepath }, true))),
                new("openFolder".GetLocalized("Resources"),
                    JsonSerializer.Serialize(new NotificationAction(new[] { filepath }, false)))
            }
        });
    }

    public void SendConversionsDoneNotification(uint amount)
    {
        _notificationManager?.ShowNotification(new()
        {
            Title = "conversionDone".GetLocalized("Resources"),
            Body = "filesConverted".GetLocalized("Resources").Replace("{0}", amount.ToString())
        });
    }

    public void SendDownloadDoneNotification(VideoData video, string path)
    {
        _notificationManager?.ShowNotification(new()
        {
            Title = "downloadFinished".GetLocalized("Resources"),
            Body = video.Title + '\n' + video.Uploader,
            Buttons =
            {
                new("playFile".GetLocalized("Resources"),
                    JsonSerializer.Serialize(new NotificationAction(new[] { path }, true))),
                new("openFolder".GetLocalized("Resources"),
                    JsonSerializer.Serialize(new NotificationAction(new[] { path }, false)))
            }
        });
    }

    public void SendDownloadsDoneNotification(string folderpath, uint amount, IEnumerable<string>? filenames = null)
    {
        Notification notification = new()
        {
            Title = "conversionDone".GetLocalized("Resources"),
            Body = $"{amount} {"videosDownloaded".GetLocalized()}"
        };
        var files = filenames?.ToArray() ?? Array.Empty<string>();
        if (files.Any())
        {
            notification.Buttons.Add(("openFolder".GetLocalized("Resources"),
                JsonSerializer.Serialize(new NotificationAction(files, false))));
        }

        _notificationManager?.ShowNotification(notification);
    }

    private void OnNotificationActivated(object? sender, NotificationActivatedEventArgs e)
    {
        Debug.WriteLine(e.ActionId);
        try
        {
            var action = JsonSerializer.Deserialize<NotificationAction>(e.ActionId);
            if (!(action.Files?.Length > 0)) return;

            //Open folder
            if (!action.OpenFileDirectly)
            {
                Process.Start("xdg-open", Path.GetDirectoryName(action.Files[0]));
                return;
            }

            Process.Start("xdg-open", action.Files[0]);
        }
        catch
        {
            Debug.WriteLine("File opening aborted");
        }
    }

    public record NotificationAction(string[] Files, bool OpenFileDirectly);
}
