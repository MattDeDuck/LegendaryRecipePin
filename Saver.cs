using HarmonyLib;
using Newtonsoft.Json;
using PotionCraft.ManagersSystem.SaveLoad;
using PotionCraft.SaveFileSystem;
using PotionCraft.SaveLoadSystem;
using PotionCraft.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LegendaryRecipePin
{
    public static class Saver
    {
        #region Customize this for your mod

        /// <summary>
        /// Fill this class with all of the properties you want to save.
        /// These properties can be any type!
        /// </summary>
        public class SaveState
        {
            public Tuple<string, List<string>> CurrentPinnedEffects { get; set; }
        }

        /// <summary>
        /// Use this method to round up all of the data you need to save and put it in a SaveState object
        /// </summary>
        private static SaveState GetSaveState()
        {
            Plugin.Log.LogInfo("saving save state");

            if (Plugin.currentPinned != null)
            {
                return new SaveState
                {
                    CurrentPinnedEffects = new Tuple<string, List<string>>(Plugin.currentPinned.Item1, Plugin.currentPinned.Item2)
                };
            }else
            {
                return new SaveState
                {
                    CurrentPinnedEffects = new Tuple<string, List<string>>("", new List<string>())
                };
            }            
        }

        /// <summary>
        /// Use this method to do whatever you need to do with the loaded data
        /// </summary>
        private static void LoadSaveState(SaveState loadedSaveState)
        {
            Plugin.Log.LogInfo("Loading save state");
            //Check to make sure it deserialized properly
            if (loadedSaveState == null)
            {
                LogError("Error: An error occured during deserialization. Could not find Current Pinned");
                return;
            }

            //Actually load in the data
            Plugin.currentPinned = new Tuple<string, List<string>>(loadedSaveState.CurrentPinnedEffects.Item1, loadedSaveState.CurrentPinnedEffects.Item2);

            List<PotionEffect> effects = Utilities.Effects.ToPotionList(Plugin.currentPinned.Item2);
            string text = Utilities.Effects.ToText(effects);
            PinHolder.SetText(text);
        }

        private static void LogError(string errorMessage)
        {
            //Do some error logging
            Plugin.Log.LogInfo(errorMessage);
        }

        #endregion

        #region patches

        [HarmonyPatch(typeof(SavedState), "ToJson")]
        public class SavedState_ToJson
        {
            static void Postfix(ref string __result)
            {
                StoreData(ref __result);
            }
        }

        [HarmonyPatch(typeof(SaveLoadManager), "LoadSelectedState")]
        public class SaveLoadManager_LoadSelectedState
        {
            static bool Prefix(Type type)
            {
                return RetreiveStoredData(type);
            }
        }

        [HarmonyPatch(typeof(File), "Load")]
        public class File_Load
        {
            static bool Prefix(File __instance)
            {
                return RetrieveStateJsonString(__instance);
            }
        }

        #endregion

        public const string JsonSaveName = "com.mattdeduck.legendaryrecipepin";

        public static string StateJsonString;

        public static void StoreData(ref string result)
        {
            string modifiedResult = null;
            try
            {
                var savedStateJson = result;
                //Serialize data to json
                var toSerialize = GetSaveState();

                if (toSerialize == null)
                {
                    LogError("Error: no data found to save!");
                }

                var serializedData = JsonConvert.SerializeObject(toSerialize, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                var serialized = $",\"{JsonSaveName}\":{serializedData}";
                //Insert custom field at the end of the save file at the top level
                var insertIndex = savedStateJson.LastIndexOf('}');
                modifiedResult = savedStateJson.Insert(insertIndex, serialized);
            }
            catch (Exception ex)
            {
                LogError($"{ex.GetType()}: {ex.Message}\r\n{ex.StackTrace}\r\n{ex.InnerException?.Message}");
            }
            if (!string.IsNullOrEmpty(modifiedResult))
            {
                result = modifiedResult;
            }
        }

        /// <summary>
        /// Reads the raw json string to find our custom field and parse any bookmark groups within it
        /// </summary>
        public static bool RetreiveStoredData(Type type)
        {
            if (type != typeof(ProgressState)) return true;

            try
            {
                var stateJsonString = StateJsonString;
                StateJsonString = null;
                if (string.IsNullOrEmpty(stateJsonString))
                {
                    LogError("Error: stateJsonString is empty. Cannot load data.");
                    return true;
                }

                //Check if there are any existing bookmark groups in save file
                var keyIndex = stateJsonString.IndexOf(JsonSaveName);
                if (keyIndex == -1)
                {
                    LogError("No existing data found during load");
                    return true;
                }

                //Deserialize the bookmark groups from json using our dummy class
                var deserialized = JsonConvert.DeserializeObject<Deserialized<SaveState>>(stateJsonString, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                LoadSaveState(deserialized.DeserializedObject);

            }
            catch (Exception ex)
            {
                LogError($"{ex.GetType()}: {ex.Message}\r\n{ex.StackTrace}\r\n{ex.InnerException?.Message}");
            }
            return true;
        }

        /// <summary>
        /// This method retrieves the raw json string and stores it in static storage for later use.
        /// The StateJsonString is inaccessible later on when we need it so this method is necessary to provide access to it.
        /// </summary>
        public static bool RetrieveStateJsonString(File instance)
        {
            try
            {
                StateJsonString = instance.StateJsonString;
            }
            catch (Exception ex)
            {
                LogError($"{ex.GetType()}: {ex.Message}\r\n{ex.StackTrace}\r\n{ex.InnerException?.Message}");
            }
            return true;
        }

        private class Deserialized<T>
        {
            [JsonProperty(JsonSaveName)]
            public T DeserializedObject { get; set; }
        }
    }
}
