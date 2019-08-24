This list is a work in progress. Some details are still unknown or should be improved upon. Some information is still in the Word document in the master branch.


# Using this information? 
Please credit MadScientist and myself.

# Definitions 
* **ZT1:** Zoo Tycoon (the original game, but this includes the expansions and official bonus content). Dinosaur Digs (DD) and Marine Mania (MM) are expansions. The game, its expansion packs and nearly all official bonus content [with some exceptions] are known as the Complete Collection (CC). 
* **ZT1 Graphic:** this file tells the ZT1 game how/where to render pixels. Colors are stored in a separate but required ZT1 Color Palette file, which is usually - but not necessarily - stored in the same directory. A ZT1 graphic consists of at least 1 frame.
* **ZT1 Color Palette:** has a .pal-extension. Contains the colors used in a ZT1 Graphic.
* **ZT1 Frame:** 1 image/drawing/picture
* **ZT1 Animation:** this would be all views (individual graphics) making up an animation.
* **View:** for example an object usually consists of 4 views: NE (North East), NW (North West), SE (South East), SW (South West). An animal has more, an icon has just 1.

# ZT1 Graphic Formats 

There's a couple of variations on the ZT1 Graphic format. The main differences:
* **Non-animated. Basic.** Appears as a non-animated object. Example: lamp
* **Animated. Basic.** Example: fire
* **Animated. With additional background frame.** Example: the regular restaurant. This file format contains one frame which acts as a background to all the other frames. The building remains mostly the same, but some aspects (smoke, sign) are animated. This format is optimized because it can be rendered faster (only the changes) and since it's also smaller (compressed).
* **Shadows.** Introduced in Marine Mania. Simply put, there's no need for a color palette since only black is used. Example: dolphin's underwater shadow in certain animations.

There's another very minor difference.
Lots of graphics start with what reads as 'ZTAF' (probably ZT Animation File or something like that).
Sometimes these bytes are dropped and it doesn't seem to make a big difference (seen in graphics of certain objects, [mostly?] non-animated objects).

The graphic files have been documented for about 99%. There are only two single mysterious bytes left to decode.


# Explanation of the file formats 
* see Word document in the master branch

# Limitations 

* **Color Palette takes** at most 255 colors + 1 transparent color (= will be invisible in ZT1).
* A single **ZT1 Graphic** is thus limited to 255 colors + 1 transparent color. This means each view is limited to 255 colors. However, if they don't share their color palette, you can use more than 255 colors among all views.

To check:
* is there a file size limit?
* there's probably a limit (255?) of the number of drawing instructions in a pixel row.
* there's definitely an image dimension (height/width) limit - the Marine Mania showarea has one of the largest in game widths and might be a starting point. 
* other limits?


# Interesting graphics for testing
* **animals / dolphin's ssurfswi**: Marine Mania's compressed format for shadows. No ZTAF-bytes.
* **objects / bamboo**: basic. 1 frame. No ZTAF-bytes.
* **objects / cinema**: Dinosaur Digs. The *used* animation's SW-view comes to 250kb. It's most likely the graphic which contains the largest number of pixels which need to be drawn. (*verification needed*). The *idle* animation contains 1 frame and a background frame. It's different from the Restaurant: the frame renders some additional elements on the background frame. Again, it doesn't seem to be necessary to split this up. ZTAF-bytes.
* **objects / showbuoy**: Marine Mania. Largest width in game, but doesn't require a lot of pixels to be drawn. No ZTAF-bytes. Despite being animated.
* **objects / restrant**: the original Restaurant. Likely one of the earliest ingame graphics. ZTAF-bytes. The *used* animation contains a background frame, everything else is rendered on top. However, the *idle* animation is also fascinating. It applies the same principle, to no visible use. An empty frame (no width, height, offsets) is rendered on top of the background frame. 
* **objects / testbox**: Marine Mania. Why not use Blue Fang's test object? It should make the entire red grid/tile invisible. 

* combination of **animals/orngutan/m/rpgeton , rpgetoff, rpdangle, rpswing** and **objects/asirope/used** animations (Endangered Species / Complete Collection). Load the animal animation, and open the asirope's used animation as background. Great thinking on Blue Fang's part!

Furthermore, ZT Studio should be able to generate the exact same hex when you first convert ZT1 Graphics to PNG images, and then convert PNG images to ZT1 Graphics. The only difference you might have, should be the so called *mystery bytes* which we haven't deciphered yet. Main condition: leave the **original .pal files** where you'd expect them. Otherwise ZT Studio will generate a new color palette and the order/indexes of the colors might be slightly different. Tested with Bamboo.

For the Restaurant's *used* animation, safe for the speed of the animation and the mystery bytes, the only difference was a few bytes. It seems a color is used twice in the color palette. It seems the original Restaurant's graphics refer to the second index of the color in restrant.pal while ZT Studio uses the first one to be found. Since the Restaurant has a very rare case in its *idle* animation (see above), it will fail!

