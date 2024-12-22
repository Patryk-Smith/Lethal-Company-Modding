using BepInEx.Logging;
using FancyManMod.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyManMod
{

    [BepInEx.BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Plugin : BepInEx.BaseUnityPlugin
    {
        private const string ModGUID = "SmithyCo.FancyManMod";
        private const string ModName = "Fancy Man Mod";
        private const string ModVersion = "1.0.0.0";
        private readonly Harmony harmony = new Harmony(ModGUID);

        private static Plugin Instance;

        internal ManualLogSource MLS;

        // Main in
        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            MLS = BepInEx.Logging.Logger.CreateLogSource(ModGUID);
            MLS.LogInfo("Starting " +  ModName + "...");

            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(PlayerControllerBPatch));

        }


    }
}
