Some preparations are in order, but you can save yourself quite some time and trouble. The batch conversion is optimized for a workflow. 

For people who use 3D software to create animals, they can usually generate .PNG images in batch straight from their 3D model with a specific file name pattern.

### Take care of your file and folder structure
* If you're unfamiliar with the ZT1 structures, take a look at existing objects, animals etc. For animals, you typically have a **folder structure** like animals/panda/m/walk . The animal name is usually shorted, each part of the path should be **8 alphanumeric characters max**. That's why you might see a folder structure like objects/restrant/idle .

* The **file names** of your .PNG-files should be like this: <view name>_00xx.PNG . For example: **NE_0000.png **. A second frame would be named NE_0001.png if you're working with animated objects or with animal animations. The ZT Studio settings allow you to specify whether you want to start numbering from 0 or from 1.

### Share a color palette or not?
This is an optional step, but ZT Studio offers a unique feature. Just like the official animals and objects do, you can share a color palette between each graphic's _animations_ - excluding the graphic's _icons_. It makes things a bit more complex and requires some additional preparation ( see specific page in this Wiki ). 
 

**Benefits?**
* It makes the total size of whatever animal/object/... you're creating, smaller. This is still very interesting for people who are hosting content. It used to matter a lot more in the past when internet connections were still way slower or with lower download limits. 
* It's also better for ZT1's performance. But again, PC's are way faster than they used to be. And people with non-optimized user created content will hardly benefit from your extra effort.
* You do have less files, making it look more structured. 
* Most importantly: recoloring is easier, you can simply change this one color palette and you've updated all this graphic's animations at once.

**Downsides?**
* Needs specific preparation ( see specific page in this Wiki )
* You are limited to 255 colors per graphic (colors in icons excluded!). This shouldn't be an issue: Blue Fang never needed separate palettes in this case. 

Check the ZT Studio settings to see if you've enabled this or not.

Let's say you do want a shared color palette. You will need to make sure your color palette is in the right place. Good news, for convenience this can be a .pal-file, a .PNG-file (16x16 pixels = 256 colors, the top left pixel will determine which color should be invisible [background]) or a .GPL-file (GIMP Color Palette, 256 colors,).

Before you start the conversion, you'd put the palette in the right folder. The palette should have the name of the folder it's located in.

Examples (you could easily just leave a GIMP Color palette/.gpl instead of a ZT1 Color palette/.pal):
* animals/redpanda/m/m.pal -> the batch process will use this ZT1 color palette for all male animations (e.g. animations/redpanda/m/eat/NE, NW, SE, SW).
* animals/redpanda/redpanda.pal -> the batch process will use this ZT1 color palette for all animations (also for female and/or young versions). 
* if both animals/redpanda/m/m.pal and animals/redpanda/redpanda.pal are available, the male animations will use m.pal; while female and/or young versions would use redpanda.pal 

* objects/myobj/myobj.gpl -> the batch process will use this ZT1 color palette for objects/myobj/idle/NE, NW, SE, SW.

If a folder only contains 1 graphic (such as an icon), the shared palette is ignored. This is done because an icon might include background colors which aren't used anywhere else. They would limit the amount of useful colors in the shared palette.

Deeper level color palettes take priority and are applied to all graphics in the same directory and below.

### The actual conversion
* Start ZT Studio and hit the Batch conversion-button. 
* Pick from .PNG to ZT1 Graphics. 
* Optional: Check the ZT Studio settings if everything's alright. 
* Just click the Convert button, sit back, relax.
