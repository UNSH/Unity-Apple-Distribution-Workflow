# UNITY APPLE DISTRIBUTION WORKFLOW

Workflow to automate and guide people in delivering Unity builds inside or outside of the Appstore.

**CAUTION** building for outside the Appstore may still need some work.

[UNITY THREAD](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/)

## Instructions
### You're new at this?
For a first build just follow all the steps. In each folder you will find instructions to deal with the problem at hand in chronology. After you finished the steps a first time you can just run "RepeatForUpdatedBuild" to quickly repeat the whole process. But you **need** to finish the steps so all the data used is correct otherwise problems will arise. 

### You've been here for a while?
If you already have all the necessaries you can place them in their respective directories, following the table below and run "RepeatForUpdatedBuild".  The development profile is placed in the Appstore dir, because you only need it to test Appstore features.

| WHAT | DIR |
|:--|:--|
| DEV.provisionprofile | 0_BeforeYouBuild/ProvisioningProfiles/Appstore/Development/  |
| APPSTORE_DIST.provisionprofile | 0_BeforeYouBuild/ProvisioningProfiles/Appstore/Distribution  |
| DEV_ID_DIST.provisionprofile | 0_BeforeYouBuild/ProvisioningProfiles/DeveloperID/  |
| YOURGAME | 1_MyBuild/ |
|PlayerIcon.icns|2_Icons/MyOsxIcon/|
|UnityPlayerIcon.png|2_Icons/MyUnityPlayerIcon/|
| Info.plist | 4_InfoPlist/ |
| entitlements.plist | 5_Entitlements/ |

## Scripts
All scripts (and todos) are described in more detail in each folder. As everything is automated there are also failsafes in place for overwriting or to check if needed files are present. We will not describe all of those, but you can view the script to see what is happening.

###### OSX Icon 
Creates PlayerIcon.icns & UnityPlayerIcon.png from single png file and placed them in BUILD/Contents/resources/

###### Delete Meta Files
Script to Delete meta files in *"BUILD/Contents/Plugins/plugin_x"*

###### ImportPlist
Info.plist is copied from *"BUILD/Contents/"* to *"4_InfoPlist/"* so you can update it manually once and reuse it after.

###### CopyPlist
Copies your manually updated Info.plist from *"4_InfoPlist/"* to *"BUILD/Contents/"*

###### PluginsReplaceBundleId
Opens all bundles in the plugins folder and replaces the BundleIdentifier value with yours (taken from *"4_InfoPlist/Info.plist"*).

###### Entitlements.plist templates
Self explanatory.

###### SignAndPackage
- Create a version folder structure for your builds. 
- Copies the correct provisioning profile as embedded.provisionprofile to *"BUILD/Contents/"* depending on Appstore, Development or Distribution choice.
- Signs all code with your entitlements. Loops over bundles in plugins, finds .dylib files in Frameworks, Deep signs app.
- Verify
- Builds signed package for either Appstore, Development or Distribution outside Appstore (installer or zip)

### RepeatForUpdatedBuild
Script that calls all the other scripts to speed up preparing updates and creating new builds. 

## PM me to contribute
As we will not be constantly uploading games to the App Store it might be good to have other people pitching in so that there's a central point to get help that doesn't age. So anyone who wants to become a contributor just pm me on git even if it is to just add some documentation.

## Credit
We do not take credit for anything besides creating this workflow, tying the code we found together in bash with added automation and organising & rehashing documentation. Below are the credits for all the posts, threads we used. If we missed a spot please contact us. 

We created this workflow because delivering a Unity build to Apple is a mess and the answer is spread across several websites which makes it hard to wrap your head around the correct final chronology especially if you're new or on Windows.

