# OnionMedia X - The Free Media Converter and Downloader
<a href="https://github.com/onionware-github/OnionMedia/blob/main/images/icon.svg">
  <img src="https://github.com/onionware-github/OnionMedia/blob/main/images/icon.svg" width="275"/>
</a>

Converts and downloads videos and music quickly and easily.

[Overview](#overview) • [Features](#features) • [Download](#download-and-installing) • [Releases](https://github.com/onionware-github/OnionMedia-X/releases/) • [Discord](https://discord.gg/3ahqCzQxs8) • [Info](#info)

## Overview
![Screenshot showing the Converter-Page](https://github.com/onionware-github/OnionMedia-X/blob/main/screenshots/ubuntu/converterpage_dark.png?raw=true)
You're looking for a simple solution to convert, recode, trim or even download media files and do it as easily as possible with many customizable options?

Then OnionMedia is the right choice for you.
It offers a simple and adaptive user experience and also adapts to your chosen Windows theme.
So it fits perfectly into your system as an almost native app.

![Screenshot showing the Downloader-Page](https://github.com/onionware-github/OnionMedia-X/blob/main/screenshots/ubuntu/downloaderpage_dark.png?raw=true)
Download multiple videos and audios at the same time from many platforms with just a click.
OnionMedia delivers a built-in Searchbar for Youtube, functions to recode your downloaded videos to the H.264 codec after download and lets you get shortened videos if you want.

## Features

- #### Converting files
  - Recode to other video and audio codecs.
  - Hardware-Accelerated-Encoding for video files.
  - Change resolution, aspect ratio, bitrates and frames per second.
  - Short the file and get only a part instead of the full content.
  - Edit the Tags from files (e.g. Title, Author, Album...)

- #### Downloading files
  - Supports many platforms
  - Search videos or add them directly with an URL.
  - Add multiple videos to the queue and download them at the same time.
  - Lets you select a resolution to download the video, or download only the audio of the file.
  - Recode the video after download directly to H.264

- #### Other
  - Customizable theme and accent color
  - Completely free and Open-Source

## Download and installing

#### Download from Flathub
<a href='https://flathub.org/en/apps/io.github.onionware_github.onionmedia'><img width='240' alt='Download on Flathub' src='https://dl.flathub.org/assets/badges/flathub-badge-en.svg'/></a>


#### Download directly from GitHub
[Go to the Releases page and find all releases of OnionMedia to download!](https://github.com/onionware-github/OnionMedia-X/releases)


## Info
Copyright © Jaden Phil Nebel
###### OnionMedia is Free Software and is based on the awesome tools ffmpeg and yt-dlp for converting and downloading files.

<a href="https://ffmpeg.org/">
  <img src="https://github.com/onionware-github/OnionMedia/blob/main/images/ffmpeg.svg" width="250" valign="middle" margin-right="10"/>
</a>


<a href="https://github.com/yt-dlp/yt-dlp">
  <img src="https://github.com/onionware-github/OnionMedia/blob/main/images/yt-dlp.svg" width="250" height="65" valign="bottom" margin-left="10"/>
</a>


## More information
You still have open questions? [Join our Discord Server](https://discord.gg/3ahqCzQxs8)



Build:
1. Download ffmpeg+ffprobe (GPL version with libx264) for your target system and paste the files in the correct folder to replace the placeholder files:
(Windows: OnionMedia.Avalonia/Platforms/Windows/Output/ExternalBinaries/binaries/ffmpeg+yt-dlp/)
(Linux: OnionMedia.Avalonia/Platforms/Linux/Output/ExternalBinaries/binaries/ffmpeg+yt-dlp/)
Download from gyan.dev or Btbn, be sure to use the gpl/full versions. Current ffmpeg version: 6.0

Gyan.dev: https://www.gyan.dev/ffmpeg/builds/#git-master-builds

Btbn: https://github.com/BtbN/FFmpeg-Builds/releases

2. (Linux only)

    a) In this folder, you can find the executable binary files. You have to give them execution permissions with ``chmod +x <path>``

    b) You also have to install "nscd", which is used for video trimming on downloads. Install it on your system, e.g. with ``sudo apt-get install nscd``

3. Install dotnet-sdk-7.0 and run ``dotnet run`` in the "OnionMedia.Avalonia" folder
