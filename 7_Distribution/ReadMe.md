# DISTRIBUTION

## GENERAL
#### Installing pkg
When Installing the installer will default to the directory of your build. e.g. /1_MyBuild/App and not the application folder. So either test the application from here or if you want your testing build in the applications folder, delete your build before installing your final pkg.   

#### Problems?
Open your game and check the logs (**Applications > Utilities > Console**) for errors and use that as reference to Google yourself out.

## APPSTORE DEVELOPMENT / TEST BUILD 

### iCloud
[**QUOTE "Joel @ Kitteh Face"**](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html)
*Sign into iCloud on your test Mac.  Make sure you have iCloud Drive enabled, or CloudKit will not work.

### Sandbox Login
[**QUOTE "Zwilnik @ Strange flavour"**](http://www.strangeflavour.com/creating-mac-app-store-games-unity/)
When you launch the game, you should see a dialog pop up that tells you that the game was purchased by a different account, so you need to sign in with one of your Mac App Store Sandbox test IDs here for the game to launch. Don’t use your normal login, it must be a Sandbox ID. You can create sandbox users [here at iTunes Connect](https://appstoreconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/users_roles/sandbox_users)

### Location Log files 
	  ~/Library/Containers/<your app ID>/Data/Library/Logs/Unity/Player.log
[**QUOTE "Joel @ Kitteh Face"**](http://www.kittehface.com/2019/06/unity-games-using-cloudkit-on-macos-part1.html)
Note that the game will run in sandbox mode.  This means all of its files will be written to ~/Library/Containers/<your app ID>/Data/Library/.  Where the Unity documentation says the log file writes to ~/Library/Logs/Unity/Player.log, the sandboxed version is in ~/Library/Containers/<your app ID>/Data/Library/Logs/Unity/Player.log.  Also of note, even though the game is sandboxed, it will use the iCloud credentials that the current machine is using.

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

### In app purchases
If you are uploading an app with IAP for the first time you have to know that it takes some time for the IAP to get linked with Apple (this also applies to future updates, though updates are alot faster) With us it took about 6 hours (on iOS) so don't do like we did and panic at 6 O'clock in the morning, just go to sleep and give it some time :)  

### You need to upload a new build?
Don't worry you don't need to go through everything again. You just run RepeatForUpdatedBuild and it will go through all the steps with the data you used. But don't forget to increase the version or build number in Info.plist for a new build.

### Cry yourself to sleep