There are tools out there like [Signed](https://assetstore.unity.com/packages/tools/utilities/mac-app-store-signed-54970) that will also take care of some of this, but often it's better to understand and not depend upon 3rd party assets, especially when things go wrong. We have tried to include paths, actions and DIY as much as possible so you can understand what's happening and where everything is supposed to go.

### ALL THE BRAVE SOULS WHO PAVED THE ROAD.

| WHO | WHAT | HOME |
|:--|:--|:--|
| N3uRo | [UNITY THREAD](https://forum.unity.com/threads/the-nightmare-of-submitting-to-app-store-steps-included-dec-2016.444107/) | [Unity Assetstore Page](https://assetstore.unity.com/publishers/18584) |
| Zwilnik | [GUIDE](http://www.strangeflavour.com/creating-mac-app-store-games-unity/) | [Strangeflavour.com](http://www.strangeflavour.com) |
| Victor Leung | [MEDIUM GUIDE](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412) | .. |
| Matthias | [GUIDE](https://gentlymad.org/blog/post/deliver-mac-store-unity) | [Gentlymad.org](https://gentlymad.org) |
| Dilmer Valecillos | [GUIDE](https://www.dilmergames.com/blog/2017/03/29/unity3d-how-deliver-application-apple-mac-store/) |[www.dilmergames.com](https://www.dilmergames.com)|
|BrainAndDrain|[UNITY POST/GUIDE](https://forum.unity.com/threads/where-is-up-to-date-info-on-making-mac-app-store-build.423330/#post-2829466)| [Brainandbrain.co](http://brainandbrain.co/about) |
| Alamboley | [CODESIGNING GIT](https://github.com/DaVikingCode/FromUnityAppToMacAppStore) | [Davikingcode.com](http://davikingcode.com/) |
| JoeStrout | [POST BUILD,SIGN, ZIP](https://forum.unity.com/threads/osx-code-signing.455830/) | [Stroutandsons.com](http://stroutandsons.com) |
| Henry | [ICON GEN](https://stackoverflow.com/a/20703594) |..|
| TranslucentCloud | [PIXEL GRID](https://stackoverflow.com/a/39678276) |..|

### WORKFLOW
Worked on this workflow? Add yourself! Btw you can use the *"doc/CombineAllReadmeIntoDoc*" to recompile all readme's for the online manual page.

| WHAT | WHO | HOME |
|:--|:--|:--|
| ORGINAL WORKFLOW | UNSH | [UNSH.IO](https://unsh.io) |
|[Post](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/#post-3604213) | Atorisa | [Assets](https://assetstore.unity.com/publishers/17426) |
| post | Your name | page | 



 


# BEFORE YOU BUILD
Just place this workflow somewhere outside of your Unity project.

## INSTRUCTIONS (APPLE)
### Create Apple Developer Account 
If you do not already have one go to [Apple Developer portal](https://developer.apple.com/) and pay 100$ to get confused by professionals. And you might as well enable **AUTO RENEW** in the membership tab at [Apple Developer portal](https://developer.apple.com/). So you avoid surprises. 

**SELLING ON APPSTORE?** If you are not releasing a free app/game and want to get paid outside of the US? Go fix the banking and tax first at [Appstore Connect](https://appstoreconnect.apple.com) >> Agreements, Tax and banking. It's not necessarily something that has to happen now, but get it over with if you are about to release. Prepare for some serious legal and tax lingo.

### Create the Certificates you need
Go to the [Apple Developer Portal](https://developer.apple.com/account/mac/certificate/development) and create your certificates. Depending on where you want to release your game your need different certs. These serve as an identity that will be added to your iCloud keychain and allow you to codesign and create your provisioning profiles later. [More on certificate names](https://stackoverflow.com/a/13603031)

**IMPORTANT** In the creation process the field "common name" will name your keychain. Make sure you name your certificates in a way you can recognise them later. Its not a disaster if you don't but it does make your keychain a bit more clear if problems arise later. So for example "TEAM_Mac Installer Distribution". 


**IMPORTANT** Always make sure you use the correct certificates and provisioning profiles. If you have signed your app with old certificates (and provisioning profiles) you will need to download these again from the member center. **CREDIT** [Atorisa](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/#post-3604213)

#### Selling on the APPSTORE
1. Mac Installer Distribution
2. Mac App Distribution
3. Mac Development (for a test build)

#### Selling outside the Appstore
1. Developer ID Application
2. ~~Developer ID Installer~~?

### Create an identifier 
[Apple Developer Portal Go to App Identifiers](https://developer.apple.com/account/mac/identifier/bundle/). Examples : COM.COMPANY.PRODUCT, UNITY.COMPANY.PRODUCT, ... It's up to you. Note that you cannot disable In App Purchase. IAP is always enabled, but if you don't implement any button or script in Unity you can just ignore this. [More info on bundle identifiers here](https://cocoacasts.com/what-are-app-ids-and-bundle-identifiers/)

### Create a new app in iTunes Connect
Go to [iTunes connect](https://appstoreconnect.apple.com) and create a new OSX app.

**[QUOTE Victor Leung](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)** Login to iTunes Connect, choose My Apps > “+” > “New Mac App”, fill in the values and choose the bundle ID matches with the previous step. The prefix field would be the game name, such us ufo in my case. 

### Create your provisioning profiles
The type of provisioning profile depends on your selling platform (in -or Outside Appstore). To test Appstore features like Game Center you will need to create a separate provisioning profile for Appstore development as the distribution build will not allow testing.

| FOR | NAME PROVISIONING PROFILE |
|:--|:--|
| Testing | Mac App Development |
| Distribution Appstore | Mac App Store |
| Distribution Outside Appstore | Developer ID |

Go to the [Apple developer portal](https://developer.apple.com/account/mac/profile/). Just follow the instructions, if you have more problems follow [this tutorial](https://help.apple.com/developer-account/#/devf2eb157f8). Also **give a clear name to your profiles** when you download them so you do not make mistakes later. 

###### The first time 
you will be asked to add a device and give the UUID of this device you can find this here: About this Mac > System Report > Hardware overview (Hardware UUID: XXXXXXXXX)

### Place Provisioning Profiles in their respective folders
These will be copied later into your app as embedded.provisionprofile ( dependant on the build you chose).

| PROFILE | FOLDER |
|:--|:--|
| Development | 0_BeforeYouBuild/ProvisioningProfiles/Appstore/Development/MY.provisionprofile |
| Appstore| 0_BeforeYouBuild/ProvisioningProfiles/Appstore/Distribution/MY.provisionprofile |
|Outside Appstore| 0_BeforeYouBuild/ProvisioningProfiles/DeveloperID/MY.provisionprofile |

**QUOTE** "Zwilnik @ [Strange flavour](https://www.dilmergames.com/blog/2017/03/29/unity3d-how-deliver-application-apple-mac-store/)"
Another key step is to include a copy of the provisioning profile in the app bundle before signing it. It goes in the app bundle at Contents/embedded.provisionprofile.  Again, this is something Xcode would do for you normally that you have to do manually when building with Unity.  Do this for both development and distribution builds including the correct development or distribution profile.

### Download & Install Xcode from the App Store if you haven't already.
[Xcode is actually required](https://forum.unity.com/threads/failed-to-create-il2cpp-build-on-osx.530824/) ( and has to be installed at /Applications ) for a IL2CPP build. If you have more versions of Xcode and run into problems [Read This by Hogwash](https://forum.unity.com/threads/failed-to-create-il2cpp-build-on-osx.530824/#post-3508248)

You will also need it to open your .plist files later. Or alternatively to create your icon without the scrips.

## INSTRUCTIONS UNITY

### Add OsxResolutionFix.cs to your build
Add to any GO that will live @ startup to fix retina on large screens.

**CREDITS** 
[https://gentlymad.org/blog/post/deliver-mac-store-unity](https://gentlymad.org/blog/post/deliver-mac-store-unity)

[Original Unity Doc](https://docs.unity3d.com/Manual/HOWTO-PortToAppleMacStore.html)

### Double check in Project Settings > Player Settings 
If you want to build for outside the Appstore, make sure you uncheck Mac Appstore validation in your build.

1. Mac Appstore validation = true
2. Default is Full Screen = true
3. Default is Native Resolution = true
4. Capture Single Screen = false
5. Display Resolution Dialog = false

You can enter all of the other OSX values later in your Info.plist which is more typo proof as it has dropdown for certain values.

[**CREDITS / READ MORE Victor Leung**](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)

### Make sure:
- If you are building for more platforms not to include links or references to them.
- If for example you sell on iOs with IAP and on OSX you have a fixed price, then remember to disable the IAP buttons.
- Make sure all buttons are big enough and everything is accessible. ([Apple guidelines](https://developer.apple.com/design/human-interface-guidelines/))
- Double check if all your buttons work.
- [Read this](https://developer.apple.com/app-store/review/rejections/)

### Build your game and place your build in “/1_MyBuild”
So that's *"1_MyBuild/MYGAME"*
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
And replace the BundleIdentifier with yours.# Entitlements

## WHAT YOU NEED TO DO
### Again open this entitlements.plist with XCode not in text editor. 
Change it and basically describe what your app will need. At the very least it always needs *com.apple.security.app-sandbox set to yes*.

* [Find more info on all keys here.](https://developer.apple.com/library/archive/documentation/Miscellaneous/Reference/EntitlementKeyReference/Chapters/EnablingAppSandbox.html#//apple_ref/doc/uid/TP40011195-CH4-SW5)
* There are 3 examples in the _Examples folder as a reference.
* You can find **com.apple.application-identifier** & **com.apple.developer.team-identifier** in your provisioining profile. 
* Alternatively TeamID can also be found in [Membership tab of Apple Developer Portal](https://developer.apple.com/account/#/membership/)

| KEY | VALUE  | WHEN NEEDED |
|:--|:--|:--|
|com.apple.security.app-sandbox| **YES**| Always |
| com.apple.application-identifier |**TeamID.COM.COMPANY.GAME**  | If Game Center,...? |
| com.apple.developer.team-identifier | **TeamID**  | If Game Center,...? |
| com.apple.security.network.client | **YES** | If accessing things online |


## WHY
When you sign your code you need to add the correct entitlements, meaning you describe what you will access and need or in other words what the app can be expected to do. Codesign will use this information in the signature.  

###### Examples
connect to outside websites, use camera, access public folders like Pictures, … 

[**QUOTE Matthias @ GentlyMad**](https://gentlymad.org/blog/post/deliver-mac-store-unity) (Entitlements file with only com.apple.security.app-sandbox.) This is the most basic .entitlements file with near zero capabilities. It worked for our app, because we didn’t need anything special. It might not work for you! If it doesn't work for you: Unity mentions the Unity Entitlements Tool to easily generate an .entitlements file but the link in the manual is broken. After some search: Here is the download link for the latest version . Luckily I didn't need to use it for our little app, so good luck to you! Make sure you read the guide! Save the file with the .entitlements ending, for convenience it should be located besides your .app package.


[**QUOTE Zwilnik @Strangeflavour**](http://www.strangeflavour.com/creating-mac-app-store-games-unity/) As the game’s going to work on Yosemite and Mavericks and be downloaded from the Mac App Store it *MUST* have the App Sandbox enabled. This protects users from any bugs in your code accessing and breaking things it shouldn’t. However it does mean you have to specifically add any features your app needs.

[**QUOTE Zwilnik**](http://www.strangeflavour.com/creating-mac-app-store-games-unity/)
You’ll need the App Sandbox entitlement (set to YES) and if you’re accessing anything on the internet, you’ll need com.apple.security.network.client set to YES too
To cover Game-Center you have to manually add the following entitlements in your entitlements file (normally Xcode would handle this for you..) com.apple.application-identifier & com.apple.developer.team-identifier

## DIY Entitlements
Create your entitlements file and place it in the same folder as your build. You could place it anywhere you just have to reference the correct folder when you call codesign in the terminal.# Sign, package build & Deliver
## SIGN & PACKAGE
### Get your team name & team id 
###### OPTION 1
From your keychain. Looks like this “TEAM NAME (XXXXXXXXXX)” Copy it exactly as it is, so no spaces before or after. [Example where to find](https://apple.stackexchange.com/a/312503) 

###### OPTION 2 
Like in the previous chapter go to [Membership](https://developer.apple.com/account/#/membership/) and copy your team name and team ID there and put it in this format: “TEAM NAME (XXXXXXXXXX)”

**IMPORTANT** If your team name has spaces in it they also have to be included. So full team name including spaces then again a space after and then in between parentheses your team id. Ok three examples:

1. JOHN (DGHHJF45F4)
2. JOHN CRIED (DGHHJF45F4)
3. JOHN CRIED INC (DGHHJF45F4)

### Run “SignAndPackage”
1. When prompted type your team name
2. When prompted enter : dev, appstore or outside
3. If building for outside Appstore, you will be prompted again to choose between zip or installer.

| INPUT | WILL CREATE PACKAGE FOR | AT |
|:--|:--|:--|
| dev | Appstore development | 7_Distribution/Appstore/Development/VERSION/ |
| appstore| Appstore distribution | 7_Distribution/Appstore/Distribution/VERSION/ |
| outside | Distribution outside Appstore | 7_Distribution/Outside Appstore/CHOICE/VERSION/ |

**IMPORTANT** if signing is successful and you have signed for the first time you will be prompted to add “codesign” to your keychain. **Always allow**

**Troubleshouting** 

1. If you have an error on ambiguous identity, you might have double certificates or old ones. [check here](https://stackoverflow.com/a/32926182)
2. If the identity cannot be found double check your keychain and check your certificates & keys to make sure you have them. Otherwise go back to the first chapter and follow create certificates. 

## WHAT WILL HAPPEN
### Creates a folder structure with version names for your builds
Reads your new Info.plist and finds version & build to create a final folder based on your values so you can keep track of which builds were made for what. If you use the same value for build & version only version will be used, otherwise it will create a folder named "VERSION"+b+"BUILD" (e.g. 1.0b0).

### Existing profiles in your build are deleted
At *"1_MyBuild/YOUR_GAME/Contents/"*

### Adds the correct provisioning profile to your game
Depending on your choice between appstore, dev or outside the  correct provisioningprofile will be copied to your game as “embedded.provisioningprofile“

| FROM | TO | 
|:--|:--|
|0_BeforeYouBuild/ProvisioningProfiles/CHOICE/MY.provisioningprofile |1_MyBuild/YOUR_GAME/Contents/embedded.provisioningprofile |

**NOTE** That when zipping your build this profile will not be included as I am not clear yet if this is required.

### Code signing
[All code needs to be signed for your app to be approved.](https://developer.apple.com/library/archive/documentation/Security/Conceptual/CodeSigningGuide/Procedures/Procedures.html#//apple_ref/doc/uid/TP40005929-CH4-SW5) Sometimes --deep does not work so we manually sign all the code we (know) and loop over all the .bundles in your plugin folder. For us this worked out, but if you know of more generic libraries that should be added please let me know.

| ALL CODE |
|:--|
| appDir/Contents/Frameworks/MonoEmbedRuntime/osx/libmono.0.dylib” |
|appDir/Contents/Frameworks/MonoEmbedRuntime/osx/libmono.0.dylib”|
|appDir/Contents/Frameworks/MonoEmbedRuntime/osx/libMonoPosixHelper.dylib”|
|all .bundle files in /Contents/Plugins/ are signed iteratively|
|Your app will be signed with --deep ( all subfolders)|

### Verify signed bundle
See if everything worked out.

### Depending on your choice a signed package will be made
At *"/7_Distribution/DISTRIBUTION_CHOICE/VERSION."*

## DIY SIGN & PACKAGE 
### Provisioning profile
When creating any installer make a duplicate of the correct provisioning profile, rename it to embedded.provisioningprofile and place it in YOUR_BUILD/Contents. [Read more at Strangeflavour](http://www.strangeflavour.com/creating-mac-app-store-games-unity/)

### CODESIGN TERMNIAL

###### Appstore
	codesign --entitlements "DIR_Entitlements" --deep --force --verify --sign "3rd Party Mac Developer Application: TEAM NAME (TEAM_ID)" "appDir"

###### Test / developer build

	codesign --deep --force --verify --sign "3rd Party Mac Developer Application: TEAM NAME (TEAM_ID)" "appDir"

###### Outside Appstore
	codesign --entitlements "DIR_Entitlements" --deep --force --verify --sign "Developer ID Application: TEAM NAME (TEAM_ID)" "appDir"

| OPTION | INFO |
|:--|:--|
|--deep | Go through all subfolders to sign everything |
|--force | Resign everything that has been signed |
|--verify | See if things got signed |
|--entitlements | Location of your entitlements |
|--sign | The type of signing with your team name|

### BUILD PACKAGE TERMINAL 

###### Appstore

	productbuild --component “$appDir” “/Applications” --sign "3rd Party Mac Developer Installer: TEAMNAME (TEAM_ID)" "$appName.pkg

###### Test / developer build
	productbuild --component "$appDir" "/Applications" "$appName.pkg"

###### Outside Appstore
	productbuild --component “$appDir” “/Applications” --sign "Developer ID Installer: TEAMNAME (TEAM_ID)" "$appName.pkg

# DISTRIBUTION

## APPSTORE DEVELOPMENT / TEST BUILD 

#### IMPORTANT
When Installing the installer will default to the directory of your build. e.g. /1_MyBuild/App and not the application folder. So either test the application from here or if you want your testing build in the applications folder, delete your build before installing your final pkg.   

[**QUOTE "Zwilnik @ Strange flavour"**](http://www.strangeflavour.com/creating-mac-app-store-games-unity/)
When you launch the game, you should see a dialog pop up that tells you that the game was purchased by a different account, so you need to sign in with one of your Mac App Store Sandbox test IDs here for the game to launch. Don’t use your normal login, it must be a Sandbox ID 

[Create testusers here at iTunes Connect](https://appstoreconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/users_roles/sandbox_users)

## APPSTORE DEVELOPER ID - INDEPENDENT DISTRIBUTION 
### Open your app and see if gatekeeper complains
If you cannot open your build it's possible you forgot to uncheck "Mac Appstore validation" in the player settings when you made your Unity build.

### IF INSTALLER PKG
[QUOTE Mark-ffrench](https://forum.unity.com/threads/how-to-open-mac-build-file-after-code-sign.454435/#post-2954548) Installing the pkg file should, in theory, install the app in the /Applications folder. However, there are a couple of other possible issues that you might encounter:

If OSX already thinks you have a copy of your app anywhere on your mac, it will install your new version over it. This could be anywhere on your hard drive, so make sure that the app you are trying to run after installation is the one that has actually been updated by the installer. Your best bet is to try track down all copies of your app and delete them.

## APPSTORE DISTRIBUTION
### Go to Itunes connect and open your app again
Fill in the details for your App such as description, keywords,... Make sure here you **DO NOT reference other platforms**. 

Now add Screenshots (Min 1 - Max 10) as they are required for your upload [(Screenshot specifications)](https://help.apple.com/app-store-connect/#/devd274dd925). If you have it also add your app preview video. [guidelines](https://developer.apple.com/app-store/app-previews/)

[**Quote Victor Leung**](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)
After filling in the form, you will also need to spend some time to take screenshots, and crop them in the right size. Only the following sizes are allowed:

- 1280 x 800 pixels
- 1440 x 900 pixels
- 2560 x 1600 pixels
- 2880 x 1800 pixels

**IMPORTANT** Once you upload your build for review with your screenshots, those screenshots are stuck. Meaning you need to upload a new build to replace the previous screenshots.

### Use application loader to send your app to Apple.
In Xcode Top Menu "Xcode" > Open Developer Tools > Application loader. 

[Dilmer Valecillos told of a problem](https://www.dilmergames.com/blog/2017/03/29/unity3d-how-deliver-application-apple-mac-store/) with newer versions of Application Loader, but we didn't have this problem. The opposite actually, we weren't able to open application loader 3 getting this error: *"To use this application, you must first sign in to Itunes Connect and sign the relevant contracts."* Which we couldn't fix, but we could just do it with Xcode's default Application loader. [Though if you need it: Download link application Loader 3.0](https://itunesconnect.apple.com/apploader/ApplicationLoader_3.0.dmg)

### Check if your build got through. 
Go to [Itunes Connect](https://appstoreconnect.apple.com) > Your App > Activity and check if your build is there. After uploading a build it can take **15 minutes to 1 hour** (or longer) to finish processing in Appstore connect.

### Add your build for review
Now open the Prepare for submission (in the Appstore tab of Your game/app). Select your build, fill in the contact info and click on Send for review.

### And Now answer questions three...
Is your app designed to use cryptography or does it contain or incorporate cryptography? (Select Yes even if your app is only utilizing the encryption available in iOS or macOS.) 

* [EAR (Encryption-and-Export-Administration-Regulations)](https://www.bis.doc.gov/index.php/encryption-and-export-administration-regulations-ear)
* [Offial FAQ US government look @ 15. What is Note 4?](https://www.bis.doc.gov/index.php/policy-guidance/encryption/encryption-faqs/15-policy-guidance/encryption/560-encryption-faqs#15)
* [Answer by Unity for Unity services](https://forum.unity.com/threads/us-export-compliance-encryption.389208/#post-2893835) 
* [Submitting Unity iOS App to iTunes: "Does Product Use Encryption?"](http://answers.unity.com/answers/669794/view.html)
* [Best Short answer https encryption](https://stackoverflow.com/a/46691541) 	
* [Best Long Anwser](https://www.cocoanetics.com/2017/02/itunes-connect-encryption-info/)
* [Long Answer on HTTPS](https://stackoverflow.com/a/16080233)

Does your app contain, display, or access third-party content? 
 
 - [**NOTE** You own the rights for assets purchased through the Asset Store  ](http://answers.unity.com/answers/986554/view.html)

### Wait for response from apple
The review process can go from a few hours up to 7 days depending on the complexity of your game and some other factors like Christmas. 

* The review team works weekends
* Updates will always go faster. 
* In our experience it was only a couple of hours. 
* [Read more on review time](https://www.quora.com/How-long-is-an-app-in-review-on-iTunes-Connect-Does-it-depend-on-robustness/answer/Michael-Schranz-1)

### Rejected?
If you upload a new package to the Appstore after a rejection, you will not be able to reply on the previous answer of Apple. So if there are things you need to tell/ask the Review people do it when your App gets rejected in Appstore Connect.

[**QUOTE "Victor Leung"**](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)
If you got an email with issues,
Invalid Signature — The main app bundle game at path GAMENAME.app has following signing error(s): invalid Info.plist (plist or signature have been modified) In architecture: i386 .
It is probably one of your subcomponent didn’t sign correctly, which you may need to spend sometime on Apple documentations to understand how it works.

[READ UP ON : Most common reasons for rejection](https://rollout.io/blog/how-long-does-the-apple-review-process-take/)

### You need to upload a new build?
Don't worry you don't need to go through everything again. You just run RepeatForUpdatedBuild and it will go through all the steps with the data you used. But don't forget to increase the version or build number in Info.plist for a new build.

### Cry yourself to sleep

