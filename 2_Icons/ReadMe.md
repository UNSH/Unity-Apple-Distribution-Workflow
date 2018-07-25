# ICON GENERATION

## INSCTRUCTIONS
### I already have an icon & UnityPlayerIcon.png
If you have created an PlayerIcon.icns & UnityPlayerIcon.png and want to use those just put it them in the folders described below and Run “OSX Icon" to copy them to Contents/resources in your build. 

| WHAT | DIR |
|:--|:--|
| PlayerIcon.icns | 2_Icons/MyOsxIcon/ |
| UnityPlayerIcon.png | 2_Icons/MyUnityPlayerIcon/ |

### I don't have an Icon
Either use this icon as a base or:

1. Create a PNG file of 1024x1024 pixels.
2. Name the file “Icon1024.png”
3. Make sure it has an alpha channel.
4. Replace your file with the dummy in this folder
5. Run “OSX Icon”

## WHAT THE SCRIPT WILL DO
1. Check and set the color profile to **sRGB IEC61966-2.1** 
2. Double check if your file has the correct size, alpha channel, is square, is png)
3. Create an iconset from your image. With correct naming convention and size. 
4. From that iconset it will create an PlayerIcon.icns file.
5. Create a UnityPlayerIcon.png in correct size (64x64)
6. Copy both “UnityPlayerIcon.png” and “PlayerIcon.icns” to *“1_MyBuild/YOUR.APP/Contents/Resources”* and replace what unity made.

###### Problem 
On High Sierra there is no way to create an icns file that includes all 10 required sizes through icon util (Everything but 16X16@1x and 32x32@1x). The only way to do this is to run this script on an older OS. I believe I did it with Sierra. You can open .ICNS files with preview to double check. I am not sure if this is a problem though. I have read not all are necessary. But if you are obsessive on details like me there is no other way. Believe me I have tried it all. Nothing worked besides running the IconUtil on an older OS.

## WHY
1. UnityPlayerIcon.png doesn’t get updated by Unity 
2. The .icns file Unity creates does not include all needed sizes
3. Every time you build both of the above are overwritten by Unity.

## DIY ICONS
1. Create a dummy project in Xcode and copy .ICNS file after build. It did not work for me (High Sierra). My ICNS file only included 4 sizes of the 10. To create all the needed image files you could still use this script. When prompted for delete clutter just enter "n". You will find an iconset with all the sizes here.
	- **QUOTE ["BrainAndBrain"](https://forum.unity.com/threads/where-is-up-to-date-info-on-making-mac-app-store-build.423330/)** " Create icns file using XCode: make a dummy project, open Assets.xcassets, and fill in the information. Build the app and open it to extract the icns file.
2. Create a 64X64@72dpi png file of your icon.
3. Place your ICNS file and UnityPlayerIcon.png in YOUR_APP/Contents/Resources/

###### OSX Icon Files

| Filename | Pixel size |
|:--|:--|
| icon_512x512@2x.png | 1024 x 1024 @ 144 dpi |
| icon_512x512.png | 512 x 512 @ 72 dpi |
| icon_256x256@2x.png | 512 x 512 @ 144 dpi |
| icon_256x256.png | 256 x 256 @ 72 dpi |
| icon_128x128@2x.png | 256 x 256 @ 144 dpi |
| icon_128x128.png | 128 x 128 @ 72 dpi |
| icon_32x32@2x.png  | 64 x 64 @ 144 dpi |
| icon_32x32.png | 32 x 32 @ 72 dpi |
| icon_16x16@2x.png | 32 x 32 @ 144 dpi |
| icon_16x16.png | 16 x 16 @ 72 dpi |

