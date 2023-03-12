# OnionMedia X
OnionMedia X is the planned cross-platform port of the OnionMedia project with primary focus on Linux

*Currently in development*

Build:
1. Download ffmpeg+ffprobe (GPL version recommended) for your target system and paste the files in the correct folder to replace the placeholder files:
(Windows: OnionMedia.Avalonia/Platforms/Windows/Output/ExternalBinaries/binaries/ffmpeg+yt-dlp/)
(Linux: OnionMedia.Avalonia/Platforms/Linux/Output/ExternalBinaries/binaries/ffmpeg+yt-dlp/)

2. (Linux only) In this folder, you can find the "yt-dlp" file. You have to give it execution permissions with ``chmod +x <yt-dlp path>``

3. Install dotnet-sdk-7.0 and run ``dotnet run`` in the "OnionMedia.Avalonia" folder
