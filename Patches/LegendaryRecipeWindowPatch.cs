using HarmonyLib;
using PotionCraft.ObjectBased.UIElements.LegendaryRecipe;
using System.Linq;
using UnityEngine;

namespace LegendaryRecipePin.Patches
{
    class LegendaryRecipeWindowPatch
    {
        /// <summary>
        /// Update the position of the buttons
        /// </summary>
        public static void PositionButtons()
        {
            var bs = Resources.FindObjectsOfTypeAll<PinButton>().ToList();
            foreach (var button in bs)
            {
                button.MovePos();
            }
        }
        
        [HarmonyPostfix, HarmonyPatch(typeof(LegendaryRecipeWindow), "OnActiveBookmarkChangedByClickOnBookmark")]
        public static void OnActiveBookmarkChangedByClickOnBookmark_Postfix()
        {
            PositionButtons();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LegendaryRecipeWindow), "Appear")]
        public static void Appear_Postfix()
        {
            PositionButtons();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(LegendaryRecipeWindow), "FlipPage")]
        public static void FlipPage_Postfix()
        {
            PositionButtons();
        }
    }
}
