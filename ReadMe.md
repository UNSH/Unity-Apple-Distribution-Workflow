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

### Entitlements.plist templates
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

	./RepeatForUpdatedBuild -q -t appstore -i 'TEAM NAME (XXXXXXXXXX)' -s deep

The only thing that needs to be done manually is placing the build in the right directory, and uploading the final package via Application Loader.

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

 


