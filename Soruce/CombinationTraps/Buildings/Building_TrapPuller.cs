using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace CombinationTraps
{
    class Building_TrapPuller : Building_TrapContinue
    {
        protected override DamageDef dDef => CT_DamageDefOf.Pull;
    }
}
