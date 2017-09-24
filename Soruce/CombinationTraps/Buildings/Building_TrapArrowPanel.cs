using RimWorld;
using UnityEngine;
using Verse;

namespace CombinationTraps
{
    class Building_TrapArrowPanel : Building_TrapElectricRearmable
    {
        protected override void DamagePawn(Pawn p)
        {
            foreach (var a in p.health.hediffSet.GetHediffs<Hediff_ForceOutside>())
            {
                a.Direction = base.Rotation.FacingCell;
                float momentum = this.GetStatValue(CT_StatDefOf.Momentum);
                if(momentum > 0)
                {
                    a.Momentum += momentum;
                }
                a.Severity += this.GetStatValue(CT_StatDefOf.ImpactForce);
            }
        }

        protected override float SpringChance(Pawn p)
        {
            return p.health.hediffSet.HasHediff(CT_HediffDefOf.ForceOutside) ? base.SpringChance(p) : 0;
        }
    }
}
