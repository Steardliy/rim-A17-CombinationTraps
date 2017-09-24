using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace CombinationTraps
{
    class StatWorker_CombinationTrapForce : StatWorker_MeleeDamageAmount
    {
        protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
        {
            return def.building.trapDamageCategory;
        }
    }
}
