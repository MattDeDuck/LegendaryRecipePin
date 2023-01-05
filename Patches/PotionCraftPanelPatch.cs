using BepInEx.Logging;
using HarmonyLib;
using PotionCraft.ManagersSystem;
using PotionCraft.NotificationSystem;
using PotionCraft.ObjectBased.UIElements.PotionCraftPanel;
using System.Linq;
using System.Collections.Generic;

namespace LegendaryRecipePin.Patches
{
    class PotionCraftPanelPatch
    {
        static ManualLogSource Log => Plugin.Log;

        /// <summary>
        /// Check to see if list of brewed effects matches the pinned effects
        /// If it matches then the current pinned effects are cleared
        /// </summary>
        public static void CompareBrewedToPinned()
        {
            var brewed = Managers.Potion.potionCraftPanel.previousPotionRecipe.effects;
            List<string> brewedEffects = Utilities.Effects.ToList(brewed.ToList());
            if (Plugin.currentPinned != null)
            {
                var pinnedPotionBrewed = Plugin.currentPinned.Item2.All(brewedEffects.Contains);
                if (pinnedPotionBrewed)
                {
                    Notification.ShowText("Legendary Recipe Pin", "Pinned potion brewed!", Notification.TextType.EventText);
                    PinHolder.Clear();
                    Plugin.currentPinned = null;
                }
            }
            else
            {
                Log.LogInfo("No pinned recipe!");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PotionCraftPanel), "MakePotion")]
        public static void MakePotion_Postfix()
        {
            CompareBrewedToPinned();
        }
    }
}
