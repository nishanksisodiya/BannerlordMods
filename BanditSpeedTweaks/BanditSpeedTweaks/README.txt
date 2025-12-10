================================================================================
BANDIT SPEED TWEAKS MOD
================================================================================

Version: 1.1.0
Game: Mount & Blade II: Bannerlord
Author: Nony

================================================================================
DESCRIPTION
================================================================================

This mod allows you to customize the movement speed of all bandit party types.
I created this to nerf Steppe Bandits but expanded to support all
bandit parties.

================================================================================
CONFIGURATION
================================================================================

To customize bandit speeds, edit this file:
C:\Users\<UserName>\Documents\Mount and Blade II Bannerlord\Configs\ModSettings\BanditSpeedTweaks\BanditSpeedConfig.json

Change the multiplier values:
- 1.0 = normal speed (100%)
- 0.7 = slower (70% speed)
- 0.5 = half speed (50%)
- 1.5 = faster (150% speed)

Example configuration:
{
  "BanditSpeedMultipliers": {
    "steppe_bandits": 0.7,
    "forest_bandits": 1.0,
    "mountain_bandits": 1.0,
    "desert_bandits": 0.7, 
    "sea_raiders": 1.0,
    "looters": 1.0,
    "deserters": 1.0,
    "corsairs": 1.0
  }
}

After editing, simply restart your game.

================================================================================
DEFAULT SETTINGS
================================================================================

- Steppe Bandits: 70% speed (slower)
- Mountain Bandits: 100% speed (slightly slower)
- Sea Raiders: 100% speed (slightly slower)
- Forest Bandits: 100% speed (normal)
- Desert Bandits: 70% speed (slower)
- Looters: 100% speed (normal)
- Deserters: 100% speed (normal)
- Corsairs: 100% speed (normal)

================================================================================
COMPATIBILITY
================================================================================

This mod should be compatible with most other mods. It only modifies the
party speed calculation.

================================================================================
KNOWN ISSUES
================================================================================

None currently. Please report any bugs in the comments!

================================================================================
CHANGELOG
================================================================================

Version 1.0.0 (Initial Release)
- Configurable speed multipliers for all bandit types
- JSON-based configuration system
- In-game feedback messages
- Default speed reductions for Steppe Bandits, Mountain Bandits, and Sea Raiders

Version 1.1.0 (Added War Sails Support)
- Config file moved to "Documents\Mount and Blade II Bannerlord\Configs\ModSettings\BanditSpeedTweaks"
- Added support for new bandit types added in War Sails DLC (Deserters, Corsairs)
- Fixed an issue where land speed perks were applied when party was in water.