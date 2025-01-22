using HarmonyLib;
using Rocket.Core.Plugins;
using SDG.Unturned;

namespace Zombs_R_Cute_NoVehicleDamage
{
    public class NoVehicleDamage : RocketPlugin
    {
        protected override void Load()
        {
            Harmony harmony = new Harmony("NoVehicleDamage");
            harmony.PatchAll();
        }
    }
}