using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;

namespace CombinationTraps
{
    public class Building_TrapRangeBase : Building_TrapElectricRearmable
    {
        private const float DistanceCorrectionFactor = 0.01f;
        protected int inspectRange;
        protected virtual DamageDef dDef { get; }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.inspectRange = Mathf.RoundToInt(this.GetStatValue(CT_StatDefOf.TrapRange));
        }

        public override void Tick()
        {
            if (!this.IsActive) { return; }

            base.Tick();
            if (this.Armed)
            {
                for (int i = 1; i < this.inspectRange; i++)
                {
                    IntVec3 pos = base.Position + base.Rotation.FacingCell * i;
                    if (!pos.CanBeSeenOver(base.Map))
                    {
                        break;
                    }
                    List<Thing> thingList = pos.GetThingList(base.Map);
                    for (int j = 0; j < thingList.Count; j++)
                    {
                        Pawn pawn = thingList[j] as Pawn;
                        if (pawn != null)
                        {
                            this.CheckSpringRange(pawn, i);
                        }
                    }
                }
            }
        }

        protected override void DamagePawn(Pawn p)
        {
            BodyPartHeight height = (Rand.Value >= 0.666f) ? BodyPartHeight.Middle : BodyPartHeight.Top;
            int num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage, true));
            float angle = this.ActAngle();
            DamageInfo dinfo = new DamageInfo(this.dDef, num, angle, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            dinfo.SetBodyRegion(height, BodyPartDepth.Outside);

            p.TakeDamage(dinfo);
        }
        protected void CheckSpringRange(Pawn p, int dist)
        {
            if(p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
            {
                return;
            }
            if (Rand.Value < this.DistanceCorrection(this.SpringChance(p), dist))
            {
                this.Spring(p);
            }
        }
        protected virtual float ActAngle()
        {
            return base.Rotation.AsAngle;
        }
        private float DistanceCorrection(float chance, int dist)
        {
            return chance - dist * DistanceCorrectionFactor;
        }
    }
}
