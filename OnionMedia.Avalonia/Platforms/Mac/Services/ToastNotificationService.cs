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
using DesktopNotifications.FreeDesktop;
using OnionMedia.Core.Extensions;

namespace OnionMedia.Avalonia.Mac.Services;

sealed class ToastNotificationService : IToastNotificationService
{
    private INotificationManager? _notificationManager;

    public ToastNotificationService()
    {

    }

    public void SendConversionDoneNotification(MediaItemModel mediafile, string filepath, string thumbnailpath)
    {

    }

    public void SendConversionsDoneNotification(uint amount)
    {

    }

    public void SendDownloadDoneNotification(VideoData video, string path)
    {

    }

    public void SendDownloadsDoneNotification(string folderpath, uint amount, IEnumerable<string>? filenames = null)
    {

    }
}