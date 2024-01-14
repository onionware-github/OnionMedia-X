/*
 * Copyright (C) 2022 Jaden Phil Nebel (Onionware)
 *
 * This file is part of OnionMedia.
 * OnionMedia is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, version 3.

 * OnionMedia is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

 * You should have received a copy of the GNU Affero General Public License along with OnionMedia. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using OnionMedia.Core.Services;
using System.IO;
using System.Reflection;

namespace OnionMedia.Avalonia.Mac.Services;

sealed class PathProvider : IPathProvider
{
    private string dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Onionware/OnionMedia/");
    private string currentDirectory = AppContext.BaseDirectory;
    public string InstallPath => currentDirectory;
    public string LocalPath => Path.Combine(dataDirectory, "LocalState");
    public string LocalCache => Path.Combine(dataDirectory, "LocalCache");
    public string Tempdir => Path.Combine(Path.GetTempPath(), "Onionmedia");
    public string ConverterTempdir => Path.Combine(Tempdir, "Converter");
    public string DownloaderTempdir => Path.Combine(Tempdir, "Downloader");
    public string ExternalBinariesDir => Path.Combine(InstallPath, "ExternalBinaries", "ffmpeg+yt-dlp", "binaries");
    public string FFmpegPath => Path.Combine(ExternalBinariesDir, "ffmpeg");
    public string YtDlPath => Path.Combine(ExternalBinariesDir, "yt-dlp");
    public string LicensesDir =>  Path.Combine(InstallPath, "licenses");
}
