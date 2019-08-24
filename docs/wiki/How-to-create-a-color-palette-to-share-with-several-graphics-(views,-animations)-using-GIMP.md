Based on **GIMP 2.18** and the plugin **BIMP 1.17** (previous versions had bugs).
GIMP is a free commonly used graphic editor, BIMP is a plugin to perform batch operations.

**GIMP**

_Preparation_
* Create new image
* Drag and drop all .PNGs of the animal on the new image canvas (or better: do these steps separately for male, female if different than male, young and icons). After GIMP has added each image as a new layer, you should see the total amount of your .PNGs + 1 (empty layer).
* Hint: you can recover an additional color by removing the empty layer.

_Creating the color palette_
* Go to menu Image -> Mode -> Indexed.
* A dialog window opens. Choose _Color Palette_ and pick _Generate Optimum Palette_ . Assuming you rendered images with for example Blender or whatever, your background is not transparent, but it's a color (eg flashy green) which you'll want to turn into a transparent color at a later point. If this is the case, simply create a color palette of **256** colors.

_Using the color palette_
* You should now find a toolwindow named _Palettes_. It should contain something like _Colormap of Image #1 (untitled)_ . 
* Right click to rename and/or save.

_Batch process PNGs_
* We are using a free GIMP plugin called BIMP (Batch Image Manipulation).
* Add the folder containing only the graphics you're preparing (make sure to check it adds everything within that folder)
* Add a manipulation (action): _gimp-image-convert-indexed_ , choose CUSTOM_PALETTE and at the bottom specify the name of the color palette you've created/renamed/saved earlier
* be careful to set your Output Folder correctly. You do need to specify it explicitly! (not very intuitive)
* hit Apply 

**Hint:** set your background color in ZT Studio to the same one you're using in your images to be able to use the 255 (+1 transparent) color as intended!

Final note: if you are planning to simply use the generated GIMP Color Palette (.GPL) as the actual file / color palette to be used in the batch conversion feature of ZT Studio, you should make sure the background color (e.g. flashy green, the color you DON'T want to see in ZT1) is the first color in this palette!
