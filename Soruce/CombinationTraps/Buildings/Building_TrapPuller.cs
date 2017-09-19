﻿using Verse;

namespace CombinationTraps
{
    class Building_TrapPuller : Building_TrapRange
    {
        protected override DamageDef dDef => CT_DamageDefOf.Pull;

        protected override float ActAngle()
        {
            return base.Rotation.Opposite.AsAngle;
        }
    }
}
