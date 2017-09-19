using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Harmony;
using Verse;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace CombinationTraps
{
    [StaticConstructorOnStartup]
    class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.steardliy.CombinationTraps");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
    [HarmonyPatch(typeof(PawnTweener), "PreDrawPosCalculation")]
    public static class PreDrawPosCalculation_Fix
    {
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            foreach (var a in instructions)
            {
                var code = a;
                if (code.opcode == OpCodes.Ldc_R4 && code.operand.Equals(Override_PreDrawPosCalculation.default_SpringTightness))
                {
                    code = new CodeInstruction(OpCodes.Call, typeof(Override_PreDrawPosCalculation).GetProperty("SpringTightness", BindingFlags.Public | BindingFlags.Static).GetGetMethod());
                }
                yield return code;
            }
        }
    }
}
