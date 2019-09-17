# Testing ZT Studio

These are notes on testing ZT Studio's capabilities and potential differences towards the actual game.

## Graphics

ZT Studio should be able to generate the exact same hex when you first convert ZT1 Graphics to PNG images, and then convert PNG images to ZT1 Graphics. 
The only difference you might have, should be the so called *mystery bytes* (not deciphered yet). 

Main condition: keep the **original .pal files**.
Otherwise ZT Studio will generate a new color palette and the order/indexes of the colors might be slightly different. 
Tested with Bamboo.

For the Restaurant's **used** animation, safe for the speed of the animation and the mystery bytes, the only difference was a few bytes. 
It seems a color is used twice in the Restaurant's original color palette. 
It seems the original graphics use second index of the color in restrant.pal.
Because of this, ZT Studio now uses the index of the last occurrence of a color in a palette.

The Restaurant's **idle** animations used an empty frame, causing PNG to ZT1 Graphics to fail. This may have been resolved by now.
