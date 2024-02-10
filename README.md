# Split And Merge Media
This repo provides a simple GUI for splitting and merging video and music, with features for pausing/canceling. Only supports Windows 10 and 11 (not tested on other versions of Windows). Powered by FFMPEG.
![image](https://github.com/PeteJobi/SplitAndMergeMedia/assets/45200292/643e23a9-b4c4-4851-b08d-21c5bdd63bb3)

## How to build
You need to have at least .NET 6 runtime installed to build the software. Download the latest runtime [here](https://dotnet.microsoft.com/en-us/download). If you're not sure which one to download, try [.NET 6.0 Version 6.0.16](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.408-windows-x64-installer)

In the project folder, run the below
```
dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained false
```
When that completes, go to `\bin\Release\net<version>-windows\win-x64\publish` and you'll find the **SplitAndMerge.exe** amongst other files. All the files are necessary for the software to run properly. Run **SplitAndMerge.exe** to use the software.

## Run without building
You can also just download the release builds if you don't wish to build manually. The assets release contains the assets used by the software. The standard release contains the compiled executable. Download them both, extract the assets to a folder and drop the executable in that folder.

If you wish to run the software without installing the required .NET runtime, download the self-contained release.

## How to use
The file types supported are ".mp4", ".mkv" and ".mp3".

The two options available are **Split** for splitting one video or music file into segments determined by duration, and **Merge** for merging two or more video or music file segments into one. It's important to know that for merging, the file segments chosen have to be of the same codec, file type, resolution e.t.c. No re-encoding is performed in the process.

The use-case that prompted me to build this was that I needed to split a really large video file into smaller parts so I could process each part (frame interpolation in my case) without having to worry about power outage. More than once, I've tried to interpolate a 4k video file, and after 5 hours and at 89%, my PC goes off due to power outage.

For splitting, you should enter the duration that you want each split segment to be. You have to do this before selecting the file to split, because as soon as you do that, the process starts. The resulting segments may be one or two seconds longer than the duration you chose - the video is split at the next frame after the duration. This is for the best as splitting at the exact duration might break the video, especially if you wish to re-merge it.

When you're done with the parameters, click **Select file** to select the file to be split, or **Select files** to select multiple files to be merged, or **Select folder** to merge every media in that folder. For the later, make sure that only the media you want to merge are in the folder. For both splitting and merging, you can also drag and drop files into the window.
