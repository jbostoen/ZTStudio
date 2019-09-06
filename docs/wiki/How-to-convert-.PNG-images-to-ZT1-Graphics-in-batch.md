# How to convert .PNG images to ZT1 Graphics in batch
Some preparations are in order, but you can save yourself quite some time and trouble. The batch conversion is a highly optimized workflow.

For people who use 3D software to create animals, they can usually generate .PNG images in batch straight from their 3D model with a specific file name pattern. 
But also other designers who use more common non-3D graphic manipulation tools (Paint, Paint.NET, GIMP, ...) can benefit from this.

## Preparing file and folder structure
If you're unfamiliar with the ZT1 structures, take a look at existing objects, animals etc. 
For animals, you typically have a **folder structure** like animals/panda/m/walk . 
The animal name is usually shorted, each part of the path should be **up to 8 alphanumeric characters max**. 
That's why you might see a folder structure like objects/restrant/idle .
It's recommended to never start the name with a number though!

Hint: over the years, a lot of ZT1 fans created their own content and used a prefix for their creation; 
so they reserved the first character(s) to include their initials for example. It helps reducing conflicting file/folder names.

The **file names** of your .PNG-files should be like this: <view name>_00xx.PNG . 
For example: **NE_0000.png**. 

If you're working with animated objects or with animal animations which have animated (changing) graphics:
a second frame would be named **NE_0001.png**,
a third frame would be named **NE_0002.png**,
a third frame would be named **NE_0003.png**,
and so on.

Hint: in programming, things are often referred to by index, which usually starts at 0.
In ZT Studio's settings, you can specify whether you want to start numbering from 0 or from 1.

### Share a color palette or not?
This is an **optional** and **advanced** step, but ZT Studio offers a unique feature.
You can skip this step if you don't need/want it!

It must be enabled in ZT Studio's settings.

You can force a graphic to **share a color palette between each graphic's animations - excluding icons**. 
It makes things more complex and requires some additional preparation ( see specific page in this Wiki ). 

**Benefits?**
* Recoloring is easier
  * Simply change this one color palette and all this graphic's animations are recolored at once.
* Looks more official; less files.
* The total size of whatever animal/object/... you're creating, smaller. 
  * When the game was originally released, lots of people had much slower internet speed and much more limited bandwidth (amount of data they could download in a certain time).
  * Nowadays this feature is still very interesting for people who are hosting content.
* Optimized for ZT1 game engine.
  * Mostly on slower PCs; newer PCs are much faster than they used to be.

**Downsides?**
* Needs specific preparation
* You are limited to 255 colors per graphic (colors in icons excluded!). This shouldn't be an issue: Blue Fang never needed separate palettes in this case. 

Let's say you do want a shared color palette. You will need to make sure your suggested color palette is in the right place. 
For convenience this can be:
* a ZT1 Color Palette (.pal)
* a .PNG-file (16x16 pixels = 256 colors, the top left pixel will determine which color should be invisible [background])
* a .GPL-file (GIMP Color Palette, 256 colors).

Before you start the conversion, you'd put the palette in the right folder. This does make a difference!
For ZT Studio, the palette should have the name of the folder it's located in.

Example with a .pal file (.png or .gpl will work the same way):
| **Filename**                      | **What happens**                                                                                         |
| --------------------------------- | ----------------                                                                                         |
| **animals/redpanda/m/m.pal**      | ZT Studio will use this palette for all male animations (e.g. animations/redpanda/m/eat/NE, NW, SE, SW). |
| **animals/redpanda/redpanda.pal** | ZT Studio will use this palette for all animations (also for female and/or young versions).              |
| **bjects/myobj/myobj.pal**        | ZT Studio will use this palette for all animations NE, NW, SE, SW).                                      |

Hint: if both **animals/redpanda/m/m.pal** and **animals/redpanda/redpanda.pal** are available, the male animations will use m.pal; while female and/or young versions would use redpanda.pal

Summary:
* If multiple color palettes are in the same folder, this is the preferred order: .PAL > .GPL > .PNG
  * .pal is original ZT1 extension; very clear it should get priority
  * .gpl is also clearly a palette
  * .png is more of a guess
* The palette which is relatively closest (in the folder path), will be used.
* If a folder only contains 1 graphic (such as an icon), the shared palette is ignored. 
  * This is done because an icon or plaque might include background colors which aren't used anywhere else. They would limit the amount of useful colors in the shared palette.


### Start the conversion
* Start ZT Studio
* Hit the Batch conversion-button. 
* Pick from .PNG to ZT1 Graphics. 
* Optional: Check the settings if everything's configured alright. 
* Just click the Convert button

