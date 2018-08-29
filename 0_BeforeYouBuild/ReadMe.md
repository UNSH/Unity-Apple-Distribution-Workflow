# BEFORE YOU BUILD
Just place this workflow somewhere outside of your Unity project.

## INSTRUCTIONS (APPLE)
### Create Apple Developer Account 
If you do not already have one go to [Apple Developer portal](https://developer.apple.com/) and pay 100$ to get confused by professionals. And you might as well enable **AUTO RENEW** in the membership tab at [Apple Developer portal](https://developer.apple.com/). So you avoid surprises. 

**SELLING ON APPSTORE?** If you are not releasing a free app/game and want to get paid outside of the US. Go fix the banking and tax first at [Appstore Connect](https://appstoreconnect.apple.com) >> Agreements, Tax and banking. It's not necessarily something that has to happen now, but get it over with if you are about to release. Prepare for some serious legal and tax lingo.

### Create the Certificates you need
Go to the [Apple Developer Portal](https://developer.apple.com/account/mac/certificate/development) and create your certificates. Depending on where you want to release your game you will need different certs. These serve as an identity that will be added to your iCloud keychain and allow you to codesign and create your provisioning profiles later. [More on certificate names](https://stackoverflow.com/a/13603031)

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

**IMPORTANT** There seems to be a bug with Unity 2018 which will have your bundle rejected because of gamekit. So don't upgrade until this is fixed (currently 2018.2). [Link to workaround by giorgos_gs](https://forum.unity.com/threads/app-links-against-the-gamekit-framework-reject-by-apple-reviewer.542306/#post-3577490) 

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
