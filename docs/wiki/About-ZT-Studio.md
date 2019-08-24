

# What is ZT Studio?

ZT Studio is a tool for viewing/manipulating the original Zoo Tycoon game and its expensions (Dinosaur Digs + Marine Mania + nearly all official bonus content = Complete Collection), as well as creating images from scratch.

# Scope of this project


## Most important features
* read all possible ZT1 Graphic formats (there are a few variations, we'll document them, we're the first and only tool for ZT1 to support them all at this point!).
* write ZT1 Graphics (traditional formats only for now)
* import from or export to a folder of .PNG-images (frame per frame OR **in batch**!) ( ZT1 Graphic <=> .PNG files )
* preview all graphics, including a way to preview variations (eg. when you can choose the color of the roof in the game) and adding a background graphic (eg to see the Orang Utan's play animation combined with the actual toy)
* rotation fixing (move graphic up, down, left, right) - immediately applied to all frames in a view or entire animation, unless explicitly set otherwise
* modify (add, delete, change) color palettes
* reorder color palettes (necessary to create color customizable items, e.g. roof) or change a color in the palette
* import/export color palettes to .PNG, export color palettes to .GPL (GIMP Palette)
* share color palettes among graphics (in batch conversions) 
* create .ani file


## To-do
* feature to optimize/compress graphics (use background frame, remove APE junk bytes)
* figure out the purpose of the last 2 unknown bytes

 
 
## Not in the scope
* configuration editing of .ai, .cfg, .uca, .ucs, ....-files. The game is simply not popular enough anymore to put extra effort in it. And quite a few things have never been probably figured out anyway.
* graphic manipulations. ZTStudio is NOT a paint program.

# History
There was an official editor for objects and animals called **APE**. It was capable of creating/copying the configuration and graphics of existing animals/objects, so users could create new content. Some users improved APE to support the expansion packs. While APE has its merits, it also had severe shortcomings when it comes to the graphical side. It also generates ZT1 Graphics with a lot of redundant bytes. As for the configuration, the settings of the original animal were copied, but APE made a mess of it.

Next came **MARKHOR**, to deal with a notorious issue: lots of things created in APE, didn't show up in the right position in the ZT1 game. You had to do 'rotation fixing' (moving the graphic up/down/left/right). APE did also not support large graphics and crashed on quite a few objects/animals. Also, the copied configuration is a big mess.

Afterwards, we had **ZOOT**. A Java application (.jar file). A big shoutout to someone I know only by the nickname MadScientist for explaining a lot of the ZT1 Graphic format (but not all of it). ZOOT allowed us to see most formats, but couldn't handle the graphic format with a background frame (such as the Restaurant) nor the shadows (dolphin in Marine Mania).

That's where I stepped in. Very late, only at the beginning of 2015, I created a program which improves upon ZOOT. One of the main features would be **batch conversion**. Easily put: if .PNG files are in the right folders with a certain pattern in their name, our program would automatically create a ZT1 Graphic out of it. This spares us the tedious process of importing frame by frame. This was done because I had plans for a new unofficial expansion pack, although the game is more than 10 years old now. There's still a small community left though, although it's definitely not as vibrant as it used to be. Meeting a designer who also looked back at ZT1 and who wanted to create new animals in **Blender** (open source/free 3D program) and import them into ZT1, the process accelerated a bit.

Some first results:
* Umbrella Table with a roof where you choose the 2 colors
* Red Panda (designed by Vondell)

 