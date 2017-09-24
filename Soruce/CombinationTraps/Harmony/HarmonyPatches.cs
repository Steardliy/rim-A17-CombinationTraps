using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;
using Verse.Sound;

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
    public static class PreDrawPosCalculation_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            for(int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i].operand.Equals(Hediff_ForceOutside.Default_SpringTightness))
                {
                    codes.Insert(i, new CodeInstruction(OpCodes.Ldarg_0));
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PawnTweener), "pawn")));
                    codes.Insert(i + 2, new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Pawn), "health")));
                    codes.Insert(i + 3, new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Pawn_HealthTracker), "hediffSet")));
                    codes.Insert(i + 4, new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(CT_HediffDefOf), "ForceOutside")));
                    codes.Insert(i + 5, new CodeInstruction(OpCodes.Ldc_I4_0));
                    codes.Insert(i + 6, new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(HediffSet), "GetFirstHediffOfDef", new Type[] { typeof(HediffDef), typeof(bool) })));
                    codes.Insert(i + 7, new CodeInstruction(OpCodes.Isinst, typeof(Hediff_ForceOutside)));
                    codes.Insert(i + 8, new CodeInstruction(OpCodes.Dup));
                    codes.Insert(i + 9, new CodeInstruction(OpCodes.Brtrue_S, 0x0b));
                    codes.Insert(i + 10, new CodeInstruction(OpCodes.Pop));
                    codes.Insert(i + 12, new CodeInstruction(OpCodes.Br, 0x05));
                    codes.Insert(i + 13, new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Hediff_ForceOutside), "DrawPosTightness").GetGetMethod()));
                    break;
                }
            }
            return codes;
        }
    }
    [HarmonyPatch(typeof(Building_Trap), "Spring", new Type[] { typeof(Pawn) })]
    public static class Spring_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            for(int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldstr && codes[i].operand.Equals("DeadfallSpring"))
                {
                    codes.Insert(i, new CodeInstruction(OpCodes.Ldarg_0));
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Thing), "def")));
                    codes.Insert(i + 2, new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ThingDef), "soundDrop")));
                    codes.Insert(i + 3, new CodeInstruction(OpCodes.Dup));
                    codes.Insert(i + 4, new CodeInstruction(OpCodes.Brtrue_S, 0x0b));
                    codes.Insert(i + 5, new CodeInstruction(OpCodes.Pop));
                    break;
                }
            }
            return codes;
        }
    }
    [HarmonyPatch(typeof(Building_TrapRearmable), "Rearm")]
    public static class Rearm_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldstr && codes[i].operand.Equals("TrapArm"))
                {
                    codes.Insert(i, new CodeInstruction(OpCodes.Ldarg_0));
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(Thing), "def")));
                    codes.Insert(i + 2, new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ThingDef), "soundPickup")));
                    codes.Insert(i + 3, new CodeInstruction(OpCodes.Dup));
                    codes.Insert(i + 4, new CodeInstruction(OpCodes.Brtrue_S, 0x0b));
                    codes.Insert(i + 5, new CodeInstruction(OpCodes.Pop));
                    break;
                }
            }
            return codes;
        }
    }
    
}
