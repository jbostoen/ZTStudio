The latest version of ZT Studio allows you to specify some command line arguments. 

# Command line arguments 
There are two types of arguments. 

**settings**: 
The structure is based on _settings.cfg_ . 

The argument is simply:  
/iniSection.iniKey=newValue .  
Example: /paths.root="C:\Users\admin\Documents\ZTProjects".  

Nearly every setting - except for recent files/paths - is supported:

* /conversionOptions.deleteOriginal
* /conversionOptions.fileNameDelimiter
* /conversionOptions.overwrite
* /conversionOptions.pngFilesIndex
* /conversionOptions.sharedPalette
* /editing.animationSpeed
* /editing.individualRotationFix
* /exportOptions.pngCrop
* /exportOptions.pngRenderExtraFrame
* /exportOptions.pngRenderExtraGraphic
* /exportOptions.pngRenderTransparentBG
* /exportOptions.zt1AlwaysAddZTAFBytes
* /exportOptions.zt1Ani
* /paths.root
* /preview.bgColor
* /preview.fgColor

**actions**:
The actions are currently limited to:
* /action.convertFile.toPNG: converts a set of PNG graphics (should be in root folder)
* /action.convertFile.toZT1: converts a ZT1 graphic (should be in root folder)
* /action.convertFolder.toPNG: batch-converts entire specified folder to PNG (should be in root folder!)
* /action.convertFolder.toZT1: batch-converts entire specified folder to ZT1 (should be in root folder!)


**extra**
* /extra.colorQuantization: if set to 1, ZT Studio will automatically try to match the closest color if a graphic with too many colors is being converted.

# Settings file
For the latest version of the _settings.cfg_ file, refer to 
https://github.com/jbostoen/ZTStudio/blob/master/source/bin/Release/settings.cfg

Settings should be obvious but will be explained in detail later.