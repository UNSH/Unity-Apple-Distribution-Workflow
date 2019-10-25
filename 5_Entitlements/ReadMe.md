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
| com.apple.security.cs.allow-unsigned-executable-memory | **YES** | IAP |
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
