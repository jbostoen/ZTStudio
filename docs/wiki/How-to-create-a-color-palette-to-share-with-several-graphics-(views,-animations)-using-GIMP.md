# How to create a color palette to share with several graphics (views, animations) using GIMP

Based on **GIMP 2.18** and the plugin **BIMP 1.17** (previous versions had bugs).
GIMP is a free commonly used graphic editor.
BIMP is a plugin to perform batch operations.


**Preparation**
* Create new image in GIMP
* Drag and drop all PNG graphic files of the animal on the new image canvas
  * Do this separately for icon(s) and plaque.
  * If you're planning to have a different female (or young) color; also use a separate series of PNG graphic files to create a separate palette.
* After GIMP has added each image as a new layer, the amount of layers is the same as the total amount of PNG Graphic files + 1 (empty layer).

Hint: 
* Remove the empty layer from the start.


**Creating the color palette**
* In GIMP, go to menu Image -> Mode -> Indexed.
* A dialog window opens. Choose _Color Palette_ and pick _Generate Optimum Palette_ . 
  * If the background is not transparent, you'll want to turn into a transparent color at a later point. If this is the case, simply create a color palette of **256** colors.

**Using the color palette**
* In GIMP you should now find a toolwindow named _Palettes_. It should contain a palette named like _Colormap of Image #1 (untitled)_ . 
* Right click to rename and/or save this palette.

**Batch process PNGs**
* There is a free GIMP plugin called BIMP (Batch Image Manipulation).
* Add the folder containing only the specific graphics (make sure to check it adds everything within that folder)
* Add a manipulation (action): _gimp-image-convert-indexed_ , choose CUSTOM_PALETTE and at the bottom specify the name of the color palette you've created/renamed/saved earlier
* Be careful to set the Output Folder correctly. You do need to specify it explicitly! (not very intuitive)
* Hit Apply 

Hint:
* Set the background color in ZT Studio to the same one used in the PNG Graphic files to be able to use the maximum of 255 (+1 transparent) colors as intended!

Final note: if you'll use the GIMP Color Palette(s) (saved as .GPL) generated during the first steps (Preparation) during the batch conversion with ZT Studio, then make sure the background color (e.g. flashy green, the color that should be transparent in ZT1) is the first color in this palette!

Time to use these color palette(s)!
* [How to convert .PNG images to ZT1 Graphics in batch](How-to-convert-.PNG-images-to-ZT1-Graphics-in-batch)
