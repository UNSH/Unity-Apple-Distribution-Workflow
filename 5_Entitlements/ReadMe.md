# Entitlements

## WHAT YOU NEED TO DO
### CREATE ENTITLEMENTS FILE
Create an entitlements filed named "entitlements.plist" and place it in either Development or Distribution depending on your needs. ***Again open this entitlements.plist with XCode not in text editor to avoid typo's.*** 

| DIR | 
|:--|
|/5_Entitlements/Distribution/entitlements.plist|
|/5_Entitlements/Development/entitlements.plist|

### WHAT DO I PUT IN

Change it and basically describe what your app will need. At the very least it always needs *com.apple.security.app-sandbox set to yes*.

* [Find more info on all keys here.](https://developer.apple.com/library/archive/documentation/Miscellaneous/Reference/EntitlementKeyReference/Chapters/EnablingAppSandbox.html#//apple_ref/doc/uid/TP40011195-CH4-SW5)
* There are other examples in "5_Entitlements/Examples" folder to use as a reference.
* You can find **com.apple.application-identifier** & **com.apple.developer.team-identifier** in your provisioining profile. 
* Alternatively TeamID can also be found in [Membership tab of Apple Developer Portal](https://developer.apple.com/account/#/membership/)

| KEY | VALUE  | WHEN NEEDED |
|:--|:--|:--|
|com.apple.security.app-sandbox| **YES**| Always |
| com.apple.application-identifier |**TeamID.COM.COMPANY.GAME**  | Always |
| com.apple.developer.team-identifier | **TeamID**  | Always |
| com.apple.security.network.client | **YES** | If accessing things online |
| com.apple.developer.aps-environment | **development** | Development build *WITH* Services |
| com.apple.developer.icloud-container-identifiers | **CloudKit** | iCloud |
| com.apple.developer.icloud-services | **your container identifiers** (the Enabled iCloud Containers in the Capabilities of the Identifier used in your Provisioning Profile, likely in the form  **iCloud.COM.COMPANY.GAME**) | iCloud |

## DEVELOPMENT BUILDS
There is a separate folder with entitlements for the development build. By default it's an entitlements file with only com.apple.security.app-sandbox	set to YES. 

### iCloud Development
In the examples you can find other entitlements and an iCloud example to use it  just move the file from examples to Development and rename it to "entitlement.plist" Be sure to read the Readme of SignAndPackage because you will need to preform another step with optool.

Depending on what you need you can bump into crashes when your app tries to access a specific service of function that requires entitlements. The crash report will tell you that an entitlement is needed. will need to adjust your [entitlements](https://developer.apple.com/library/archive/documentation/Miscellaneous/Reference/EntitlementKeyReference/Chapters/EnablingAppSandbox.html#//apple_ref/doc/uid/TP40011195-CH4-SW5) accordingly.   

#### CREDIT FOR THIS UPDATE [**"Joel @ Kitteh Face"**](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html)

(However, in some cases, improper iCloud entitlements can instead crash on launch with a `Namespace CODESIGNING` error; so if you're seeing this error, double-check your entitlements and the capabilities of the Identifier used in your Provisioning Profile.)

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
Create your entitlements file and place it in the same folder as your build. You could place it anywhere you just have to reference the correct folder when you call codesign in the terminal.
