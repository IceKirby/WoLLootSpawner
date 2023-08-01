# Wizard of Legend Loot Spawner Mod

Adds a menu that can be accessed anytime to spawn any Relic or Arcana. In other words, it's a cheat mod.

Press F1 to bring up the menu. If you wish to set it to a different key, edit the `DisplaySpawnerMenu` property at the file `GAME_DIR\BepInEx\config\com.RiceKirby.LootSpawner.cfg` (if the files doesn't exist, just run the game once with the mod installed and it will automatically generate it).

If you spawn anything with it, the **Loot Spawner Activated** message will be printed to the screen and won't go away until you close the game. This is intentional, it's a mark that you have cheated, so you won't be able to post videos or screenshots pretending you got lucky with your build.

# Installation (Manual)

- [Make sure you have BepInEx installed](https://github.com/WoL-Modding-Extravaganza/WoLWiki/wiki/Using-Mods:-Installing-BepInEx).
  
- Extract the zip file contents to the `GAME_DIR\BepInEx\plugins\` folder.

# Known Issues

- The menu can be brought up at times you shouldn't try to, like while talking to an NPC. Try to refrain from doing so, as that can lead to weird behaviour, like the NPC UI staying open even after you finish talking to it. If that happens, you can try to open the Loot Spawner menu again, then press the key to close the NPC UI, then close the Spawner menu (can't guarantee it will always solve it, so if this ruins your run, sorry).
- Some items like Gold and Gems have a limit to how many can be on the ground simultaneously, so sometimes it will spawn less than what you selected. If that happens, just collect the ones lying around the ground, wait 1 or 2 seconds and then trying again.
- No mouse navigation. It may be added in the future if I ever feel like dealing with Unity UI system again.

# Developing  
  
If you wish to modify this mod, follow the [instructions here](https://github.com/WoL-Modding-Extravaganza/WoLWiki/wiki/Mod-Creation:-The-Project-Environment) to setup the project environment.