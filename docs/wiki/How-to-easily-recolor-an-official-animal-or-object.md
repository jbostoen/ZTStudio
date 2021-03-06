This is only meant as a short tutorial for 99% of the original animals/objects (I vaguely remember a few exceptions).

Previously you'd have to either recolor all the actual images to get a proper color palette at the very end or perform some magic similar to the method I described to create such a shared color palette - and then the biggest trick was to make sure you kept the original order of the colors.

Start from the original animal game files, not from what APE generated.

**Prepare**

* Always copy the original files (for example, all files from T-Rex), keep the naming first.
* Now you have for instance c:\temp\animals\trex (or something like that), which contains all original .pal files and icons and animations


**Recolor**

* open any animation now from trex
* on the right side of the main window, right click the color palette, choose to export it (to .PNG for instance)
* recolor the palette in any program you'd like
* very important step: go to Settings -> Palette tab -> make sure ZT Studio is forced to add all colors (even duplicates)
* on the right side of the main window, right click the color palette and choose import from PNG
* see if your T-rex looks nice enough or repeat the recolor-steps.

Hint: make sure real/complete black stays black (= shadow for the animal)

**Rename (to unique name)**

* Use ZT Studio to convert all files from ZT1 to .PNG (batch conversion)
* Rename the folder and trex.pal to for example mytrex and mytrex.pal
* Use ZT Studio to convert all files from .PNG to ZT1 (batch conversion)


Why? This allows you to not have single .pal-files for each single view in each single animation (= the crappy APE way)
But, why? Having one color palette allows you to simply recolor this; and all animations will be recolored at once.

**What's left?**

In most (all?) cases, icons and plaques use a separate color palette file. Adjust that graphic manually, recoloring is a bad idea here because these color palettes also contain the background of the icon.


**Configuration**

Again, I never use APE because it generates utter crap. I always start from the original .AI-file from the animal.
But if you really want to, I think it's safe now to use APE to adjust those settings, as long as you don't do any more graphic manipulations.




