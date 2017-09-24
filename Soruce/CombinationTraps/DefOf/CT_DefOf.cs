using RimWorld;
using Verse;

namespace CombinationTraps
{
    [DefOf]
    public static class CT_DamageDefOf
    {
        public static DamageDef Push;
        public static DamageDef Pull;
    }

    [DefOf]
    public static class CT_StatDefOf
    {
        public static StatDef ImpactForce;
        public static StatDef Momentum;
        public static StatDef TrapRange;
        public static StatDef RearmInterval;
    }

    [DefOf]
    public static class CT_HediffDefOf
    {
        public static HediffDef ForceOutside;
    }
}
