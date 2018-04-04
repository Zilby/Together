At least one of these libraries need to be shipped with your game.

If you're using Unity put the library in your project root.
If you're not using Unity put them somewhere your app is going to be able to find them.

steam_api64.dll - Windows 64
steam_api.dll - Windows x86 (32bit)
libsteam_api64.so - Linux 64
libsteam_api.dylib - OSX

You should also create a steam_appid.txt file in your project root, next to these files. It should contain only the appid of your app.