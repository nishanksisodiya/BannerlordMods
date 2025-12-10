using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace BanditSpeedTweaks.Loader
{
    public static class Utility
    {
        public static Dictionary<string, float> LoadConfig()
        {
            var banditSpeedMultipliers = new Dictionary<string, float>();
            try
            {
                string configPath = GetConfigPath();

                if (File.Exists(configPath))
                {
                    string jsonContent = File.ReadAllText(configPath);
                    BanditSpeedConfig config = JsonConvert.DeserializeObject<BanditSpeedConfig>(jsonContent);
                    banditSpeedMultipliers = config.BanditSpeedMultipliers;

                    InformationManager.DisplayMessage(new InformationMessage(
                        $"Bandit Speed Mod: Loaded {banditSpeedMultipliers.Count} bandit configurations",
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
                banditSpeedMultipliers = LoadDefaultConfig();
            }

            return banditSpeedMultipliers;
        }

        private static string GetConfigPath()
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Directory.CreateDirectory(documentsFolder);
            var documentsConfigFile = Path.Combine(documentsFolder, "Mount and Blade II Bannerlord", "Configs", "ModSettings", "BanditSpeedTweaks", "BanditSpeedConfig.json");

            var modulePath = ModuleHelper.GetModuleInfo("BanditSpeedTweaks").FolderPath;
            var configPath = Path.Combine(modulePath, @".\ModuleData\BanditSpeedConfig.json");
            if (!File.Exists(documentsConfigFile))
            {
                Directory.CreateDirectory(Path.Combine(documentsFolder, "Mount and Blade II Bannerlord", "Configs", "ModSettings", "BanditSpeedTweaks"));
                File.Copy(configPath, documentsConfigFile, true);
            }


            return documentsConfigFile;
        }


        private static Dictionary<string, float> LoadDefaultConfig()
        {
            return new Dictionary<string, float>
            {
                { "steppe_bandits", 1.0f },
                { "forest_bandits", 1.0f },
                { "mountain_bandits", 1.0f },
                { "desert_bandits", 1.0f },
                { "sea_raiders", 1.0f },
                { "looters", 1.0f },
                { "deserters", 1.0f },
                { "corsairs", 1.0f}
            };
        }
    }
}
