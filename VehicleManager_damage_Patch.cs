using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace Zombs_R_Cute_NoVehicleDamage
{
    [HarmonyPatch(typeof(VehicleManager), nameof(VehicleManager.damage))]
    public static class VehicleManager_damage_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var newCodes = new List<CodeInstruction>();
            bool found = false;
            
            for (int i = 0; i < codes.Count; i++)
            {
                newCodes.Add(codes[i]);

                if (!found &&
                    codes[i + 1].opcode == OpCodes.Ldc_I4_1 &&
                    codes[i + 2].opcode == OpCodes.Stloc_1)
               {
                   newCodes.Add(new CodeInstruction(OpCodes.Ldarg, 4));
                   newCodes.Add(new CodeInstruction(OpCodes.Call,
                       AccessTools.Method(typeof(VehicleManager_damage_Patch), nameof(ShouldAllowVehicleDamage))));
                   i++;
                   found = true;
               }
            }

            foreach (var instruction in newCodes)
            {
                yield return instruction;
            }

            Logger.Log("VehicleDamage_Patch: Patch applied");
        }

        public static bool ShouldAllowVehicleDamage(CSteamID steamID)
        {
            return UnturnedPlayer.FromCSteamID(steamID)?.IsAdmin ?? false;
        }
    }
}