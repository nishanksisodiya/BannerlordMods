using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using NavalDLC.GameComponents;
using BanditSpeedTweaks.Loader;

namespace BanditSpeedTweaks.NavalDLC
{
    public class CustomBanditSpeedModel : NavalDLCPartySpeedCalculationModel
    {
        private static Dictionary<string, float> _banditSpeedMultipliers;
        private static bool _configLoaded = false;

        public CustomBanditSpeedModel()
        {
            if (!_configLoaded)
            {
                _banditSpeedMultipliers = Utility.LoadConfig();
                _configLoaded = true;
            }
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
