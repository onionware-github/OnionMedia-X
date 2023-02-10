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

namespace OnionMedia.Services;

sealed class PathProvider : IPathProvider
{
    public string InstallPath => Environment.CurrentDirectory;
    public string LocalPath => Environment.CurrentDirectory + @"\LocalState";
    public string LocalCache => Environment.CurrentDirectory + @"\LocalCache";
    public string Tempdir => Path.GetTempPath() + @"\Onionmedia";
    public string ConverterTempdir => Tempdir + @"\Converter";
    public string DownloaderTempdir => Tempdir + @"\Downloader";
    public string ExternalBinariesDir => InstallPath + @"\Platforms\Windows\ExternalBinaries\ffmpeg+yt-dlp\binaries\";
    public string FFmpegPath => ExternalBinariesDir + "ffmpeg.exe";
    public string YtDlPath => ExternalBinariesDir + "yt-dlp.exe";
    public string LicensesDir => InstallPath + @"\licenses\";
}