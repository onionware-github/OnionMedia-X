# OnionMedia X
OnionMedia X is the planned cross-platform port of the OnionMedia project with primary focus on Linux

*Currently in development*

Build:
1. Download ffmpeg+ffprobe (GPL version with libx264) for your target system and paste the files in the correct folder to replace the placeholder files:
(Windows: OnionMedia.Avalonia/Platforms/Windows/Output/ExternalBinaries/binaries/ffmpeg+yt-dlp/)
(Linux: OnionMedia.Avalonia/Platforms/Linux/Output/ExternalBinaries/binaries/ffmpeg+yt-dlp/)
Download from gyan.dev or Btbn, be sure to use the gpl/full versions.

Gyan.dev: https://www.gyan.dev/ffmpeg/builds/#git-master-builds

Btbn: https://github.com/BtbN/FFmpeg-Builds/releases

2. (Linux only)

    a) In this folder, you can find the executable binary files. You have to give them execution permissions with ``chmod +x <path>``

    b) You also have to install "nscd", which is used for video trimming on downloads. Install it on your system, e.g. with ``sudo apt-get install nscd``

3. Install dotnet-sdk-7.0 and run ``dotnet run`` in the "OnionMedia.Avalonia" folder
