using BepInEx.Logging;
using HarmonyLib;
using PotionCraft.ManagersSystem;
using PotionCraft.NotificationSystem;
using PotionCraft.ObjectBased.UIElements.PotionCraftPanel;
using PotionCraft.ScriptableObjects.Potion;
using System;
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
            Potion potion = Managers.Potion.GeneratePotionFromCurrentPotion();

            if (potion == null)
            {
                Log.LogError("Potion was null");
                return;
            }

            if (potion.effects.Length != Plugin.currentPinned.Item2.Count)
            {
                return;
            }

            List<string> pinned = Plugin.currentPinned.Item2;
            List<string> potionEffects = Utilities.Effects.ToList(potion.effects.ToList());
            bool doesBrewedMatchPinned = pinned.All(potionEffects.Contains);

            if (!doesBrewedMatchPinned)
            {
                return;
            }

            Notification.ShowText("Legendary Recipe Pin", "Pinned potion brewed!", Notification.TextType.EventText);
            PinHolder.Clear();
            Plugin.currentPinned = new Tuple<string, List<string>>("", new List<string>());
        }

        [HarmonyPrefix, HarmonyPatch(typeof(PotionFinishingButton), "OnButtonReleasedPointerInside")]
        public static void OnButtonReleasedPointerInside_Prefix()
        {
            CompareBrewedToPinned();   
        }
    }
}
