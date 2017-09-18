using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

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
    }

    [DefOf]
    public static class CT_HediffDefOf
    {
        public static HediffDef ForceOutside;
    }
}
