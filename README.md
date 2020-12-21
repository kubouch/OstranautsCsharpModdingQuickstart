# Assembly Modding Quickstart Guide for Ostranauts

This repository describes how to work with C# mods for Ostranauts.
Since Ostranauts does not support loading custom assemblies (yet?), the procedure is somewhat convoluted and this guide attempts to alleviate the pain of setting it up from scratch.
I'm also writing this down for my future forgetful self.

[Unity Mod Manager (UMM)](https://www.nexusmods.com/site/mods/21) is the core component of this guide.
It uses [Doorstop](https://github.com/NeighTools/UnityDoorstop) for injecting 3rd party code into the game.
You could as well use Doorstop alone but you would need to ensure all the required assemblies (e.g., the ones you want to patch) are loaded prior entering the entry point.
UMM already does that and I'm a C# noob, so I use it for convenience.

*NOTE: I am not a C# programmer nor a native English speaker, so please correct me where I'm wrong and submit a pull request*

This guide is split into three parts:
* [**Part 0: Unity Modding Primer**](#part-0-unity-modding-primer): Intended for those who know 0 about Unity, C# and modding
* [**Part 1: Installing Unity Mod Manager**](#part-1-installing-unity-mod-manager): How to install & configure UMM
* [**Part 2: Installing Mods**](#part-2-installing-mods): How to install an existing C# mod
* [**Part 3: Making Your Own Mod**](#part-3-making-your-own-mod): How to make your own C# mod


## Part 0: Unity Modding Primer

Ostranauts is made in a Unity engine and uses the C# language to describe most of its internal logic.
A compiled C# code is stored in assembly .dll files -- those are kept in `Ostranauts/Ostranauts_Data/Managed`.
To browse the compiled assembly source, you can use a decompiler such as [ILSpy](https://github.com/icsharpcode/ILSpy) or [dnSpy](https://github.com/0xd4d/dnSpy).
Most likely, you want to load the `Assembly-Csharp.dll`.

A lot of the data is already exposed and modifiable via json and image/audio files (you can find these in the `Ostranauts/Ostranauts_Data/Streaming_Assets` folder).
You don't need C# to modify those, a text editor is enough.
So it is worth looking into the files to see what you can do.
For example, replacing textures or adding items based on the existing ones is typically possible without touching the assemblies.

To modify the game's assembly you have two choices:
1. Use a tool like dnSpy to edit and recompile the `Assembly-Csharp.dll`: This might be quick and convenient, however, it breaks the compatibility with other mods and you might get into legal trouble by redistributing the altered assembly so you probably shouldn't do that.
2. Trick the game into executing your assembly's code which can have its own logic or modify the game's code. Let's use this option.

By default, Ostranauts does not support loading any other assembly files than its own.
[Doorstop](https://github.com/NeighTools/UnityDoorstop) is a tool used to hijack the Unity engine to execute a function within a 3rd party assembly.
Ensuring that the function is loaded properly with just plain Doorstop can be tricky (at least for my level of C# noobiness).
[Unity Mod Manager (UMM)](https://www.nexusmods.com/site/mods/21) is a tool that takes care of that (among other things).
Doorstop executes the UMM loader and UMM then makes sure your assembly is loaded properly.

To modify the game's assembly, you can use [Harmony](https://harmony.pardeike.net) to create patches.
Using Harmony has the advantage of being relatively user friendly and also supports multiple mods, merging their patches together.
The Harmony's docummentation explains it better than me so please go there for more details.


## Part 1: Installing Unity Mod Manager

You need Unity Mod Manager (UMM) in order to install C# mods to Ostranauts, both yours and others'.

1. Go to [UMM homepage](https://www.nexusmods.com/site/mods/21), download and unpack anywhere

2. Append the following to the unpacked `UnityModManager/UnityModManagerConfig.xml` file:
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


## Part 2: Installing Mods

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

To verify this part, you can use the 'ModExample' mod bundled with this guide (go to Releases and grab the latest `ModExample.zip`, make sure it corresponds to your Ostranauts vesion).
This magnificent mod will turn every button in the main menu to appear as the 'BBG' button.
Once you verified it works, uninstall the mod because it's useless.


## Part 3: Making Your Own Mod

First, you need a decompiler (such as [ILSpy](https://github.com/icsharpcode/ILSpy) or [dnSpy](https://github.com/0xd4d/dnSpy) to browse the game source code and Visual Studio 2017 or 2019 to compile your code.
The steps for setting up a Visual Studio solution can be derived from [UMM Wiki](https://wiki.nexusmods.com/index.php/How_to_create_mod_for_unity_game) and to some extent [RimWorld Modding Wiki](https://www.rimworldwiki.com/wiki/Modding_Tutorials/Setting_up_a_solution).
You can also use the [ModExample](ModExample) mod as a [MWE](https://en.wikipedia.org/wiki/Minimal_working_example).

Basically, you need the following:
  * Create a new solution (Visual Studio -> Create a new project -> Class Library (.NET Framework) **not .NET SomethingElse!** -> Choose name etc. and .NET Framework 3.5
  * Add the following assemblies as references (right click Reference in the Solution Explorer -> Add Reference), these were enough to compile ModExample:
    * `Assembly-CSharp.dll`, `UnityEngine.dll` from 'Ostranauts/Ostranauts_Data/Managed'
    * `UnityModManager.dll`, `0Harmony.dll` from  'Ostranauts/Ostranauts_Data/Managed/UnityModManager'
  * Set 'Copy Local' to False in the Properties of each reference to avoid copying the game's files around
  * In the Project (top menu) -> <Mod Name> Properties:
    * Application -> make sure target framework is .Net Framework 3.5
    * Build -> set output path for your assembly according to your preference, possibly close to Info.json   
    * [RimWorld Wiki](https://www.rimworldwiki.com/wiki/Modding_Tutorials/Setting_up_a_solution) recommends to set Build -> Advanced -> Debugging information to none
  * Set up the `Info.json` to point at your assembly and its entry point (e.g., SomeMod.dll and SomeMod.Main.Load) -- see the UMM guide for details 

Harmony is shipped with UMM so you can start patching straight away.


## References

Guides for Unity Mod Manager: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager

Rimworld C# Modding tutorials (note that Rimworld supports loading 3rd party assemblies, unlike Ostranauts): https://www.rimworldwiki.com/wiki/Modding_Tutorials#C.23_tutorials

Harmony documentation: https://harmony.pardeike.net

ILSpy: https://github.com/icsharpcode/ILSpy

dnSpy: https://github.com/0xd4d/dnSpy

Doorstop:  [Doorstop](https://github.com/NeighTools/UnityDoorstop)


## License

This repository is published under a public domain.
It has no author and is not anybody's property.
Nobody made it -- it just made itself.
