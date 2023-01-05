using HarmonyLib;
using PotionCraft.ManagersSystem.Game;
using PotionCraft.SceneLoader;

namespace LegendaryRecipePin.Patches
{
    class GameManagerStart
    {
        [HarmonyPostfix, HarmonyPatch(typeof(GameManager), "Start")]
        public static void Start_Postfix()
        {
            ObjectsLoader.AddLast("SaveLoadManager.SaveNewGameState", () => PinHolder.MakePinHolder());
            ObjectsLoader.AddLast("SaveLoadManager.SaveNewGameState", () => Utilities.TextureService.LoadTextures());
            ObjectsLoader.AddLast("SaveLoadManager.SaveNewGameState", () => ButtonManager.CreateMachineButtons());
        }
    }
}
