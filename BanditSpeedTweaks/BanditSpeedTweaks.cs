
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ModuleManager;
using Path = System.IO.Path;

namespace BanditSpeedTweaks
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                CampaignGameStarter campaignStarter = (CampaignGameStarter)gameStarterObject;
                campaignStarter.AddModel(new CustomBanditSpeedModel());
            }
        }
    }

    public class BanditSpeedConfig
    {
        public Dictionary<string, float> BanditSpeedMultipliers { get; set; }
    }

    public class CustomBanditSpeedModel : DefaultPartySpeedCalculatingModel
    {
        private static Dictionary<string, float> _banditSpeedMultipliers;
        private static bool _configLoaded = false;

        public CustomBanditSpeedModel()
        {
            if (!_configLoaded)
            {
                LoadConfig();
                _configLoaded = true;
            }
        }

        private void LoadConfig()
        {
            try
            {
                string configPath = GetConfigPath();

                if (File.Exists(configPath))
                {
                    string jsonContent = File.ReadAllText(configPath);
                    BanditSpeedConfig config = JsonConvert.DeserializeObject<BanditSpeedConfig>(jsonContent);
                    _banditSpeedMultipliers = config.BanditSpeedMultipliers;

                    InformationManager.DisplayMessage(new InformationMessage(
                        $"Bandit Speed Mod: Loaded {_banditSpeedMultipliers.Count} bandit configurations",
                        Colors.Green));
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage(
                        $"Bandit Speed Mod: Config file not found at {configPath}, using defaults",
                        Colors.Yellow));
                    LoadDefaultConfig();
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage(
                    $"Bandit Speed Mod: Error loading config - {ex.Message}",
                    Colors.Red));
                LoadDefaultConfig();
            }
        }

        private string GetConfigPath()
        {
            var modulePath = ModuleHelper.GetModuleInfo("BanditSpeedTweaks").FolderPath;
            var configPath = Path.Combine(modulePath, @".\ModuleData\BanditSpeedConfig.json");

            return configPath;
        }


        private void LoadDefaultConfig()
        {
            _banditSpeedMultipliers = new Dictionary<string, float>
            {
                { "steppe_bandits", 1.0f },
                { "forest_bandits", 1.0f },
                { "mountain_bandits", 1.0f },
                { "desert_bandits", 1.0f },
                { "sea_raiders", 1.0f },
                { "looters", 1.0f }
            };
        }

        public override ExplainedNumber CalculateFinalSpeed(MobileParty mobileParty, ExplainedNumber finalSpeed)
        {
            ExplainedNumber result = base.CalculateFinalSpeed(mobileParty, finalSpeed);

            if (mobileParty != null && mobileParty.IsBandit && _banditSpeedMultipliers != null)
            {
                string partyName = mobileParty.Name?.ToString().ToLower() ?? "";

                foreach (var banditType in _banditSpeedMultipliers)
                {
                    string banditKey = banditType.Key.Replace("_", " ");

                    if (partyName.Contains(banditKey))
                    {
                        float multiplier = banditType.Value;
                        if (Math.Abs(multiplier - 1.0f) > 0.001f)
                        {
                            result.AddFactor(multiplier - 1f, new TaleWorlds.Localization.TextObject($"{banditType.Key} modifier"));
                        }
                        break;
                    }
                }
            }

            return result;
        }
    }
}