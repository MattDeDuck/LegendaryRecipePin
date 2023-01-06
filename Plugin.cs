using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using PotionCraft.ScriptableObjects.Potion;
using System;
using System.Collections.Generic;

namespace LegendaryRecipePin
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, "LegendaryRecipePin", "1.0.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log { get; set; }
        public const string PLUGIN_GUID = "com.mattdeduck.legendaryrecipepin";

        public static Tuple<string, List<string>> currentPinned = new Tuple<string, List<string>>("", new List<string>());
        public static Potion savedPotion;

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Log = this.Logger;

            Log.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            Harmony.CreateAndPatchAll(typeof(Plugin));
            Harmony.CreateAndPatchAll(typeof(PinHolder));
            Harmony.CreateAndPatchAll(typeof(PinButton));
            Harmony.CreateAndPatchAll(typeof(Utilities));
            Harmony.CreateAndPatchAll(typeof(ButtonManager));
            Harmony.CreateAndPatchAll(typeof(Patches.GameManagerStart));
            Harmony.CreateAndPatchAll(typeof(Patches.LegendaryRecipeWindowPatch));
            Harmony.CreateAndPatchAll(typeof(Patches.PotionCraftPanelPatch));
            Harmony.CreateAndPatchAll(typeof(Saver));
            Harmony.CreateAndPatchAll(typeof(Saver.File_Load));
            Harmony.CreateAndPatchAll(typeof(Saver.SavedState_ToJson));
            Harmony.CreateAndPatchAll(typeof(Saver.SaveLoadManager_LoadSelectedState));
            Log.LogInfo($"Plugin {PLUGIN_GUID}: Patch Succeeded!");
        }
    }
}