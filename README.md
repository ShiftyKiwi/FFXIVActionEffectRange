# ActionEffectRange

A FFXIV Dalamud plugin that provides a visual cue on the effect range of the AoE action the player has just used.

May be used as a supplement/replacement to the actions' VFXs in showing effect range related information,
such as where has the action landed and how large an area it covered.


## How to Install

[XIVLauncher](https://github.com/goatcorp/FFXIVQuickLauncher) is required to install and run the plugin.

Add [https://raw.githubusercontent.com/ShiftyKiwi/MyDalamudPlugins/main/pluginmaster.json](https://raw.githubusercontent.com/ShiftyKiwi/MyDalamudPlugins/main/pluginmaster.json) to Dalamud's Custom Plugin Repositories.

Once added, look for the plugin "ActionEffectRange" in Plugin Installer's available plugins.


## Disclaimer

1. Because the visuals are drawn on an overlay without any current context/knowledge about the in-game geographical features etc.,
   it can sometimes look distorted or "hovered in the air" depending on the terrain and/or camera angle.

2. Please expect errors in calculation.
   There are minor ones due to network latency that are not possible to fix.
   For other errors, please feel free to open issues to report them.

3. Some data (such as Cone AoE angles) are not found in the client (as far as I know).
   For these, I have to find out by myself, but I am unable to guarantee when this could be done
   after each game update when new actions or changes to existing actions are introduced.


## Known Issues

- Overlay rendering does not account for terrain or collision, so circles and lines can still appear visually offset on uneven ground.
- Some action data remains heuristic-driven, especially on brand-new skills or unusual PvP actions after major patches.
