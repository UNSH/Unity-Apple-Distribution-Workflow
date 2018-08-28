# Sign, package build & Deliver
## SIGN & PACKAGE
### Get your team name & team id 
###### OPTION 1
From your keychain. Looks like this “TEAM NAME (XXXXXXXXXX)” Where the XXXXXXXXXX is your team id. Copy it exactly as it is, so no spaces before or after. [Example where to find](https://apple.stackexchange.com/a/312503) 

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

### What to sign?
In this order

- You sign both **libmono.0.dylib** & **libMonoPosixHelper.dylib** 
- You sign all your **plugins**
- You sign **your game** with the "--deep" option

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
	productbuild --component “$appDir” “/Applications” --sign "3rd Party Mac Developer Installer: TEAMNAME (TEAM_ID)" "$appName.pkg

###### Outside Appstore
	productbuild --component “$appDir” “/Applications” --sign "Developer ID Installer: TEAMNAME (TEAM_ID)" "$appName.pkg

