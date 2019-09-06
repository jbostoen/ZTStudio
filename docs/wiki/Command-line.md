# Command line 
ZT Studio has gained command line operations very early on.
This allows integrating this tool into automated workflows (scripts).

There are basically two types of arguments:
* **settings**: rather than relying on what has been configured (**settings.cfg**); enforce certain settings
* **actions**: makes ZT Studio run an action

While not all settings might make sense to be altered with the command line, they have been made possible.


# Actions
As for files and folders: they should be within the root path specified!

**Important:** you can only use 1 action per command line instruction. If combined with settings: all settings are processed before the action is performed.

Use like:
```
ZTStudio.exe /action.convertFile.toZT1="C:\Users\root\Documents\testcases\objects\aqbounce\idle\NE"
```

| Action                           | Value                    | Explanation                                                         |
| -------------------------------- | ------------------------ | ------------------------------------------------------------------- |
| convertFile.toZT1                | Filename                 | Converts PNG file to ZT1 Graphic                                    |
| convertFile.toPNG                | Filename                 | Converts ZT1 Graphic to PNG                                         |
| convertFolder.toZT1              | Folder path              | Converts PNG files to ZT1 Graphics                                  |
| convertFolder.toPNG              | Folder path              | Converts ZT1 Graphics to PNG files                                  |
| listHashes *(experimental)*      | Folder path              | Creates file with list of hashes of each file in this folder.       |
|                                  |                          | Purpose: see if any changes in the source code of ZT Studio         |
|                                  |                          | cause any (unexpected) changes in the output (generated graphics)   |
| saveConfig                       | 1                        | Use if settings specified on the command line should be saved.      |



# Settings
The settings - with a few exceptions - are usually found in **settings.cfg**
If you see this in the settings.cfg file:

```
[conversionOptions]
pngFilesIndex=1
```

The command line translates to:
```
ZTStudio.exe /conversionOptions.pngFilesIndex=1
```

They are meant to be used in scripts. Mind: they are **NOT** saved by default, unless combined with **/action.saveConfig:1**

**conversionOptions**
Hint: might make more sense after reading [ZT1 Graphics Explained](ZT1-Graphics-Explained)

| Setting                          | Value                    | Explanation                                                         |
| -------------------------------- | ------------------------ | ------------------------------------------------------------------- |
| deleteOriginal                   | 0                        | Don't delete original files after conversion.                       |
|                                  | 1                        | Delete original files after conversion.                             |
| filenameDelimiter                | Character. Often _       | Defines the delimiter used in names. Optional, can be empty.        |
|                                  |                          | Used between the name of the ZT1 Graphic (e.g. NE) and              |
|                                  |                          | the PNG exported frame(s) (e.g. NE_0000.png, NE_0001.png,... )      |
| overwrite *(not implemented!)*   | 0                        | Do not overwrite destination file (if it exists).                   |
|                                  | 1                        | Do overwrite destination file (if it exists) without warning.       |
| pngFilesIndex                    | 0                        | Starts numbering of PNG files with 0 (when reading/writing).        |
|                                  | 1                        | Starts numbering of PNG files with 1 (when reading/writing).        |
| sharedPalette                    | 0                        | Always create a separate color palette per graphic.                 |
|                                  | 1                        | Try using a shared color palette between graphics.                  |

**editing**
| Setting                          | Value                    | Explanation                                                         |
| -------------------------------- | ------------------------ | ------------------------------------------------------------------- |
| animationSpeed                   | 125-1000                 | Default animation speed for new graphics.                           |
| individualRotationFix            | 0                        | All frames in the graphic are adjusted when changing offsets.       |
|                                  | 1                        | Only the offsets of the specified/displayed frame are altered.      |


**exportOptions**

| Setting                          | Value                    | Explanation                                                         |
| -------------------------------- | ------------------------ | ------------------------------------------------------------------- |
| pngCrop                          | 0                        | Keep canvas size (512x512 pixels)                                   |
|                                  | 1                        | Crop to largest relevant width / height in this graphic.            |
|                                  | 2                        | Crop to relevant pixels of this frame.                              |
|                                  | 3                        | Crop around center (fast but experimental).                         |
| pngRenderExtraframe              | 0                        | Render all frames, there is no background frame.                    |
|                                  | 1                        | Renders last frame of graphic as background for all other frames.   |
|                                  |                          | (example: all frames of smoke; last frame is building - Restaurant) |
| pngRenderExtraGraphic            | 0                        | Do not render a ZT1 Graphic as background.                          |
|                                  | 1                        | Renders a specified ZT1 Graphic as background.                      |
|                                  |                          | (example: the Rope Swing toy as background for swinging Orang Utan) |
| pngRenderTransparentBG           | 0                        | Renders ZT Studio's background color as PNG background color.       |
|                                  | 1                        | Renders PNG with a fully transparent background.                    |
| ZT1AlwaysAddZTAFBytes            | 0                        | Skips the ZTAF-bytes when creating a ZT1 graphic.                   |
|                                  | 1                        | Enforces the use of ZTAF-bytes at the beginning of a graphic.       |
| ZT1Ani                           | 0                        | Try to create an .ani file.                                         |
|                                  | 1                        | Do not create an .ani file                                          |


**paths**

| Setting                          | Value                    | Explanation                                                         |
| -------------------------------- | ------------------------ | ------------------------------------------------------------------- |
| root                             | <folder path>            | Specifies root project folder.                                      |


**preview**

| Setting                          | Value                    | Explanation                                                         |
| -------------------------------- | ------------------------ | ------------------------------------------------------------------- |
| bgColor                          | VB.Net color value       | Specifies background color of canvas                                |
| fgColor                          | VB.Net color value       | Specifies foreground color (grid)                                   |
| footPrintX                       | 2, 4, 6, ...             | Sets the footprint of the X-axis. Jumps by 2. Limit: unknown.       |
| footPrintY                       | 2, 4, 6, ...             | Sets the footprint of the Y-axis. Jumps by 2. Limit: unknown.       |
| zoom *(not implemented!)*        | 1                        | Reserved. Zoom feature never implemented!                           |


**extra**
These settings are *NOT* exposed in ZT Studio, at the moment.
They allow to specify choices which are often shown to the user during processes.

| Setting                          | Value                    | Explanation                                                         |
| -------------------------------- | ------------------------ | ------------------------------------------------------------------- |
| colorQuantization                | 0                        | Don't apply color quantization.                                     |
|                                  | 1                        | Apply color quantization.                                           |
|                                  |                          | If a graphic contains more than 255 colors, prevent error message.  |
|                                  |                          | Instead, look for the best possible match within the palette.       | 
|                                  |                          | Expect color degradation!                                           |



