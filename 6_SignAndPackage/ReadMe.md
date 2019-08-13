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

**IMPORTANT** if signing is successful and you have signed for the first time you will be prompted to add “codesign” to your keychain. **Always allow**

**Troubleshouting** 

1. If you have an error on ambiguous identity, you might have double certificates or old ones. [check here](https://stackoverflow.com/a/32926182)
2. If the identity cannot be found double check your keychain and check your certificates & keys to make sure you have them. Otherwise go back to the first chapter and follow create certificates. 
3. If you get the error "No identity found" make sure you installed the correct certificate in your keychain

### NOTARISATION
Based on [this post](https://forum.unity.com/threads/notarizing-osx-builds.588904/) notarisation requires the **--timestamp** & -**-options=runtime** code sign options. We have not released a build with notarisation so will report back when we know more. Both have been included in all distribution code signs.

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
|YOUR_BUILD | .app |

If your code signing fails with the error "your app is not signed at all" look for missed dylibs in the response and sign them to.

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

- You sign all  **dylib** files
- You sign all your **plugins**
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

	codesign --entitlements "DIR_Entitlements" --sign "Developer ID Application: TEAM NAME (TEAM_ID)" "appDir"

### DEVELOPMENT BUILD
###### DYLIB & BUNDLES (Sign without entitlements)

	codesign --sign "Mac Developer: DEV NAME (TEAM_ID)" --preserve-metadata=identifier,entitlements,flags "appDir"

###### YOUR.APP (Sign WITH entitlements)

	codesign --entitlements "DIR_Entitlements" --sign "Mac Developer: DEV NAME (DEV_ID)" "appDir"

### BUILD PACKAGE TERMINAL 

###### Appstore

	productbuild --component “$appDir” “/Applications” --sign "3rd Party Mac Developer Installer: TEAMNAME (TEAM_ID)" "$appName.pkg

###### Outside Appstore
	productbuild --component “$appDir” “/Applications” --sign "Developer ID Installer: TEAMNAME (TEAM_ID)" "$appName.pkg

