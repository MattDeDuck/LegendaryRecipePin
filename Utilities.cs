using BepInEx.Logging;
using PotionCraft.ManagersSystem;
using PotionCraft.ObjectBased.AlchemyMachine;
using PotionCraft.QuestSystem.DesiredItems;
using PotionCraft.ScriptableObjects;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LegendaryRecipePin
{
    class Utilities
    {
        static ManualLogSource Log => Plugin.Log;

        public static string pluginLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static Texture2D pinButtonBGTex;
        public static Sprite pinButtonBGSprite;

        public static Texture2D pinButtonBlankTex;
        public static Sprite pinButtonBlankSprite;

        public static Dictionary<string, Dictionary<string, string>> legendaryRecipes = new Dictionary<string, Dictionary<string, string>>();

        public class SpriteService
        {
            /// <summary>
            /// Creates a sprite from the texture provided
            /// </summary>
            /// <param name="texture">The name of the texture to create a sprite with</param>
            public static Sprite FromTexture(Texture2D texture)
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }

        public class TextureService
        {
            /// <summary>
            /// Load a texture from the path provided
            /// </summary>
            /// <param name="filePath">The path of the texture to be used</param>
            public static Texture2D LoadTextureFromFile(string filePath)
            {
                var data = File.ReadAllBytes(filePath);

                // Do not create mip levels for this texture, use it as-is.
                var tex = new Texture2D(0, 0, TextureFormat.ARGB32, false, false)
                {
                    filterMode = FilterMode.Bilinear,
                };

                if (!tex.LoadImage(data))
                {
                    throw new Exception($"Failed to load image from file at \"{filePath}\".");
                }

                return tex;
            }

            /// <summary>
            /// Loads the textures needed
            /// </summary>
            public static void LoadTextures()
            {
                pinButtonBGTex = LoadTextureFromFile(pluginLoc + "/pinbuttonbg.png");
                pinButtonBGSprite = SpriteService.FromTexture(pinButtonBGTex);
                pinButtonBlankTex = LoadTextureFromFile(pluginLoc + "/buttonblank.png");
                pinButtonBlankSprite = SpriteService.FromTexture(pinButtonBlankTex);
            }
        }

        public static class Recipe
        {
            /// <summary>
            /// Gets the LegendaryRecipe object of the currently selected legendary recipe
            /// </summary>
            /// <returns>LegendsryRecipe object</returns>
            public static LegendaryRecipe GetCurrentRecipe()
            {
                var lwindow = Managers.Ingredient.legendaryRecipeSubManager.legendaryRecipeWindow;
                var ind = lwindow.currentPageIndex;
                var recipe = lwindow.legendaryRecipes[ind];
                return recipe;
            }

            /// <summary>
            /// Gets the desired item effects from the name of the alchemy machine slot provided
            /// </summary>
            /// <param name="slot">The name of the alchemy machine slot</param>
            /// <returns>DesiredItemEffect object for the given slot</returns>
            public static DesiredItemEffect GetDesired(string slot)
            {
                LegendaryRecipe recipe = GetCurrentRecipe();
                var allSlots = (Slot[])Enum.GetValues(typeof(Slot));
                Slot desiredSlot = allSlots.Where(s => s.ToString() == slot.Substring(0, slot.Length - 8)).First();
                return recipe.DesiredItemPerSlot(desiredSlot) as DesiredItemEffect;
            }
        }

        public static class Effects
        {
            /// <summary>
            /// Converts list of PotionEffect items to a list of the effect names
            /// </summary>
            /// <param name="potionEffects">The PotionEffect list</param>
            /// <returns>List of the potion effect names</returns>
            public static List<string> ToList(List<PotionEffect> potionEffects)
            {
                return potionEffects.Select(s => s.name).ToList();
            }

            /// <summary>
            /// Converts list of PotionEffect effects to a string of effect icons
            /// </summary>
            /// <param name="potioneffects">The PotionEffect list of effects</param>
            /// <returns>A string icon effects to be used with the PinHolder text</returns>
            public static string ToText(List<PotionEffect> potioneffects)
            {
                string text = "";
                potioneffects.ForEach(s => text += "<sprite=\"IconsAtlas\" name=\"" + s.icon.name + "\"><br>");
                return text;
            }

            /// <summary>
            /// Converts list of the potion effect names to PotionEffect item list
            /// </summary>
            /// <param name="potionEffects">The list of potion effect names</param>
            /// <returns>List of PotionEffect items</returns>
            public static List<PotionEffect> ToPotionList(List<string> effects)
            {
                return effects.Select(s => PotionEffect.GetByName(s)).ToList();
            }
        }
    }
}