

# What is ZT Studio?

ZT Studio is a graphical tool for viewing/manipulating the original Zoo Tycoon game and its expensions (Dinosaur Digs + Marine Mania + nearly all official bonus content = Complete Collection), as well as creating images from scratch.

# Scope of this project

## Most important features
* read all possible ZT1 Graphic formats (there are a few variations, we'll document them, we're the first and only tool for ZT1 to support them all at this point!).
* write ZT1 Graphics (traditional formats only for now)
* import from or export to a folder of .PNG-images (frame per frame OR **in batch**!) ( ZT1 Graphic <=> .PNG files )
* preview all graphics, including a way to preview color customization (for example when you can choose the color of the roof in the game) and adding a background graphic (for example to see the Orang Utan's play animation combined with the actual toy)
* offsetting ("rotation fixing") (move graphic up, down, left, right) - immediately applied to all frames in a view or entire animation, unless explicitly set otherwise
* modify color palettes: add, delete, change, reorder colors, import/export to .PNG or .GPL (GIMP Palette)
* share color palettes among graphics (in batch conversions) 
* create .ani file


## To-do
* figure out the purpose of the last 2 unknown bytes
* other ideas, feature requests, bug reports: see Issues on GitHub
 
 
## Not in the scope
* configuration editing of .ai, .cfg, .uca, .ucs, ....-files. The game is simply not popular enough anymore to put extra effort in it. And quite a few things have never been probably figured out anyway.
* graphic manipulations. ZTStudio is NOT a paint program.


# Support
There is no support for this program. Bug reports, feature requests or pull requests may be processed at some point; but it's not guaranteed at all.

* [Report issues/request features](https://github.com/jbostoen/ZTStudio/issues)
* Be as detailed as possible when reporting bugs; preferably include the files causing the problem you’re experiencing.


# Credits

Please credit in this order:
* **MadScientist** for figuring out most of the common ZT1 graphic format
* **Jay** for some additional info on the ZT1 graphic format
* **Jeffrey Bostoen** for creating ZT Studio, documentation, figuring out how the background frame and MM shadow format works
* **Vondell** for providing demo/test graphics
* **HENDRIX** for some contributions to the source code


# History
There was an official editor for objects and animals called **APE**. 
It was capable of creating/copying the configuration and graphics of existing animals/objects, so users could create new content. 
Some users improved APE to support the expansion packs. While APE has its merits, it also had severe shortcomings when it comes to handling graphics.
It also generates ZT1 Graphics with a lot of redundant bytes. As for the configuration, the settings of the original animal were copied.
APE did not support large graphics and crashed on quite a few objects/animals. Also, the copied configuration is a big mess.

Next came **MARKHOR**, to deal with a notorious issue: lots of things created in APE, didn't show up in the right position in the ZT1 game. 
Content creators had to do 'rotation fixing' (moving the graphic up/down/left/right; so configuring offsets). 

Afterwards, MadScientist released **ZOOT**. A Java application (.jar file). 
ZOOT allowed users to see most ZT1 Graphics, but couldn't handle the graphic format with a background frame (such as the Restaurant) nor the shadows (dolphin in Marine Mania).
MadScientist was kind enough to share most information that had been discovered at that point.

That's where I (jbostoen) stepped in. Very long after the game's original release, I created a program which improves upon ZOOT. 
ZT Studio was first released at the beginning of 2015.
One of the main features planned was **batch conversion**. 
Easily put: if .PNG files are in the right folders with a certain pattern in their name, 
this program would automatically create a ZT1 Graphic out of it. This spares us the tedious process of importing frame by frame. 
This was done because I had plans for a new unofficial expansion pack, although the game is more than 10 years old now. 
There's still a small community left though, although it's definitely not as vibrant as it used to be. 
Meeting a designer (**Vondell**) who also looked back at ZT1 and who wanted to create new animals in **Blender** (open source/free 3D program) and import them into ZT1, the process accelerated a bit.

Some first results:
* Umbrella Table with a roof where you choose the 2 colors
* Red Panda (designed by Vondell)


## License
https://www.gnu.org/licenses/gpl-3.0.en.html
Copyright (C) 2015 Jeffrey Bostoen


 