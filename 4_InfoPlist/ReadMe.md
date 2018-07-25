# INFO.PLIST

## INSCRUCTIONS
###### Run ImportPlistFromYourBuild if you don't an Info.plist
Will take the Info.plist from 1_MyBuild/YOUR.APP/Contents/Info.plist and paste it here. If there is already one in this folder it will make a backup copy just to be sure.

###### Open your Info.plist @ 4_InfoPlist/
Don't open in an editor, just double click and open with Xcode so you have access to the dropdown values to avoid typo's. 

In the examples folder you can find an empty example (that we used) for reference.

### Now check these values

| KEY | VALUE |
|:--|:--|
|Bundle identifier|**COM.COMPANY.APPNAME** Its the one you created in the beginning in the [Apple Developer Portal](https://developer.apple.com/account/mac/certificate/development)  |
|Bundle name| **App name**|
|GetInfoString | **The copyrights** to your game instead of Unity's |
| BundleSignature | ~~4 letter creator code~~, Is [**outdated**](https://stackoverflow.com/a/1898662) We didn't need it. |
| ApplicationCategory | **Your category** In Unity 2017.3 this seems to be ok, but you can avoid typo's using the dropdown |
| Localization native development region | **The development language of the bundle**.|
|ShortVersionString | **Version** Build or version number should always be increased each time you upload to the Appstore as you cannot upload a package with identical version & build [More on Version & build](https://stackoverflow.com/a/19728342)|
|BundleVersion| **Build number**. Used to upload packages to the Appstore that you do not want to show in your public version. Can be the same as ShortVersionString, but if you want to change your screenshots at the Appstore you will need to send them a new package, with build you could hide this new version without increasing your version number.|
|App Uses Non-Exempt Encryption| **TRUE/FALSE** - A.K.A "App is using encryption that is exempt from [EAR](https://www.bis.doc.gov/index.php/encryption-and-export-administration-regulations-ear)" **Before you scare read** [Short answer](https://stackoverflow.com/a/46691541), [this for Unity](http://answers.unity.com/answers/669794/view.html), and [this by Unity](https://forum.unity.com/threads/us-export-compliance-encryption.389208/#post-2893835) |
|CFBundleSupportedPlatforms|**MacOSX** Without this the application loader seems to default to iOS. [Read More by N3uRo](https://forum.unity.com/threads/the-nightmare-of-submitting-to-app-store-steps-included-dec-2016.444107/)|

### Run “CopyPlist” 
Will replace the Info.plist in your build with the one you just made.

| FROM | TO |
|:--|:--|
| 4_InfoPlist/Info.plist | 1_MyBuild/YOUR.APP/Contents/Info.plist |

 

### Run PluginsReplaceBundleId
Takes the Info.plist you just made and finds your BundleIdentifier. Opens all bundles in the plugins folder and replaces the BundleIdentifier value with yours. 

* Even the Unity services bundles need your BundleIdentifier, otherwise you will get errors when uploading to the Appstore.
* **[QUOTE N3uRo](https://forum.unity.com/threads/the-nightmare-of-submitting-to-app-store-steps-included-dec-2016.444107/)**
Edit other "*.bundle"s Info.plist that has a "CFBundleIdentifier" to point to your identifier also. I had a problem with AVPro that had it's own identifier that was not valid.

## WHY
Every app and plug-in uses an Info.plist file to store configuration data in a place where the system can easily access it. macOS and iOS use Info.plist files to determine what icon to display for a bundle, what document types an app supports, and many other behaviors that have an impact outside the bundle itself.
[More info on Info.plist @ Apple](https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Introduction/Introduction.html)

1. Unity does not update all fields
2. Every time you build in Unity the file is overwitten so you need to keep a copy here that you can replace every build.
	- [**QUOTE “Matthias @ Gently Mad“**](https://gentlymad.org/blog/post/deliver-mac-store-unity)
Save the Info.plist inside your package and don’t forget to create a copy since Unity will overwrite the file the next time you make a build!

## DIY Info.plist
1. Create your manually updated Info.plist and replace the one in YOUR_BUILD/Contents/
2. So open YOUR_BUILD/Contents/Plugins/each_bundle/Info.Plist
And replace the BundleIdentifier with yours.