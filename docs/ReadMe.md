# UNITY APPLE DISTRIBUTION WORKFLOW
Workflow to automate and guide people in delivering Unity builds for macOS.

## DISCLAIMER 

We delivered builds to the Appstore using this script but beyond that it is possible that some things are not correct (e.g. iCloud, Building for outside the Appstore, ... ) Things also change often so if you notice a mistake don't hesitate to tell in the thread below.  

[UNITY THREAD](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/) 

## BUGS  

### Unity 2018 Results in Apple rejection  
There seems to be a bug with Unity 2018 which will have your bundle rejected because of gamekit. So don't upgrade until this is fixed ([fixed in 2018.3 and up](https://issuetracker.unity3d.com/issues/apple-platforms-gamekit-reference-in-the-application-when-game-room-is-not-used-app-store-rejects-the-build?page=2#comments)). [Link to workaround by giorgos_gs](https://forum.unity.com/threads/app-links-against-the-gamekit-framework-reject-by-apple-reviewer.542306/#post-3577490)

Fix from Giorgos

1. Install [MachOView](https://sourceforge.net/projects/machoview/)
2. Open the binary in Contents/MacOS
3. Expand the "Load Commands" section
4. Look for LC_LOAD_DYLIB (GameKit)
	- Notice the command size is 88, we need to find a same length framework to replace it, we'll use Webkit
	- Change Command Data to 0000000C (with our build data already had this value)
	- Change Current Version to **025C0407** (check the value at Webkit) 
	- Change Name to **2F53797374656D2F4C6962726172792F4672616D65776F726B732F5765624B69742E6672616D65776F726B2F56657273696F6E732F412F5765624B697400**
5. File -> Save
6. Proceed to signing

### Unity Purchasing and closing a build
There is a bug with OSX/MacOS and Unitypurchasing. When quitting your build either takes up to 10 seconds to close or outright crashes. We are not 100% sure if it has something to do with our setup or not, so just see if your build closes properly, if it does ignore this. Below is currently the only fix I have found taken from this [Issue](https://issuetracker.unity3d.com/issues/osx-enabling-unitypurchasing-on-mac-standalone-causes-builds-to-hang-when-quitting-them). Place this on an object that will live throughout your game/app. 

```
void OnApplicationQuit() 
{
  if (!Application.isEditor) { System.Diagnostics.Process.GetCurrentProcess().Kill(); } 
}
```

We decided to leave IAP behind though and disable Purchasing altogether. If you are developing for more Platforms and like us want to use Purchasing in them, just wrap the code for initialising (in unitypurchasing) in [platform dependant directives](https://docs.unity3d.com/Manual/PlatformDependentCompilation.html).

### Unity 2018 & Mavericks 
 If you are supporting users on Mavericks make sure you have added OpenGlCore Graphics Library otherwise if you only use Metal you can set the Minimum system version to 10.10.0 in your Info.plist (see chapter Info.plist)

[macOS Version History](https://en.wikipedia.org/wiki/MacOS_version_history)

## INSTRUCTIONS
### You're new at this?
For a first build just follow all the steps. In each folder you will find instructions to deal with the problem at hand in chronology. After you finished the steps a first time you can just run "RepeatForUpdatedBuild" to quickly repeat the whole process. But you **need** to finish the steps so all the data used is correct otherwise problems will arise. 

### You've been here for a while?
If you already have all the necessaries you can place them in their respective directories, following the table below and run "RepeatForUpdatedBuild"

| WHAT | DIR |
|:--|:--|
| DEV.provisionprofile | 0_BeforeYouBuild/ProvisioningProfiles/Development/  |
| APPSTORE.provisionprofile | 0_BeforeYouBuild/ProvisioningProfiles/Appstore/ |
| DEV_ID.provisionprofile | 0_BeforeYouBuild/ProvisioningProfiles/Developer/  |
| YOURGAME | 1_MyBuild/ |
|PlayerIcon.icns|2_Icons/MyOsxIcon/|
|UnityPlayerIcon.png|2_Icons/MyUnityPlayerIcon/|
| Info.plist | 4_InfoPlist/ |
| entitlements.plist | 5_Entitlements/Distribution/ |
| **OPTIONAL** (entitlements.plist) | 5_Entitlements/Development/ |

## Scripts
All scripts (and todos) are described in more detail in each folder. As everything is automated there are also failsafes in place for overwriting or to check if needed files are present. We will not describe all of those, but you can view the script to see what is happening.

### OSX Icon 
Creates PlayerIcon.icns & UnityPlayerIcon.png from single png file and placed them in BUILD/Contents/resources/

### Delete Meta Files
Script to Delete meta files in *"BUILD/Contents/Plugins/plugin_x"*

### ImportPlist
Info.plist is copied from *"BUILD/Contents/"* to *"4_InfoPlist/"* so you can update it manually once and reuse it after.

### CopyPlist
Copies your manually updated Info.plist from *"4_InfoPlist/"* to *"BUILD/Contents/"*

### PluginsReplaceBundleId
Takes the adjusted Info.plist and finds your BundleIdentifier. Opens all bundles in the plugins folder and replaces the BundleIdentifier value with yours.

### Entitlements templates
Self explanatory.

### SignAndPackage
- Create a version folder structure for your builds in 7_Distribution/
- Copies the correct provisioning profile as embedded.provisionprofile to *"BUILD/Contents/"* depending on Appstore, Development or Distribution choice.
- Signs all code with your entitlements. Loops over bundles in plugins, finds .dylib files in Frameworks, signs app.
- Verify
- Builds signed package for either Appstore, Development or Distribution outside Appstore (installer or zip)

### RepeatForUpdatedBuild
Script that calls all the other scripts to speed up preparing updates and creating new builds. 

When you need to run the script over and over again, or you want to automate this process for future builds, call `./PluginsReplaceBundleId -h` to see automation options.
Here is an example of a fully automated process:

**ATTENTION: This is no shortcut to set up the steps by hand!<br/>
DO NOT RUN IF YOU HAVE NOT FINISHED THE STEPS**

	./RepeatForUpdatedBuild -q -t appstore -i 'TEAM NAME (XXXXXXXXXX)' -s deep

The only thing that needs to be done manually is placing the build in the right directory, and uploading the final package via Transporter.

## PM or ask at [UNITY THREAD](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/) or to contribute
As we will not be constantly uploading games to the App Store it might be good to have other people pitching in so that there's a central point to get help that doesn't age. So anyone who wants to become a contributor just pm me on git even if it is to just add some documentation.

## CREDIT
We do not take credit for anything besides creating this workflow, tying the code we found together in bash with added automation and organising & rehashing documentation. Below are the credits for all the posts, threads we used. If we missed a spot please contact us. 

We created this workflow because delivering a Unity build to Apple is a mess and the answer is spread across several websites which makes it hard to wrap your head around the correct final chronology especially if you're new or on Windows.

There are tools out there like [Signed](https://assetstore.unity.com/packages/tools/utilities/mac-app-store-signed-54970) that will also take care of signing, but often it's better to understand and not depend upon 3rd party assets, especially when things go wrong. We have tried to include paths, actions and DIY as much as possible so you can understand what's happening and where everything is supposed to go.

### ALL THE BRAVE SOULS WHO PAVED THE ROAD.

| WHO | WHAT | HOME |
|:--|:--|:--|
| Joel | [GUIDE ICLOUD DEV](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html) | [Kittehface.com](http://www.kittehface.com) |
 N3uRo | [UNITY THREAD](https://forum.unity.com/threads/the-nightmare-of-submitting-to-app-store-steps-included-dec-2016.444107/) | [Unity Assetstore Page](https://assetstore.unity.com/publishers/18584) |
| Zwilnik | [GUIDE](http://www.strangeflavour.com/creating-mac-app-store-games-unity/) | [Strangeflavour.com](http://www.strangeflavour.com) |
| Victor Leung | [MEDIUM GUIDE](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412) | .. |
| Matthias | [GUIDE](https://gentlymad.org/blog/post/deliver-mac-store-unity) | [Gentlymad.org](https://gentlymad.org) |
| Dilmer Valecillos | [GUIDE](https://www.dilmergames.com/blog/2017/03/29/unity3d-how-deliver-application-apple-mac-store/) |[www.dilmergames.com](https://www.dilmergames.com)|
|BrainAndDrain|[UNITY POST/GUIDE](https://forum.unity.com/threads/where-is-up-to-date-info-on-making-mac-app-store-build.423330/#post-2829466)| [Brainandbrain.co](http://brainandbrain.co/about) |
| Alamboley | [CODESIGNING GIT](https://github.com/DaVikingCode/FromUnityAppToMacAppStore) | [Davikingcode.com](http://davikingcode.com/) |
| JoeStrout | [POST BUILD,SIGN, ZIP](https://forum.unity.com/threads/osx-code-signing.455830/) | [Stroutandsons.com](http://stroutandsons.com) |
| Henry | [ICON GEN](https://stackoverflow.com/a/20703594) |..|
| TranslucentCloud | [PIXEL GRID](https://stackoverflow.com/a/39678276) |..|
| Jeremy Statz | [CONTROLLER SUPPORT](http://www.kittehface.com/2019/08/controller-usage-in-signed-macos-game.html) | [Kittehface.com](http://www.kittehface.com) |

### WORKFLOW
Worked on this workflow? Add yourself! Btw you can use the *"doc/CombineAllReadmeIntoDoc*" to recompile all readme's for the online manual page.

| WHAT | WHO | HOME |
|:--|:--|:--|
| ORGINAL WORKFLOW | UNSH | [UNSH.IO](https://unsh.io) |
|[Post](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/#post-3604213) | Atorisa | [Assets](https://assetstore.unity.com/publishers/17426) | 
Corrections guide iCloud  | omgitsraven |[Home](https://github.com/omgitsraven|http://andrew.fraticelli.info/)|
| Build automation | v01pe | [brokenrul.es](http://brokenrul.es)

 


# BEFORE YOU BUILD
Just place this workflow somewhere outside of your Unity project.

## INSTRUCTIONS (APPLE)
### Download & Install Xcode from the App Store if you haven't already.
[Xcode is actually required](https://forum.unity.com/threads/failed-to-create-il2cpp-build-on-osx.530824/) ( and has to be installed at /Applications ) for a IL2CPP build. If you have more versions of Xcode and run into problems [Read This by Hogwash](https://forum.unity.com/threads/failed-to-create-il2cpp-build-on-osx.530824/#post-3508248)

You can also use it to open your .plist files later. Or alternatively to create your icon without the scripts.

### Create Apple Developer Account 
If you do not already have one go to [Apple Developer portal](https://developer.apple.com/) and pay 100$ to get confused by professionals. And you might as well enable **AUTO RENEW** in the membership tab at [Apple Developer portal](https://developer.apple.com/). So you avoid surprises. 

**SELLING ON APPSTORE?** If you are not releasing a free app/game and want to get paid outside of the US. Go fix the banking and tax first at [Appstore Connect](https://appstoreconnect.apple.com) >> Agreements, Tax and banking. It's not necessarily something that has to happen now, but get it over with if you are about to release. Prepare for some serious legal and tax lingo.

### Create the Certificates you need
Go to the [Apple Developer Portal](https://developer.apple.com/account/mac/certificate/development) and create your certificates. Depending on where you want to release your game you will need different certs. These serve as an identity that will be added to your iCloud keychain and allow you to codesign and create your provisioning profiles later. [More on certificate names here](https://stackoverflow.com/a/13603031) [And here](https://stackoverflow.com/questions/29039462/which-certificate-should-i-use-to-sign-my-mac-os-x-application/49015213) [What happens when you use the wrong here](https://stackoverflow.com/questions/21295255/productsigned-mac-app-not-installing-in-computers-that-are-not-mine)

##### INSTALLING CERTIFICATES
In the creation process the field "common name" will name your keychain. Make sure you name your certificates in a way you can recognise them later. Its not a disaster if you don't but it does make your keychain a bit more clear if problems arise later. So for example "TEAM_Mac Installer Distribution". 

**IMPORTANT** Always make sure you use the correct certificates and provisioning profiles. If you have used old certificates (and provisioning profiles) you will need to download these again from the member center. **CREDIT** [Atorisa](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/#post-3604213)

When your certificates are installed on the keychain test to double check with the terminal and the command below. Problems? [read this guide for troubleshooting.] (https://medium.com/@ceyhunkeklik/how-to-fix-ios-application-code-signing-error-4818bd331327) [or this] (https://apple.stackexchange.com/questions/196238/identity-not-found-when-trying-to-code-sign-an-application-with-a-certificate) Note on our build the Mac Installer Distribution did not show up but worked.

  security find-identity -vp codesigning


#### DEVELOPMENT
| CERTIFICATE NAME | DESC |
|:--|:--|
| Mac Development | Sign development versions of your Mac app |

#### APPSTORE DISTRIBUTION
| CERTIFICATE | DESC |
|:--|:--|
| Mac App Distribution | This certificate is used to code sign your app and configure a Distribution Provisioning Profile for submission to the Mac App Store. |
| Mac Installer Distribution | This certificate is used to sign your app's Installer Package for submission to the Mac App Store. |

#### NON-APPSTORE DISTRIBUTION
| CERTIFICATE | DESC |
|:--|:--|
| Developer ID Application | This certificate is used to code sign your app for distribution outside of the Mac App Store. |
| Developer ID Installer | This certificate is used to sign your app's Installer Package for distribution outside of the Mac App Store. |

### Create an App Identifier 
[Apple Developer Portal Go to App Identifiers](https://developer.apple.com/account/mac/identifier/bundle/). Examples : COM.COMPANY.PRODUCT, UNITY.COMPANY.PRODUCT, ... It's up to you. Note that you cannot disable In App Purchase. IAP is always enabled, but if you don't implement any button or script in Unity you can just ignore this. [More info on bundle identifiers here](https://cocoacasts.com/what-are-app-ids-and-bundle-identifiers/)

#### Capabilities 
Enable any services such as (iCloud, Game Center, ...) you are going to use and configure them if necessary. It's important you do this before you download your Provisioning Profile so that this information can be included in your profiles.

**iCloud** To create a single app that shares data through iCloud you always need to create separate App Identifiers for iOS/tvOS and macOS. And seeing that Unity builds with iOS can use Xcode it's easier to configure them on iOS.

[QUOTE Joel @kittehface](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html) *With iOS/tvOS being the flagship platforms, it probably makes more sense to create your primary App ID for them and have a secondary one for macOS.  But the secondary one will need to have a different name. Both App IDs will need to have the iCloud/CloudKit capability set.  Again, you can do all the primary work on the iOS/tvOS App ID.  If you're also doing your Unity game on those platforms, make a build there, have it generate the Xcode project, and use Xcode to turn on the iCloud capability and set up CloudKit.  It'll handle creating your default container (which will be named for the iOS/tvOS App ID).*

When enabling iCloud support in an Identifier on the Apple developer website, make sure to also click the "edit" button to the right, and put a checkmark next to the iCloud container(s) your game will be using (if they aren't checked already).


### Create a new app in App Store Connect
Go to [App Store Connect](https://appstoreconnect.apple.com) and create a new (macOS) app.

**[QUOTE Victor Leung](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)** *Login to App Store Connect, choose My Apps > “+” > “New Mac App”, fill in the values and choose the bundle ID matches with the previous step. The prefix field would be the game name, such us ufo in my case.* 

### Create your provisioning profiles

The type of provisioning profile depends on your selling platform (in -or Outside Appstore). To test Appstore features like Game Center you will need to create a separate provisioning profile for Appstore development as the distribution build will not allow testing.

| FOR | NAME PROVISIONING PROFILE |
|:--|:--|
| Testing | Mac App Development |
| Distribution Appstore | Mac App Store |
| Distribution Outside Appstore | Developer ID |

Go to the [Apple developer portal](https://developer.apple.com/account/mac/profile/). Just follow the instructions, if you have more problems follow [this tutorial](https://help.apple.com/developer-account/#/devf2eb157f8). Also **give a clear name to your profiles** when you download them so you do not make mistakes later. 

#### REGISTER YOUR MAC DEVICES 
If this is the first time you will be asked to add a device and give the UUID of this device you can find this here: About this Mac > System Report > Hardware overview (Hardware UUID: XXXXXXXXX) Also you will need to include all test devices in a development profile so if you need to add more devices do this now.

**IMPORTANT** Make sure you have set up all services such as iCloud before downloading your profiles.

"[QUOTE Joel @ kittehface](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html)"
*Since we're doing a development build, create your provisioning profile with your development certificate, then make sure to set your Mac as one of the allowed devices before generating and downloading it.  Also make sure you've taken care of all the CloudKit setup before you generate the provisioning profile.*


### Place Provisioning Profiles in their respective folders
These will be copied later into your app as embedded.provisionprofile ( dependant on the build you chose).

| PROFILE | FOLDER |
|:--|:--|
| Development | 0_BeforeYouBuild/ProvisioningProfiles/Development/RESPECTIVE.provisionprofile |
| Appstore| 0_BeforeYouBuild/ProvisioningProfiles/Appstore/RESPECTIVE.provisionprofile |
|Outside Appstore| 0_BeforeYouBuild/ProvisioningProfiles/Developer/RESPECTIVE.provisionprofile |

**QUOTE** "Zwilnik @ [Strange flavour](https://www.dilmergames.com/blog/2017/03/29/unity3d-how-deliver-application-apple-mac-store/)"
*Another key step is to include a copy of the provisioning profile in the app bundle before signing it. It goes in the app bundle at Contents/embedded.provisionprofile.  Again, this is something Xcode would do for you normally that you have to do manually when building with Unity.  Do this for both development and distribution builds including the correct development or distribution profile.*

## INSTRUCTIONS UNITY

**IMPORTANT** There seems to be a bug with Unity 2018 which will have your bundle rejected because of gamekit. So don't upgrade until this is fixed (currently 2018.2). [Link to workaround by giorgos_gs](https://forum.unity.com/threads/app-links-against-the-gamekit-framework-reject-by-apple-reviewer.542306/#post-3577490) 

### Add OsxResolutionFix.cs to your build
Add to any GO that will live @ startup to fix retina on large screens.

**CREDITS** 
[https://gentlymad.org/blog/post/deliver-mac-store-unity](https://gentlymad.org/blog/post/deliver-mac-store-unity)

[Original Unity Doc](https://docs.unity3d.com/Manual/HOWTO-PortToAppleMacStore.html)

### Double check in Project Settings > Player Settings 
If you want to build for outside the Appstore, make sure you uncheck Mac Appstore validation in your build.

| SETTING | STATE |
|:--|:--|
|Mac Appstore validation | true (if building for Appstore) |
|Default is Full Screen | true |
|Default is Native Resolution | true |
|Capture Single Screen | false |
|Version | Read below |
|Build | Read below |

[**CREDITS / READ MORE Victor Leung**](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)

### Version
| KEY | VALUE |
|:--|:--|
|**Version** | Build or version number should always be increased each time you upload to the Appstore as you cannot upload a package with identical version & build [More on Version & build](https://stackoverflow.com/a/19728342)|
|**Build**| Used to upload packages to the Appstore that you do not want to show in your public version. Can be the same as ShortVersionString, but if you want to change your screenshots at the Appstore you will need to send them a new package, with build you could hide this new version without increasing your version number.|

Don't use 0 in your decimals when you define your version.  e.g. to 1.1XX. If you use 1.01 Apple will set your version to 1.1 on App Store Connect, it will show 1.01 on the Appstore page, but you will not be able to upload a 1.1 version to App Store Connect anymore forcing you to jump from 1.01 to 1.11. So either 1.0.1 or 1.11

### Make sure:
- If you are building for more platforms not to include links or references to them.
- If for example you sell on iOs with IAP and on OSX you have a fixed price, then remember to disable the IAP buttons.
- Make sure all buttons are big enough and everything is accessible. ([Apple guidelines](https://developer.apple.com/design/human-interface-guidelines/))
- Double check if all your buttons work.
- [Read this](https://developer.apple.com/app-store/review/rejections/)

### Using Unity IAP & disable menu 
Make sure you do not immediately close the Menu containing the IAP button with onPurchaseComplete. If you do use Invoke to delay the closing for 0.1 seconds otherwise you will have problems. It will seem to work on some devices but with others your purchase will not come through.

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
7. For automation features call `./OSX\ Icon -h`

#### Problem 
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

Currently the only 3 values of interest are: 

| KEY | VALUE | DESC|
|:--|:--|:--|
|App Uses Non-Exempt Encryption| **TRUE/FALSE** | "App is using encryption that is exempt from [EAR](https://www.bis.doc.gov/index.php/encryption-and-export-administration-regulations-ear)" **Before you scare read**  [Short answer](https://stackoverflow.com/a/46691541), [this for Unity](http://answers.unity.com/answers/669794/view.html), and [this by Unity](https://forum.unity.com/threads/us-export-compliance-encryption.389208/#post-2893835) |
| LSMinimumSystemVersion | 10.XX.X | Limit OS [Version history](https://en.wikipedia.org/wiki/MacOS_version_history) |
|GetInfoString | Copyright COMPANY 2022 | **The copyrights** to your game instead of Unity's |

### RUN "ImportPlistFromYourBuild"
Will take the Info.plist from 1_MyBuild/YOUR.APP/Contents/Info.plist and paste it here. If there is already one in this folder it will make a backup copy just to be sure.

| FROM | TO |
|:--|:--|
| 1_MyBuild/YOUR.APP/Contents/Info.plist | 4_InfoPlist/Info.plist |

### EDIT "4_InfoPlist/Info.plist"
Just double click and open with Xcode so you have access to the dropdown values to avoid typo's. Change the values where needed using the first table.

### RUN “CopyPlist” 
Will replace the Info.plist in your build with the one you just made.

| FROM | TO |
|:--|:--|
| 4_InfoPlist/Info.plist | 1_MyBuild/YOUR.APP/Contents/Info.plist |

## WHY
Every app and plug-in uses an Info.plist file to store configuration data in a place where the system can easily access it. macOS and iOS use Info.plist files to determine what icon to display for a bundle, what document types an app supports, and many other behaviors that have an impact outside the bundle itself.
[More info on Info.plist @ Apple](https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Introduction/Introduction.html)

1. Unity does not update all fields
2. Every time you build in Unity the file is overwitten so you need to keep a copy here that you can replace every build.
	- [**QUOTE “Matthias @ Gently Mad“**](https://gentlymad.org/blog/post/deliver-mac-store-unity)
Save the Info.plist inside your package and don’t forget to create a copy since Unity will overwrite the file the next time you make a build!

## PluginsReplaceBundleId
### What it does
Takes the Info.plist you just made and finds your BundleIdentifier. Opens all bundles in the plugins folder and replaces the BundleIdentifier value with yours.

For automation features call `./PluginsReplaceBundleId -h`

### Why
Even the Unity services bundles need your BundleIdentifier, otherwise you will get errors when uploading to the Appstore.

[QUOTE N3uRo](https://forum.unity.com/threads/the-nightmare-of-submitting-to-app-store-steps-included-dec-2016.444107/) Edit other "*.bundle"s Info.plist that has a "CFBundleIdentifier" to point to your identifier also. I had a problem with AVPro that had it's own identifier that was not valid.

## DIY Info.plist
1. Adjust the above values in your YOUR_BUILD/Contents/Info.plist
2. (MAYBE) So open YOUR_BUILD/Contents/Plugins/each_bundle/Info.Plist And replace the BundleIdentifier with yours.

---------
# DEPRICATED
## VALUES THAT NEEDED TO BE AJUSTED

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
|CFBundleSupportedPlatforms|**MacOSX** Without this the application loader seems to default to iOS. [Read More by N3uRo](https://forum.unity.com/threads/the-nightmare-of-submitting-to-app-store-steps-included-dec-2016.444107/) |
| LSMinimumSystemVersion | Set to 10.10.0 if you are on Unity 2018 [Version history](https://en.wikipedia.org/wiki/MacOS_version_history) |


# Entitlements
## WHY
 [App sandboxing](https://developer.apple.com/library/archive/documentation/Security/Conceptual/AppSandboxDesignGuide/AppSandboxInDepth/AppSandboxInDepth.html) is a security measure that Apple requires from all Appstore apps (which is why you always need this in your [entitlements](https://developer.apple.com/documentation/security/app_sandbox_entitlements)) Sandboxing restricts the privileges of an app so it cannot just open any directory on disk. 
 
 When you sign your app you add privileges through the entitlements file that add these to a signature that describes what you will access and need (e.g read/write in specific folders, access to Pictures, online connections, camera use,...) Codesigning will use your provisioning profile together with these entitlements to create a signature. 
 
 A consequence of this is that ***the information on your provisioning profile has to match to your entitlements***. Your provisioning profile is static in the sense that you download it from your Apple account. You create an app, an identity that describes your intent **AND** enable and set up the capabilities you want to use, after which you download this into a file that contains all this general information of your app. Then in your entitlements you describe again exactly what you access with added detail. So if you use iCloud: what the iCloud Drive containers are called, that you will read/write files, that you need online access, etc... 
 
 Imagine you pass a checkpoint terminal on the airport. Your provisioning profile is your passport, your entitlements file is what you tell the customs officer. At the very least what's on your passport has to match what you tell the officer or you get rejected.
   
## WHAT YOU NEED TO DO
### CREATE ENTITLEMENTS FILE
Create an entitlements filed named "My.entitlements" and place it in either Development or Distribution depending on your needs. ***Again open this My.entitlements with XCode not in text editor to avoid typos.*** 

| DIR | 
|:--|
|/5_Entitlements/Distribution/MyDist.entitlements|
|/5_Entitlements/Development/MyDev.entitlements|

### WHAT DO I PUT IN

Change it and basically describe what your app will need. At the very least it always needs *com.apple.security.app-sandbox set to yes*

* [Find more info on all keys here.](https://developer.apple.com/library/archive/documentation/Miscellaneous/Reference/EntitlementKeyReference/Chapters/EnablingAppSandbox.html#//apple_ref/doc/uid/TP40011195-CH4-SW5)
* There are other examples in "5_Entitlements/Examples" folder to use as a reference.
* You can find **com.apple.application-identifier** & **com.apple.developer.team-identifier** in your provisioining profile. 
* Alternatively TeamID can also be found in [Membership tab of Apple Developer Portal](https://developer.apple.com/account/#/membership/)

| KEY | VALUE  | WHEN NEEDED |
|:--|:--|:--|
|com.apple.security.app-sandbox| **YES**| Always - Delete for STEAM |
| com.apple.application-identifier |**TeamID.COM.COMPANY.GAME**  | Always |
| com.apple.developer.team-identifier | **TeamID**  | Always |
| com.apple.security.network.client | **YES** | If accessing things online |
| com.apple.developer.aps-environment | **development** | Development build *WITH* Services |
| com.apple.developer.icloud-container-identifiers | **CloudKit** | iCloud |
| com.apple.developer.icloud-services | **your container identifiers** (the Enabled iCloud Containers in the Capabilities of the Identifier used in your Provisioning Profile, likely in the form  **iCloud.COM.COMPANY.GAME**) | iCloud |
| com.apple.security.device.bluetooth | **YES** | When using controllers |
| com.apple.security.device.usb | **YES** | When using controllers |
| com.apple.security.cs.allow-unsigned-executable-memory | **YES** | IAP & MONO |
| com.apple.security.cs.allow-dyld-environment-variables | **YES** | STEAM |
| com.apple.security.cs.disable-library-validation | **YES** | STEAM |

## DEVELOPMENT BUILDS
There is a separate folder with entitlements for the development build. By default it's an entitlements file with only com.apple.security.app-sandbox	set to YES. 

#### QUOTE [**"Hidden Jason"**](https://forum.unity.com/threads/notarizing-osx-builds.588904/#post-5071199)
A note on entitlements when notarizing for those who might be running into problems, especially when dealing with Steam:

Mono (which you have to use if you're also using UnityIAP right now) requires: com.apple.security.cs.allow-unsigned-executable-memory

Steam's API requires:
*do not* inlclude com.apple.security.app-sandbox
include com.apple.security.cs.allow-dyld-environment-variables
include com.apple.security.cs.disable-library-validation
Without that set of entitlements, SteamAPI_Init will fail (return false) when running a notarized+stapled app bundle on macOS 10.14.

### iCloud Development
In the examples you can find other entitlements and an iCloud example to use it  just move the file from examples to Development and rename it to "entitlement.plist" Be sure to read the Readme of SignAndPackage because you will need to preform another step with optool.

Depending on what you need you can bump into crashes when your app tries to access a specific service of function that requires entitlements. The crash report will tell you that an entitlement is needed. will need to adjust your [entitlements](https://developer.apple.com/library/archive/documentation/Miscellaneous/Reference/EntitlementKeyReference/Chapters/EnablingAppSandbox.html#//apple_ref/doc/uid/TP40011195-CH4-SW5) accordingly.   

#### CREDIT FOR THIS UPDATE [**"Joel @ Kitteh Face"**](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html)

(However, in some cases, improper iCloud entitlements can instead crash on launch with a `Namespace CODESIGNING` error; so if you're seeing this error, double-check your entitlements and the capabilities of the Identifier used in your Provisioning Profile.)

## MORE


[**QUOTE Matthias @ GentlyMad**](https://gentlymad.org/blog/post/deliver-mac-store-unity) (Entitlements file with only com.apple.security.app-sandbox.) This is the most basic .entitlements file with near zero capabilities. It worked for our app, because we didn’t need anything special. It might not work for you! If it doesn't work for you: Unity mentions the Unity Entitlements Tool to easily generate an .entitlements file but the link in the manual is broken. After some search: Here is the download link for the latest version . Luckily I didn't need to use it for our little app, so good luck to you! Make sure you read the guide! Save the file with the .entitlements ending, for convenience it should be located besides your .app package.


[**QUOTE Zwilnik @Strangeflavour**](http://www.strangeflavour.com/creating-mac-app-store-games-unity/) As the game’s going to work on Yosemite and Mavericks and be downloaded from the Mac App Store it *MUST* have the App Sandbox enabled. This protects users from any bugs in your code accessing and breaking things it shouldn’t. However it does mean you have to specifically add any features your app needs.

[**QUOTE Zwilnik**](http://www.strangeflavour.com/creating-mac-app-store-games-unity/)
You’ll need the App Sandbox entitlement (set to YES) and if you’re accessing anything on the internet, you’ll need com.apple.security.network.client set to YES too
To cover Game-Center you have to manually add the following entitlements in your entitlements file (normally Xcode would handle this for you..) com.apple.application-identifier & com.apple.developer.team-identifier

[**QUOTE Jeremy @ Kitteh Face**](http://www.kittehface.com/2019/08/controller-usage-in-signed-macos-game.html)
\[…\] once your app is signed it's trapped in a sandbox environment, and the only light filters through gaps provided by those entitlements.
In the case of a gamepad, we're talking either USB (wired) or Bluetooth (wireless).

## DIY Entitlements
Create your entitlements file and place it in the same folder as your build. You could place it anywhere you just have to reference the correct folder when you call codesign in the terminal.
# Sign, package build & Deliver
### WHY SIGN LAST
Making any adjustment to your build after you signing it is like opening a sealed letter. So everytime you changed something you need to reapply the seal.

## BUILD WITH ICLOUD? (&Prime31?)
First before signing you have to edit the Unity binary following this step from Kittehface (If you already signed then sign again after this step) You have to sign last because this step will modify your build which will subsequently invalidate your signing.

[TAKEN FROM Joel @Kittehface.com :](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html) 

Modify the Unity executable to link the CloudKit framework. Following from the eppz! blog
	 
1. You need to use the third party tool [optool](https://github.com/alexzielenski/optool).
2. Run the command **optool install -c load -p "/System/Library/Frameworks/CloudKit.framework/Versions/A/CloudKit" -t "<your game name>.app/Contents/MacOS/<your game name>"**
3. This will modify the Unity binary to load the CloudKit framework at startup.  
	
We found that without this - even though the CloudKit framework is linked in the Prime31 plugin - actual calls to CloudKit will fail with the error "connection to service names com.apple.cloudd was invalidated".

## SIGN & PACKAGE
### GET YOUR TEAM NAME & TEAM ID 

#### OPTION 1
From your keychain. Looks like this “TEAM NAME (XXXXXXXXXX)” Where the XXXXXXXXXX is your team id. Copy it exactly as it is, so no spaces before or after. [Example where to find](https://apple.stackexchange.com/a/312503) 

#### OPTION 2
Select the profile in the finder and press space to preview the file, then look for:

###### Development.profile
	Certificates > Mac Developer : "YOUR NAME (XXXXXXXXXX)"

###### Distribution.profile
	Team > "TEAM NAME (XXXXXXXXXX)" 

#### OPTION 3 
Like in the previous chapter go to [Membership](https://developer.apple.com/account/#/membership/) and copy your team name and team ID there and put it in this format: “TEAM NAME (XXXXXXXXXX)”

**IMPORTANT** If your team name has spaces in it they also have to be included. So full team name including spaces then again a space after and then in between parentheses your team id. Ok three examples:

1. JOHN (DGHHJF45F4)
2. JOHN CRIED (DGHHJF45F4)
3. JOHN CRIED INC (DGHHJF45F4)

#### DEVELOPMENT
To sign your testing builds you need a personal name and id. Note that the ID is different than your team id (the XXXXXXXXXX) You can find them in the same places previously described. The format stays the same only it's your name (if you set up your Mac with your name) So example

1. JOHN HOLEDIGGER (SKI9PF020A)
2. JOHN THE HOLEDIGGER (SKI9PF020A)

### RUN “SignAndPackage”
1. When prompted type your team name
2. When prompted enter : dev, appstore, installer,zip

	| INPUT | WILL CREATE PACKAGE FOR | AT |
	|:--|:--|:--|
	| dev | Development | 7_Distribution/VERSION/Development |
	| appstore| Appstore | 7_Distribution/VERSION/Appstore/ |
	| installer | Distribute outside Appstore | 7_Distribution/VERSION/Installer/ |
	| zip | Distribute outside Appstore | 7_Distribution/VERSION/Zip/ |

3. For automation features call `./SignAndPackage -h`

**IMPORTANT** if signing is successful and you have signed for the first time you will be prompted to add “codesign” to your keychain. **Always allow**

**Troubleshouting** 

1. If you have an error on ambiguous identity, you might have double certificates or old ones. [check here](https://stackoverflow.com/a/32926182)
2. If the identity cannot be found double check your keychain and check your certificates & keys to make sure you have them. Otherwise go back to the first chapter and follow create certificates. 
3. If you get the error "No identity found" make sure you installed the correct certificate in your keychain

### NOTARISATION
Based on [this post](https://forum.unity.com/threads/notarizing-osx-builds.588904/) notarisation requires the **--timestamp** & -**-options=runtime** code sign options. ***Runtime will be REQUIRED the first of January****. We have not released a build with notarisation [but there is a full guide by dpid here](https://gist.github.com/dpid/270bdb6c1011fe07211edf431b2d0fe4). 

- [Apple Notarisation General info](https://developer.apple.com/documentation/security/notarizing_your_app_before_distribution) 
- [Apple Notarisation code sign troubleshooting](https://developer.apple.com/documentation/security/notarizing_your_app_before_distribution/resolving_common_notarization_issues)
- [Apple Notarisation Command line](https://developer.apple.com/documentation/security/notarizing_your_app_before_distribution/customizing_the_notarization_workflow)
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

**NOTE** That when zipping your build this profile will not be included as I am not clear yet if this is required. When you build your pkg your profile is integrated. With a zip it would just reside loose in the directory. So as I'm not sure I deleted it before zipping.

### Code signing
[All code needs to be signed for your app to be approved.](https://developer.apple.com/library/archive/documentation/Security/Conceptual/CodeSigningGuide/Procedures/Procedures.html#//apple_ref/doc/uid/TP40005929-CH4-SW5) Sometimes --deep does not work so we manually sign all the code we (know) and loop over all the .bundles in your plugin folder. For us this worked out, but if you know of more generic libraries that should be added please let me know.

| PATH | TYPE |
|:--| :-- |
|all  files in **appDir/Contents/Frameworks/** | .dylib |
|all  files in **appDir/Contents/Plugins/** | .bundle |
|all  files in **appDir/Contents/Plugins/** | .so |
|YOUR_BUILD | .app |

If your code signing fails with the error "your app is not signed at all" look for missed dylibs or plugins in the response and sign them to.

If your build is rejected because of unsigned files you will probably need to do this and codesign with --deep, but when we send a build to the App Store I'll adjust this readme. 

### Verify signed bundle
See if everything worked out.

### Depending on your choice a signed package will be made
	At "/7_Distribution/VERSION/CHOICE"

## DIY CODESIGN & PACKAGE (TERMNIAL)
#### PROVISIONING PROFILE
When creating any installer make a duplicate of the correct provisioning profile, rename it to embedded.provisioningprofile and place it in YOUR_BUILD/Contents. [Read more at Strangeflavour](http://www.strangeflavour.com/creating-mac-app-store-games-unity/)

#### WHAT TO SIGN?
In this order

- You sign all **dylib** files
- You sign all **.bunlde** files
- You sign **your game**

###### EXAMPLE
	codesign --entitlements "DIR_Entitlements" --force --verify --sign "3rd Party Mac Developer Application: TEAM NAME (TEAM_ID)" --preserve-metadata=identifier,entitlements,flags "appDir"

| OPTION | INFO |
|:--|:--|
|--deep | Go through all subfolders to sign everything |
|--force | Resign everything that has been signed |
|--verify | See if things got signed |
|--entitlements | Location of your entitlements |
|--sign | Signing command |
|--timestamp | Required for [Notarisation](https://developer.apple.com/documentation/security/notarizing_your_app_before_distribution/resolving_common_notarization_issues) |
|--options=runtime | Required for [Notarisation](https://developer.apple.com/documentation/security/notarizing_your_app_before_distribution/resolving_common_notarization_issues) |
|--preserve-metadata | Preserve previous signatures [Read More ](https://github.com/bazelbuild/rules_apple/issues/12) |
| [Codesign Manual](https://www.manpagez.com/man/1/codesign/) | Read up on all options|

| SIGN TYPE | GOAL |
|:--|:--|
|--sign "appDir" Mac Developer: DEV NAME (TEAM_ID) | Development |
|--sign "appDir" Developer ID Application: TEAM NAME (TEAM_ID) | Installer outside Appstore |
|--sign "appDir" 3rd Party Mac Developer Application: TEAM NAME (TEAM_ID) | Appstore |

### APPSTORE

	codesign --entitlements "DIR_Entitlements" --sign "3rd Party Mac Developer Application: TEAM NAME (TEAM_ID)" "appDir"

### OUTSIDE APPSTORE

	codesign --entitlements "DIR_Entitlements" --timestamp --options=runtime --sign "Developer ID Application: TEAM NAME (TEAM_ID)" "appDir"

### DEVELOPMENT BUILD
	
	codesign --entitlements "DIR_Entitlements" --sign "Mac Developer: DEV NAME (DEV_ID)" "appDir"

### BUILD PACKAGE TERMINAL 

###### Appstore

	productbuild --component “$appDir” “/Applications” --sign "3rd Party Mac Developer Installer: TEAMNAME (TEAM_ID)" "$appName.pkg

###### Outside Appstore
	productbuild --component “$appDir” “/Applications” --sign "Developer ID Installer: TEAMNAME (TEAM_ID)" "$appName.pkg

# DISTRIBUTION
## GENERAL TROUBLESHOOTING
### CODESIGN ERROR (files not signed)
- Make sure you signed with the correct identity, have all certificates, correct profiles,... 
	- **read the previous chapters** 
	- check with terminal if all identities are present in keychain
	- Check if all plugins & libraries were signed. The code looks for .dylib .plugin .so anything else will not get signed. Error concerning specific files not. being signed, then you should sign them manually (Unfortunately things change all the time ) 
	- double check signatures after codesign
	- Download your provisioning profiles again

### CRASH (Entitlements, codesign)
- Generally crashes concerning entitlements mean that you did not correctly set up either your provisioning profile or your entitlements. Make sure that your provisioning profile is downloaded **BEFORE** all capabilities were **FULLY** set up at Apple Dev center. Then double check to make sure your entitlements match the information on your provisioning profile. 

##### KNOWN EXAMPLES
If your app opens and crashes when you access a capability like iCloud this means you made a mistake in describing iCloud in either Entitlements / provisioning profile ( and subsequently Dev Center)

**Namespace CODESIGNING, Code 0x1**  
DEVELOPMENT BUILDS
Check the iCloud container key in entitlements not matching the used provisioning profile or other features not matching between provisioning profile & iCloud.

APPSTORE BUILDS
You are not allowed to open your builds until they get approved for the Appstore. Tools like sctl will also return rejected. 

### CANNOT FIND INSTALLATION
When Installing the installer will default to the directory of your build. e.g. /1_MyBuild/App and not the application folder. This is because there can only be one installed copy of your final product so if you cannot find your new installation, start by checking if "/1_MyBuild/YOUR.app" was replaced with your install. If so either test the application from here or if you want your testing build in the applications folder, delete your build before installing your final pkg.   

[QUOTE Mark-ffrench](https://forum.unity.com/threads/how-to-open-mac-build-file-after-code-sign.454435/#post-2954548) Installing the pkg file should, in theory, install the app in the /Applications folder. However, there are a couple of other possible issues that you might encounter: If OSX already thinks you have a copy of your app anywhere on your mac, it will install your new version over it. This could be anywhere on your hard drive, so make sure that the app you are trying to run after installation is the one that has actually been updated by the installer. Your best bet is to try track down all copies of your app and delete them.

### OTHER PROBLEMS
Open your game and check the logs (**Applications > Utilities > Console**) for errors and use that as reference to Google yourself out.

**Location Log files** 

	  ~/Library/Containers/<your app ID>/Data/Library/Logs/Unity/Player.log
[**QUOTE "Joel @ Kitteh Face"**](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html)
Note that the game will run in sandbox mode.  This means all of its files will be written to ~/Library/Containers/<your app ID>/Data/Library/.  Where the Unity documentation says the log file writes to ~/Library/Logs/Unity/Player.log, the sandboxed version is in ~/Library/Containers/<your app ID>/Data/Library/Logs/Unity/Player.log.  Also of note, even though the game is sandboxed, it will use the iCloud credentials that the current machine is using.

### iCloud
[**QUOTE "Joel @ Kitteh Face"**](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html)
*Sign into iCloud on your test Mac.  Make sure you have iCloud Drive enabled on your device, or CloudKit will not work.

## DEVELOPER ID - INDEPENDENT DISTRIBUTION 
### Open your app and see if gatekeeper complains
If you cannot open your build it's possible you forgot to uncheck "Mac Appstore validation" in the player settings when you made your Unity build.

### Notarisation
We have no experience with Notarisation so [follow this guide for notarisation by dpid](https://gist.github.com/dpid/270bdb6c1011fe07211edf431b2d0fe4)

**25/10/19** [BUT Read these tests before moving forward] (https://forum.unity.com/threads/hardened-runtime-for-notarization.766262/)

## APPSTORE
### Testing your build
You can test your Appstore build with the "dev" option and "Mac Appstore validation" set too true in Unity ( see previous chapters )

#### SANDBOX LOGIN (testing account login)
[**QUOTE "Zwilnik @ Strange flavour"**](http://www.strangeflavour.com/creating-mac-app-store-games-unity/)
When you launch the game, you should see a dialog pop up that tells you that the game was purchased by a different account, so you need to sign in with one of your Mac App Store Sandbox test IDs here for the game to launch. Don’t use your normal login, it must be a Sandbox ID. You can create sandbox users [here at App Store Connect](https://appstoreconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/users_roles/sandbox_users)


### Go to App Store Connect and open your app again
Fill in the details for your App such as description, keywords,... Make sure here you **DO NOT reference other platforms**. 

Now add Screenshots (Min 1 - Max 10) as they are required for your upload [(Screenshot specifications)](https://help.apple.com/app-store-connect/#/devd274dd925). If you have it also add your app preview video. [guidelines](https://developer.apple.com/app-store/app-previews/)

[**Quote Victor Leung**](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)
After filling in the form, you will also need to spend some time to take screenshots, and crop them in the right size. Only the following sizes are allowed:

- 1280 x 800 pixels
- 1440 x 900 pixels
- 2560 x 1600 pixels
- 2880 x 1800 pixels

**IMPORTANT** Once you upload your build for review with your screenshots, those screenshots are stuck. Meaning you need to upload a new build to replace the previous screenshots.

### Use Transporter to send your app to Apple.

**XCODE11** Download 'Transporter' from the macOS App Store [here](https://apps.apple.com/us/app/transporter/id1450874784?mt=12). (Note: This is Apple's replacement for Application Loader, which was originally part of Xcode, but was removed in Xcode 11). Run Transporter, sign in, drag the pkg into the app and upload.

DEPRICATED 

**XCODE11** Application loader has been removed from Xcode 11 so either download an older version [here](https://developer.apple.com/download/more/?name=Xcode) Or download [application Loader 3.0 here](https://itunesconnect.apple.com/apploader/ApplicationLoader_3.0.dmg)

To use the old Application loader you need to generate an APP-SPECIFIC PASSWORD [at Apple here] (https://appleid.apple.com/account/manage) and give this password when prompted.

**CREDIT** [Link](https://forums.xamarin.com/discussion/170085/xcode-11-and-upload-ipa-file-without-application-loader)

In Xcode Top Menu "Xcode" > Open Developer Tools > Application loader. 

### Check if your build got through. 
Go to [App Store Connect](https://appstoreconnect.apple.com) > Your App > Activity and check if your build is there. After uploading a build it can take **5 minutes to 1 hour** (or longer) to finish processing in Appstore connect.

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

* The review team works weekends (not 100% sure though)
* Updates will always go faster. 
* In our experience reviews always went fast especially on macOS. So if you're waiting longer than 4 days something is probably wrong. We had a build that got stuck in limbo at which point you just have to cancel the review request and apply again.
* [Read more on review time](https://www.quora.com/How-long-is-an-app-in-review-on-iTunes-Connect-Does-it-depend-on-robustness/answer/Michael-Schranz-1)

### Rejected?
If you upload a new package to the Appstore after a rejection, you will not be able to reply on the previous answer of Apple. So if there are things you need to tell/ask the Review people do it when your App gets rejected in Appstore Connect.

[**QUOTE "Victor Leung"**](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)
If you got an email with issues,
Invalid Signature — The main app bundle game at path GAMENAME.app has following signing error(s): invalid Info.plist (plist or signature have been modified) In architecture: i386 .
It is probably one of your subcomponent didn’t sign correctly, which you may need to spend sometime on Apple documentations to understand how it works.

[READ UP ON : Most common reasons for rejection](https://rollout.io/blog/how-long-does-the-apple-review-process-take/)

### In app purchase have a delay
If you are uploading an app with IAP for the first time you have to know that it takes some time for the IAP to get linked with Apple (this also applies to future updates, though updates are alot faster) With us it took about 6 hours (on iOS) so don't do like we did and panic at 6 O'clock in the morning, just go to sleep and give it some time :)  

### You need to upload a new build?
Don't worry you don't need to go through everything again. You just run RepeatForUpdatedBuild and it will go through all the steps with the data you used. But don't forget to increase the version and/or build number in Info.plist for a new build.

### Cry yourself to sleep

