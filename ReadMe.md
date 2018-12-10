# UNITY APPLE DISTRIBUTION WORKFLOW

Workflow to automate and guide people in delivering Unity builds inside or outside of the Appstore.

**CAUTION** building for outside the Appstore may still need some work.

[UNITY THREAD](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/)

## READ FIRST !!! - GENERAL PROBLEMS & NOTES

#### Bug Unity 2018 Results in Apple rejection (FIXED)  
There seems to be a bug with Unity 2018 which will have your bundle rejected because of gamekit. So don't upgrade until this is fixed (currently not fixed in 2018.2). [Link to workaround by giorgos_gs](https://forum.unity.com/threads/app-links-against-the-gamekit-framework-reject-by-apple-reviewer.542306/#post-3577490)

#### Bug Unity Purchasing and closing a build
There is a bug with OSX/MacOS and Unitypurchasing. When quitting your build either takes up to 10 seconds to close or outright crashes. We are not 100% sure if it has something to do with our setup or not, so just see if your build closes properly, if it does ignore this. Below is currently the only fix I have found taken from this [Issue](https://issuetracker.unity3d.com/issues/osx-enabling-unitypurchasing-on-mac-standalone-causes-builds-to-hang-when-quitting-them).  

void OnApplicationQuit() {
  if (!Application.isEditor) { System.Diagnostics.Process.GetCurrentProcess().Kill(); } 
}

We decided to leave IAP behind though and disable Purchasing altogether. If you are developing for more Platforms and like us want to use Purchasing in them, just wrap the code for initialising (in the purchasing script) in [platform dependant directives](https://docs.unity3d.com/Manual/PlatformDependentCompilation.html).

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

## PM or ask at [UNITY THREAD](https://forum.unity.com/threads/unity-appstore-distribution-workflow-guide.542735/) or to contribute
As we will not be constantly uploading games to the App Store it might be good to have other people pitching in so that there's a central point to get help that doesn't age. So anyone who wants to become a contributor just pm me on git even if it is to just add some documentation.

## Credit
We do not take credit for anything besides creating this workflow, tying the code we found together in bash with added automation and organising & rehashing documentation. Below are the credits for all the posts, threads we used. If we missed a spot please contact us. 

We created this workflow because delivering a Unity build to Apple is a mess and the answer is spread across several websites which makes it hard to wrap your head around the correct final chronology especially if you're new or on Windows.

There are tools out there like [Signed](https://assetstore.unity.com/packages/tools/utilities/mac-app-store-signed-54970) that will also take care of signing, but often it's better to understand and not depend upon 3rd party assets, especially when things go wrong. We have tried to include paths, actions and DIY as much as possible so you can understand what's happening and where everything is supposed to go.

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




# Updates
#### 1.4
- Updated Readme
#### 1.3
- Fix wrong command for product build
#### 1.2 & 1.3
- Fumblefuck with my phone
#### 1.1 
- Fix folder structure in 7_Distribution
- Fix entitlements file Naming error


 


