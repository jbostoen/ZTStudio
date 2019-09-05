# Changelog

Currently, the tool is at version 1.0. This reflects that it is the first main version intended for public release. 
he last part translates in the year, month and day of its compilation.

E.g. 1.0.2015.0511 = version 1.0, last updated 11th of May 2015

Version 1.0

# Build 2019.xx.yy:
* Requirements
  * .NET FrameWork 4.7.2 (no longer supporting lower versions)

* New
  * Explorer pane, showing contents of root folder
    * Selecting ZT1 Graphic: loads instantly
	* Double clicking a .pal file: loads palette in separate window
  * Method to list hashes of each file in a folder (meant for debugging when changing code, see if output is still the same)
    * Command line: /action.listhashes:foldername
  * Autoloads last used graphic
      
* Improvements
  * Added right click option to Save as button. 
    * Quickly saves to last known filename, rather than showing a dialog.
  * Some information now appears in the status bar rather than as a message box.
  * Cleaner UI in Settings; more help on mouse hover
  * Added a lot more code documentation.
  * Added tracing information

* Fixes
  * Several minor UI issues have been resolved.

# Build 2017.06.03:

* Improvements
  * Different method for determining the 'defining rectangle' (cropping all transparent borders). 
    * Now using a LockBits() method rather than GetPixel(). Much faster. 
	* Could be used in a couple of more places, but there it would only result in marginal gains.
	
* Fixes
  * Regression: issues when graphics were opened after each other.

# Build 2017.05.19:

* New 
  * Command line arguments implemented. 
    * Settings: basically everything found in settings.cfg.
    * Command line argument:
      * /ini_section_name.ini_key:new_val
    * Actions:
      * /action.convertFile.toPNG
      * /action.convertFile.toZT1
      * /action.convertFolder.toPNG
      * /action.convertFolder.toZT1 
  * If a color palette has reached its maximum number of colors, the user now has the choice between continuing and letting ZT Studio pick the closest matching color or quitting ZT Studio.
  * Do a crop around the grid's center (an option to both limit the size [height/width] of an image as well as keep the original offsetting).

* Improvements
  * ZT Studio now sets the game directory as the root path if it had not been set before.
  * You can choose to save frames as .PNG with either a fully transparent background or as ZT Studio's background color.

* Fixes
  * Right clicking on the column header of the color palette in the main window no longer causes a crash.
  * When converting a .PNG file to ZT1 using an existing palette, ZT Studio could still complain about a color which should have been interpreted as a transparent color instead (= same color as the background color you've set for ZT Studio)


# Build 2016.10.11:

* New 
  * Color palette in the main window got a few new features when you right click on a color or on the column heading. It is now possible to:
    * save the palette as a new .pal-file. 
    * export the palette as a 16x16 PNG file which acts as a color palette
    * replace the palette with a 16x16 PNG file which acts as a color palette
    * replace the palette with a GIMP Color Palette (.gpl, 256 colors maximum)
  * Batch conversions: 
    * User can specify file name delimiter used in file names (optional!). 
      * Rather than forcing a user to name their files NE_0000.png etc, it can now be NE0000.png . This is easier for some automated workflows (such as exporting from current Blender versions).
    * User can force ZT Studio to use a shared color palette during batch conversions. 
      * This currently needs to be placed in the parent folder of an animation. 
	    * It has to be the same name of the parent folder. It has to be either a .pal-file or a .GPL (GIMP Template File, the first color has to be the transparent one). A .PNG color palette is NOT supported in batch conversions to avoid confusion with real graphics!
	    * E.g. animals/redpanda/m/walk/NE would rely on animals/redpanda/m/m.pal 
	    * E.g. objects/restrant/idle/SE would rely on objects/restrant/restrant.pal 
	    * This is nearly the official approach, except for animals, where one would expect animals/redpanda/redpanda.pal . But since there are plans to add an easy recolor option and since the young would be a lighter version, it's currently done like this.
	    * For animals, if no animals/redpanda/m/m.pal file is available, it will fall back to an animals/redpanda/redpanda.pal if available, or to the classic 1 .pal file per view per animation per graphic.

* Improvements
  * Batch conversion was functional, but it did some work multiple times.
    * Occasionally processing graphics multiple times (times = amount of frames)
    * If offsets are known, it's not necessary to re-determine the defining rectangle
  * Complete rewrite of clsFrame. Now it's clsFrame2. Lots of things have been cleaned up, streamlined, changed to improve performance.

  * Top left pixel in images (or in the so-called 'defining rectangle' of an image) no longer determines transparency if a color palette has been generated already. This should work well, since a second frame would use the same background color as the first one. 

# Build 2016.08.27:

* From this build onward, the code is released/updated on GitHub as well.

* Fixes
  * Previous / next frame buttons didn't work (the slider did)
  * Setting was stored incorrectly (delete files vs. overwrite files during conversion)
  * Last used directory per ZT1 Graphic and per PNG-file is what will be opened when opening/saving another one of those files.
  * .PNG files could not be deleted automatically since apparently there was a file lock after reading them in ZT Studio
  * The transparent color was not correctly rendered in the color palette (despite being properly written to a .pal file and internally being read correctly)
  * Corrected .ani-files
    * 'animation' is correct, not 'Animation'
    * Removed extra unwanted 'N'-view
    * dir-lines weren't numbered
  * Fixed background graphic rendering
  
* Improvements
  * Minor improvements in GUI. Some rephrasing, better remembering of last used files
  * The transparent color is no longer by default pink. It is now automatically the background color you prefer.
  * Right click on a position fix-button and you move the object 1/4th of a square

# Build 2015.0815:
* Fixes
  * Some performance improvements. Less triggers for ZT Studio to update certain information. Definitely helpful when the .ani-files are generated by ZT Studio.

# Build 2015.0624:
* New 
  * Create .ani-files. This option will automatically generate a .ani-file in the folder, if it detects one of the following combinations of file names:
    * N
    * NE, NW, SE, SW
    * N, NE, E, SE, S
    * 1, 2, 3, 4, 5, … , 20
	
* Fixes 
  * Exporting PNGs:
    * The button to export a frame as a .PNG image is now disabled, when no existing frame has been loaded or when the frame is still empty.
    * The setting to remember which option you prefer when saving PNGs wasn't properly remembered. (Canvas size, relevant size of frame, relevant size of graphic).
    * Cropping didn't work, all .PNG images were rendered on the full canvas size.
  * Color palettes:
    * An issue with a slow performing datagridview control has been resolved. Color palettes are displayed much faster.
  * Changed some labels
  * Changed lay-out of Settings-window
  * Review and changes in settings.cfg and in the internal naming of the settings.
  * Several minor enhancements.


## Build 2015.0511:
* No changelog available. It supports all initial features which are present in the application but which are not mentioned in newer release notes.
