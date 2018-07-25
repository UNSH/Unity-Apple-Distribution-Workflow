# DELETE .META FILES
## What you need to do

### Run “Delete Meta Files” 
The script will go into *“1_MyBuild/YOUR.APP/Contents/Plugins”* and find & delete any “.meta” files in your bundles.

## WHY
Every time you build Unity adds meta files to the plugins which give problems if they get signed.

[**CREDIT / QUOTE "Zwilnik @ Strange flavour"**](http://www.strangeflavour.com/creating-mac-app-store-games-unity/)
Firstly, as I mentioned, Unity has an annoying issue with its file system. Anything in your project folders gets a .meta file created for it (I’m assuming to help track it within the Unity editor). This is normally fine as long as you remember to build your target app *outside* your project folder, except for when you use any plugins.. 

Because your plugins are stored within the project folder, their bundle files all have .meta files in them. When it comes to signing, 
these meta files break the signing and as they’re data files in the root of the bundle (which is bad).

### DIY delete meta files
Open *"YOUR_BUILD/Contents/Plugins/"* open each bundle and delete all the meta files in there.
