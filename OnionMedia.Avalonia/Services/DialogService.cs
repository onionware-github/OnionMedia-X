/*
 * Copyright (C) 2022 Jaden Phil Nebel (Onionware)
 *
 * This file is part of OnionMedia.
 * OnionMedia is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, version 3.

 * OnionMedia is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

 * You should have received a copy of the GNU Affero General Public License along with OnionMedia. If not, see <https://www.gnu.org/licenses/>.
 */

using Avalonia.Dialogs;
using OnionMedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Platform.Storage.FileIO;
using FluentAvalonia.UI.Controls;
using OnionMedia.Avalonia;
using OnionMedia.Core.Extensions;

namespace OnionMedia.Services
{
    sealed class DialogService : IDialogService
    {
        public async Task<string> ShowFolderPickerDialogAsync(DirectoryLocation location = DirectoryLocation.Home)
        {
            OpenFolderDialog dlg = new();
            dlg.Directory = DirectoryLocationToPathString(location);
            return await dlg.ShowAsync(App.MainWindow);
        }

        public async Task<string> ShowSingleFilePickerDialogAsync(DirectoryLocation location = DirectoryLocation.Home)
        {
            var result = await App.MainWindow.StorageProvider.OpenFilePickerAsync(new() {AllowMultiple = false, SuggestedStartLocation = await App.MainWindow.StorageProvider.TryGetFolderFromPathAsync(DirectoryLocationToPathString(location)), FileTypeFilter = new List<FilePickerFileType> {new("mediaFiles".GetLocalized()) {Patterns = new List<string> {"*.*"}}}});
            if (result?.Any() is false) return null;
            return result[0].Path.LocalPath;
        }

        public async Task<string[]> ShowMultipleFilePickerDialogAsync(DirectoryLocation location = DirectoryLocation.Home)
        {
            var result = await App.MainWindow.StorageProvider.OpenFilePickerAsync(new() {AllowMultiple = true, SuggestedStartLocation = await App.MainWindow.StorageProvider.TryGetFolderFromPathAsync(DirectoryLocationToPathString(location)), FileTypeFilter = new List<FilePickerFileType> {new("mediaFiles".GetLocalized()) {Patterns = new List<string> {"*.*"}}}});
            return result?.Any() is true ? result.Select(r => r.Path.LocalPath).ToArray() : null;
        }

        public async Task<bool?> ShowDialogAsync(DialogTextOptions dialogTextOptions)
        {
            if (dialogTextOptions == null)
                throw new ArgumentNullException(nameof(dialogTextOptions));

            ContentDialog dlg = new()
            {
                Title = dialogTextOptions.Title,
                Content = new ScrollViewer()
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Content = BuildDialogContent(dialogTextOptions)
                },
                PrimaryButtonText = dialogTextOptions.YesButtonText,
                SecondaryButtonText = dialogTextOptions.NoButtonText,
                CloseButtonText = dialogTextOptions.CloseButtonText
            };

            var result = await dlg.ShowAsync();
            return ContentDialogResultToBool(result);
        }

        public async Task ShowInfoDialogAsync(string title, string content, string closeButtonText)
        {
            await new ContentDialog()
            {
                Title = title,
                CloseButtonText = closeButtonText,
                Content = new ScrollViewer()
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Content = new TextBlock()
                    {
                        Text = content,
                        TextWrapping = TextWrapping.Wrap
                    }
                }
            }.ShowAsync();
        }

        public async Task<bool?> ShowInteractionDialogAsync(string title, string content, string yesButtonText,
            string noButtonText, string cancelButtonText)
        {
            ContentDialog dlg = new()
            {
                Title = title,
                PrimaryButtonText = yesButtonText,
                SecondaryButtonText = noButtonText,
                CloseButtonText = cancelButtonText,
                Content = new ScrollViewer()
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    Content = new TextBlock()
                    {
                        Text = content,
                        TextWrapping = TextWrapping.Wrap
                    }
                }
            };

            var result = await dlg.ShowAsync();
            return ContentDialogResultToBool(result);
        }


        private static TextBlock BuildDialogContent(DialogTextOptions dialogTextOptions)
        {
            if (dialogTextOptions == null)
                throw new ArgumentNullException(nameof(dialogTextOptions));

            TextBlock txtBlock = new();
            txtBlock.Text = dialogTextOptions.Content;
            txtBlock.TextWrapping = GetTextWrapping(dialogTextOptions.ContentTextWrapping);
            return txtBlock;
        }

        private static TextWrapping GetTextWrapping(TextWrapMode wrapMode) => wrapMode switch
        {
            TextWrapMode.NoWrap => TextWrapping.NoWrap,
            TextWrapMode.Wrap => TextWrapping.Wrap,
            TextWrapMode.WrapWholeWords => TextWrapping.Wrap,
            _ => throw new NotImplementedException()
        };

        private static string DirectoryLocationToPathString(DirectoryLocation location) => location switch
        {
            DirectoryLocation.Home => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            DirectoryLocation.Desktop => Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            DirectoryLocation.Pictures => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            DirectoryLocation.Music => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
            DirectoryLocation.Videos => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
            DirectoryLocation.Documents => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            DirectoryLocation.Downloads => GetDownloadsPath(),
            DirectoryLocation.Homegroup => Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            _ => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        };

        private static string GetDownloadsPath()
        {
#if WINDOWS
            return SHGetKnownFolderPath(new("374DE290-123F-4565-9164-39C4925E467B"), 0);
#else
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
#endif
        }

#if WINDOWS
        [DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
            nint hToken = 0);
#endif

        private static bool? ContentDialogResultToBool(ContentDialogResult result) => result switch
        {
            ContentDialogResult.None => null,
            ContentDialogResult.Primary => true,
            ContentDialogResult.Secondary => false,
            _ => throw new NotImplementedException()
        };
    }
}