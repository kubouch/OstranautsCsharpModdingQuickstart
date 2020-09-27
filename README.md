# Assembly Modding Guide for Ostranauts

This repository describes how to work with C# mods for Ostranauts.
Since Ostranauts does not support loading custom assemblies (yet?), the procedure is somewhat convoluted and this guide attempts to alleviate the pain of setting it up from scratch.
I'm also writing this down for my future forgetful self.

I'm using [Unity Mod Manager](https://www.nexusmods.com/site/mods/21) to load and manage the assemblies.
You could as well use e.g. Doorstep for manual code injection but I found the UMM method quite convenient.

NOTE: I am not a C# programmer so please feel free to correct me where I'm wrong.

## Step 1: Install Unity Mod Manager

You need UMM in order to install C# mods to Ostranauts, both yours and others'.

  1. Go to [UMM homepage](https://www.nexusmods.com/site/mods/21), download and unpack anywhere
  2. Append the following to the unpacked `UnityModManager/UnityModManagerConfig.xml`:
```xml
<GameInfo Name="Ostranauts">
    <Folder>Ostranauts</Folder>
    <ModsDirectory>Mods</ModsDirectory>
    <ModInfo>Info.json</ModInfo>
    <GameExe>Ostranauts.exe</GameExe>
    <EntryPoint>[Assembly-CSharp.dll]MainMenu2.Awake:Before</EntryPoint>
    <StartingPoint>[Assembly-CSharp.dll]MainMenu2.Awake:Before</StartingPoint>        
    <UIStartingPoint>[Assembly-CSharp.dll]MainMenu2.Awake:After</UIStartingPoint>
    <MinimalManagerVersion>0.22.10</MinimalManagerVersion>
</GameInfo>
```
This will add support for Ostranauts in UMM, see: https://wiki.nexusmods.com/index.php/How_to_add_new_game_(UMM)
  3. Run `UnityModManager.exe`. 
    * Select the Ostranauts game
    * Choose the Ostranauts folder in your Steam installation 
    * Make sure 'DoorstopProxy' is selected 
    * Click 'Install'
  4. Run the game. You should see the UMM interface pop up immediately. Check the 'Logs' tab and make sure there are no errors. You can disable the popup by deleting or commenting the line with the `<UIStartingPoint>` tag in the previous step.

UMM is now set up and ready to use.

## Step 2: Installing Mods

A UMM-compatible mod contains an `Info.json` file and one or more `.dll` assembly files, something like this:
```
SomeMod
  |  
  +-- Info.json
  +-- SomeMod.dll
```
or in a zip archive:
```
SomeMod_1.2.3.zip
  |
  +-- SomeMod
        |  
        +-- Info.json
        +-- SomeMod.dll
```

To install the mod you have two options:
  1. Unzip/copy the `SomeMod` folder into `Ostranauts/Mods`
  2. Use the `UnityModManager.exe` to install the zip archive from the Mods tab.
  
Either way, the end result is that you have `SomeMod` folder in the `Ostranauts/Mods` folder.
The mod is now installed and ready to use.

To verify this step, you can use the 'ModExample' mod bundled with this guide.
This magnificent mod will turn every button in the main menu to appear as the 'BBG' button.
Once you verified it works, uninstall the mod because it's useless.

## Step 3: Making Your Own C# Mod

First, you need a a decompiler (such as [ILSpy](https://github.com/icsharpcode/ILSpy) or [dnSpy](https://github.com/0xd4d/dnSpy) to browse the game source code and Visual Studio 2017 or 2019 to compile your code.
The steps for setting up the solution can be derived from [UMM Wiki](https://wiki.nexusmods.com/index.php/How_to_create_mod_for_unity_game) and to some extent [RimWorld Modding Wiki](https://www.rimworldwiki.com/wiki/Modding_Tutorials#C.23_tutorials), no point in repeating it here.
Basically, you need the following:
  * Set up the `Info.json` to point at your assembly and its entry point (e.g., SomeMod.dll and SomeMod.Main.Load)
  * Add the following assemblies (right click Reference in the Solution Explorer -> Add Reference), these were enough to compile ModExample:
    * `Assembly-CSharp.dll`, `UnityEngine.dll` from 'Ostranauts/Ostranauts_Data/Managed'
    * `UnityModManager.dll`, `0Harmony.dll` from  'Ostranauts/Ostranauts_Data/Managed/UnityModManager'
  * You can set 'Copy Local' to False in the Properties of each reference to avoid copying the game's files around
  * In the Project -> <Mod Name> Properties:
    * Application -> set target framework to .Net Framework 3.5
    * Build -> set output path for your assembly according to your preference, possibly close to Info.json   
    * [RimWorld Wiki](https://www.rimworldwiki.com/wiki/Modding_Tutorials/Setting_up_a_solution) recommends to set Build -> Advanced -> Debugging information to none

Harmony is shipped with UMM so you can start patching straight away.
    
## References

Guides for Unity Mod Manager: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager

Rimworld C# Modding tutorials: https://www.rimworldwiki.com/wiki/Modding_Tutorials#C.23_tutorials (note that Rimworld supports loading 3rd party assemblies, unlike Ostranauts)

https://wiki.nexusmods.com/index.php/How_to_create_mod_for_unity_game:
  * 'Assembly-CSharp.dll', 'UnityEngine.dll', not needed: 'UnityEngine.UI.dll', 'Assembly-CSharp-firstpass.dll', 
  * 'UnityModManager.dll', '0Harmony.dll'
  * not present: Unity 2017+ 'UnityEngine.CoreModule.dll', 'UnityEngine.IMGUIModule.dll'
  
## License

This guide is published under a public domain.
It has no author and is not anybody's property.
Nobody wrote it, it just wrote itself.