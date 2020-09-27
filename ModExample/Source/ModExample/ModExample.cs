using System.Reflection;

using HarmonyLib;
using UnityEngine.SceneManagement;
using UnityModManagerNet;

namespace ModExample
{
    static class Main
    {
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony("com.ostranauts.kubouch.mod_example");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnToggle = OnToggle;

            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            modEntry.Logger.Log(SceneManager.GetActiveScene().name);

            return true;
        }
    }

    [HarmonyPatch(typeof(MainMenu2), "GetTexture")]
    static class Patch_MainMenu2_GetTexture
    {
        static void Prefix(ref string strName)
        {
            strName = "GUIBtnBBG.png";
        }
    }

}
