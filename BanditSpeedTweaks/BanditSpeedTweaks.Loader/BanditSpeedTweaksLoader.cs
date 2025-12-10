using System;
using System.IO;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;

namespace BanditSpeedTweaks.Loader
{
    /// <summary>
    /// Loader that detects Naval DLC and loads the appropriate implementation
    /// This DLL should be small and only reference TaleWorlds assemblies
    /// </summary>
    public class LoaderSubModule : MBSubModuleBase
    {
        private MBSubModuleBase _actualImplementation;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            try
            {
                bool hasNavalDLC = IsNavalDLCLoaded();

                var moduleInfo = ModuleHelper.GetModuleInfo("BanditSpeedTweaks");
                string modulePath = moduleInfo.FolderPath;

                string binPath = Path.Combine(modulePath, "bin", "Win64_Shipping_Client");

                string dllToLoad;
                if (hasNavalDLC)
                {
                    dllToLoad = Path.Combine(binPath, "BanditSpeedTweaks.NavalDLC.dll");
                    InformationManager.DisplayMessage(new InformationMessage(
                        "Bandit Speed Tweaks: Loading Naval DLC compatible version",
                        Colors.Green));
                }
                else
                {
                    dllToLoad = Path.Combine(binPath, "BanditSpeedTweaks.Standard.dll");
                    InformationManager.DisplayMessage(new InformationMessage(
                        "Bandit Speed Tweaks: Loading standard version",
                        Colors.Green));
                }

                // Load the appropriate DLL
                if (File.Exists(dllToLoad))
                {
                    Assembly implementation = Assembly.LoadFrom(dllToLoad);
                    Type subModuleType = implementation.GetTypes()
                        .FirstOrDefault(t => t.IsSubclassOf(typeof(MBSubModuleBase)) && t.Name == "SubModule");

                    if (subModuleType != null)
                    {
                        _actualImplementation = (MBSubModuleBase)Activator.CreateInstance(subModuleType);

                        // Call OnSubModuleLoad on the actual implementation
                        var method = subModuleType.GetMethod("OnSubModuleLoad",
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        method?.Invoke(_actualImplementation, null);
                    }
                    else
                    {
                        InformationManager.DisplayMessage(new InformationMessage(
                            "Bandit Speed Tweaks: Error - SubModule class not found in implementation DLL",
                            Colors.Red));
                    }
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage(
                        $"Bandit Speed Tweaks: Error - Implementation DLL not found: {dllToLoad}",
                        Colors.Red));
                }
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(new InformationMessage(
                    $"Bandit Speed Tweaks: Loader Error - {ex.Message}",
                    Colors.Red));
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            // Delegate to the actual implementation
            if (_actualImplementation != null)
            {
                var method = _actualImplementation.GetType().GetMethod("OnGameStart",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                method?.Invoke(_actualImplementation, new object[] { game, gameStarterObject });
            }
        }

        private bool IsNavalDLCLoaded()
        {
            try
            {
                var moduleInfo = ModuleHelper.GetModuleInfo("NavalDLC");
                if (moduleInfo == null)
                    return false;

                // Check if the assembly is actually loaded
                var assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == "NavalDLC");

                return assembly != null;
            }
            catch
            {
                return false;
            }
        }
    }
}