using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FancyManMod.Patches
{
    [HarmonyLib.HarmonyPatch(typeof(GameNetcodeStuff.PlayerControllerB))]
    internal class PlayerControllerBPatch
    {

        // Use the following code after the Update() method executes.
        // [HarmonyLib.HarmonyPatch(nameof(GameNetcodeStuff.PlayerControllerB.Update)] Does not work because Update is private
        [HarmonyLib.HarmonyPatch("Update")]
        [HarmonyLib.HarmonyPostfix]
        static void InfiniteSprintPatchUpdate(ref float ___sprintMeter)
        {
            ___sprintMeter = 1f;
        }
    }
}
