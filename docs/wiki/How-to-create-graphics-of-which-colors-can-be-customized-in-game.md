# How to create graphics of which colors can be customized in-game

This is a tutorial which briefly describes how to create graphics which allow in-game color customization (similar to how roof colors can be changed in-game).

It's recommended to be familiar with the configuration of buildings.

## Considerations

* When an item where the user will be able to alter the colors in game, there are a few restrictions to keep in mind.
  * A ZT1 Graphic has a maximum of 255 (+1 transparent) colors (1 palette).
  * There's a maximum of 2 sets of colors which can be replaced.
  
  
A typical graphic allowing customization offers one or two colors which can be chosen.
* The first color range / palette consists of 16 colors
* The second color range / palette consists of 8 colors

If allowing user to pick 2 colors, then the range of remanining colors (which can not be changed) is limited to (256 (maximum) -1 (transparent) –16 (first color) –8 (second color) =) 231 colors.

It is recommended to use one or two color ranges which are very easy to spot. For example: very fluorescent yellow and pink. 
These colors will help to create a color palette in a very specific way. 
In the game, these two colors may not even be shown at all. They will be replaced by default colors.

You can also choose to only use one color palette. I’m not 100% sure if you can choose whether you use only the large or only the small version. You might also be able to use 2 color palettes of 16 colors.
And last but not least, very important: all your 4 views will have to share one .pal file.

Summarized:
* A graphic can contain maximum 2 color ranges of which the color can be replaced.
* A custom color palette consists of either 8 or 16 colors. This is the number of colors that need to be defined in a color range in the graphic’s color palette. However, it's not required to use all of them!
* Highly recommended: make sure all views use the same .pal file

## Creating the graphics

* Create the graphics.

For this example, we’ve combined the picnic table with an umbrella we’ve extracted from a Gift Cart. In this example, we will work with two colors the user will be able to determine in the game. 


* Use 2 distinct color ranges (one range of maximum 16 colors; another one would be maximum 8 colors) for the colors you want to replace.
  * Example: in the Umbrella table, the umbrella is green and yellow; while the bench is mostly brown. 
  * If designing a graphic from scratch, pick for instance pink and bright yellow. The colors might never be visible in the game.

* Use ZT Studio to create the graphics for the first time (hint: a shared color palette is really recommended!)
  * [How to create a color palette to share with several graphics (views, animations) using GIMP](How-to-create-a-color-palette-to-share-with-several-graphics-(views,-animations)-using-GIMP)
  * Open one of the views.
  * Group each of the two colors at the bottom of this newly generated color palette (right click the color in the color palette on the right of the main window to see reorder options).
    * Make sure the first color range consists of 16 colors (no need to use them all!)
    * Make sure the second color range consists of 8 colors (no need to use them all!)
  * Save this graphic once again. This step is necessary to save the adjusted color palette and to have ZT Studio write out the graphic correctly, since the order of the colors has changed.
  * Now the palette is ready.
    * Batch convert the graphics again from PNG to ZT1 using this prepared palette as a shared color palette.



## Configuration
This tutorial is restricted to the minimum and only deals with color customization. 
This already assumes the object is configured as a building.

Make sure this characteristic is present:
```
[Characteristics/Integers]
cIsColorReplaced = 1
```


Add the bottom, add:
```
[colorrep]
; cr_color is listed below
color = cr_color

; cr_part1 is listed in building.ai
replace = cr_part1
title = 2300
defaultpal = scenery/building/pals/brwn16.pal

; cr_part2 is listed in building.ai
replace = cr_part2
title = 2301
defaultpal = scenery/building/pals/gold8.pal

[cr_color]
ncolors = 210
fullpal = objects/umbtable/idle/SE.pal
colorpal = objects/umbtable/idle/SE.pal
```

So basically, in the [colorrep] section there is a reference to the [cr_color] section. 
That’s where we simply refer to our color palette. In the original ZT1 files, there is a slight difference. 

* **cr_color** (cr is likely short for color replacement?)
  * **fullpal** refers to a palette (.pal file) containing all colors.
  * **colorpal** refers to a palette containing only the colors which are not replaced in the game, for official graphics. It seems to be okay to refer to the full palette too though!
    * **ncolors** is the total number of colors which will not be replaced in the game. 
    * In this example, this means that the color palette file of the graphic does not even use the maximum of 255 (+1 transparent) colors.
      * Otherwise, **ncolors** would have been 232 if the maximum amount of colors was used.

The other lines under **colorrep** specify that 2 colors can be replaced. The first one is a palette of 16 colors and by default the object is shown with brownish colors; the second one is 8 colors and by default shown with gold colors.

