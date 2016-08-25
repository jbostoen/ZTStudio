
**Important note**

This is the code I've written in early 2015, after conceiving some ideas for a new unofficial expansion pack. But the most important thing realized, is this new ZT1 graphic editor.

It's still called Zoot.NET in the source which was the working title. It bears no affiliation with MadScientist's ZOOT tool, although big kudos to him for documenting a lot of the aspects of the file format.


**About ZT Studio**

ZT Studio is a tool for the original Zoo Tycoon game and its expensions (Dinosaur Digs + Marine Mania + nearly all official bonus content = Complete Collection).


**Scope of this project**

Read/write all possible ZT1 Graphic formats (there are a few variations, we'll document them).

Most important features:
* import from or export to a folder of .PNG-images (frame per frame OR in batch!)
* the only tool to support all identified ZT1 Graphic formats
* preview all graphics, including a way to preview variations (eg. when ingame you can choose the color of the roof)
* rotation fixing (move graphic up, down, left, right)

Todo:
* better document/comment code, clean up code, remove debug stuff. Overhaul code, it's not my best work since it was a lot of trial and error and there was no overhaul yet.
* feature to optimize/compress graphics (use background frame, remove APE junk bytes)
* finish or remove the GIMP recolor integration
* figure out the purpose of the last 2 unknown bytes

Other features include previewing how color, changing individual colors in the color palette (after a common method of 'recoloring', the shadow might not be black. You can easily make the shadow black again).

**Definitions**

* ZT1: Zoo Tycoon (the original game, but this includes the expansions and official bonus content)
* ZT1 Graphic: this file tells the ZT1 game how/where to render pixels. Colors are stored in a separate but required ZT1 Color Palette file, which is usually - but not necessarily - stored in the same directory.
* ZT1 Color Palette: has a .pal-extension. Contains the colors used in a ZT1 Graphic.
* ZT1 Frame: 1 image/drawing/picture
* ZT1 Animation: 1 or more frames


**ZT1 Graphic Formats**

There's a couple of variations on the ZT1 Graphic format.
* **Non-animated. Basic.** Appears as a non-animated object. Example: lamp
* **Animated. Basic.** Example: fire
* **Animated. Background frame.** Example: the regular restaurant. This file format contains one frame which acts as a background to all the other frames. The building remains mostly the same, but some aspects (smoke, sign) are animated. This format is optimized because it can be rendered faster (only the changes) and since it's also smaller (compressed).
* **Shadows.** Introduced in Marine Mania. Simply put, there's no need for a color palette since only black is used. Example: dolphin's underwater shadow in certain animations.

The graphic files have been documented for about 99%. There are only two single mysterious bytes left to decode.




**History**

There was an official editor for objects and animals called APE - dealing with both the graphical aspect as well as creating/copying the configuration of existing animals/objects. Some users improved APE a bit to support the expansion packs. While APE has its merits, it also had severe shortcomings when it comes to the graphical side. It also generates ZT1 Graphics with a lot of redundant bytes. 

Next came MARKHOR, to deal with a notorious issue: lots of things created in APE, didn't show up in the right position in the ZT1 game. You had to do 'rotation fixing' (moving the graphic up/down/left/right). APE did also not support large graphics and crashed on quite a few objects/animals. Also, the copied configuration is a big mess.

Afterwards, we had ZOOT. A big shoutout to someone I know only by the nickname MadScientist for explaining a lot of the ZT1 Graphic format (but not all of it). ZOOT allowed us to see most formats, but couldn't handle the graphic format with a background frame nor the shadows.
