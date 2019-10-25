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


### Create a new app in iTunes Connect
Go to [iTunes connect](https://appstoreconnect.apple.com) and create a new (macOS) app.

**[QUOTE Victor Leung](https://medium.com/@victorleungtw/submit-unity-3d-game-to-mac-app-store-1b99c3b31412)** *Login to iTunes Connect, choose My Apps > “+” > “New Mac App”, fill in the values and choose the bundle ID matches with the previous step. The prefix field would be the game name, such us ufo in my case.* 

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

Don't use 0 in your decimals when you define your version.  e.g. to 1.1XX. If you use 1.01 Apple will set your version to 1.1 on Itunes Connect, it will show 1.01 on the Appstore page, but you will not be able to upload a 1.1 version to iTunes Connect anymore forcing you to jump from 1.01 to 1.11. So either 1.0.1 or 1.11

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
