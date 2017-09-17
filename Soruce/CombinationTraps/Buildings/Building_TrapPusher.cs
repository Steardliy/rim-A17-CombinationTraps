using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace CombinationTraps
{
    public class Building_TrapPusher : Building_TrapContinue
    {
        protected override DamageDef dDef => CT_DamageDefOf.Push;
    }
}
