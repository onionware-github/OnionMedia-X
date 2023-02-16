/*
 * Copyright (C) 2022 Jaden Phil Nebel (Onionware)
 *
 * This file is part of OnionMedia.
 * OnionMedia is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, version 3.

 * OnionMedia is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

 * You should have received a copy of the GNU Affero General Public License along with OnionMedia. If not, see <https://www.gnu.org/licenses/>.
 */

using OnionMedia.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAvalonia.UI.Controls;
using OnionMedia.Avalonia.Views.Dialogs;
using OnionMedia.Core.ViewModels.Dialogs;
using YoutubeExplode.Videos;

namespace OnionMedia.Services
{
    sealed class DownloaderDialogService : IDownloaderDialogService
    {
        public async Task<IEnumerable<IVideo>> ShowPlaylistSelectorDialogAsync(IEnumerable<IVideo> videosFromPlaylist)
        {
            PlaylistSelectorDialog dlg = new(videosFromPlaylist);
            if (await dlg.ShowAsync() != ContentDialogResult.Primary) return Array.Empty<IVideo>();
            return ((PlaylistSelectorViewModel)dlg.DataContext).Videos.Where(v => v.IsSelected);
        }
    }
}
