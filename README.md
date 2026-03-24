# ActionEffectRange

A FFXIV Dalamud plugin that provides a visual cue on the effect range of the AoE action the player has just used.

May be used as a supplement/replacement to the actions' VFXs in showing effect range related information,
such as where has the action landed and how large an area it covered.


## How to Install

[XIVLauncher](https://github.com/goatcorp/FFXIVQuickLauncher) is required to install and run the plugin.

Add [https://raw.githubusercontent.com/ShiftyKiwi/FFXIVActionEffectRange/master/pluginmaster.json](https://raw.githubusercontent.com/ShiftyKiwi/FFXIVActionEffectRange/master/pluginmaster.json) to Dalamud's Custom Plugin Repositories.

Once added, look for the plugin "ActionEffectRange" in Plugin Installer's available plugins.

For patch-day debugging or data verification, you can also use `/actioneffectrange dump <actionId>`.
This prints the raw action-sheet values and the plugin's derived/customized effect-range data to the Dalamud log.


## Disclaimer

1. This plugin draws its indicators as an overlay, not as native in-game ground effects.
   Because of that, shapes can still look slightly offset, warped, or elevated on uneven terrain, stairs, steep slopes, and certain camera angles.
   This can be reduced, but not eliminated completely, in a plugin of this type.

2. The plugin uses live action events and local snapshots to estimate where and when an effect should be drawn.
   Most cases can be made very accurate, but small timing differences from network latency, animation timing, and delayed secondary effects are still unavoidable.
   If a skill is consistently wrong rather than slightly delayed, please report it as a bug.

3. Some action properties are not exposed cleanly by the client data and still require maintained overrides or heuristics.
   That mostly affects edge cases such as cone angles, unusual secondary effects, and newly changed skills after a patch.
   These cases are fixable over time, but they are not always discoverable automatically on patch day.


## Known Issues

- Overlay rendering does not account for terrain or collision, so circles and lines can still appear visually offset on uneven ground.
- Some action data remains heuristic-driven, especially on brand-new skills or unusual PvP actions after major patches.


Validated for FFXIV Patch 7.45 HotFix Patch 2 on March 24, 2026.


