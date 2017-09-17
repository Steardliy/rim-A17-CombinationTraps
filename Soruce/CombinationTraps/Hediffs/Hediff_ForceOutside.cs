using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace CombinationTraps
{
    class Hediff_ForceOutside : HediffWithComps
    {
        protected Vector3 origin;
        protected Vector3 direction;
        private int tickInterval;

        public override bool TryMergeWith(Hediff other)
        {
            base.TryMergeWith(other);
            return false;
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            Pawn p = base.pawn;

            float angle;
            if (dinfo.HasValue)
            {
                DamageInfo info = dinfo.Value;
                this.tickInterval = Mathf.RoundToInt(1f / (info.Instigator.GetStatValue(CT_StatDefOf.Momentum)));
                base.Severity = info.Instigator.GetStatValue(CT_StatDefOf.ImpactForce);
                angle = info.Angle;
            }
            else
            {
                this.tickInterval = 1;
                base.Severity = base.def.initialSeverity;
                angle = 0;
            }

            this.origin = p.TrueCenter();
            this.direction = this.Vector3Round(angle.ToQuat() * Vector3.forward);

            p.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
            p.stances.SetStance(new Stance_Cooldown((int)base.Severity * this.tickInterval, p, null));
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsBlockedByAny())
            {
                this.Severity = 0;
                return;
            }
            if (this.Severity >= 0 && this.DoTick())
            {
                base.pawn.Position += this.direction.ToIntVec3();
                base.pawn.pather.Notify_Teleported_Int();
                if (base.pawn.jobs != null && base.pawn.jobs.curJob != null)
                {
                    base.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
                }
                this.Severity--;
            }
        }

        private bool IsBlockedByAny()
        {
            IntVec3 vec = base.pawn.Position + this.direction.ToIntVec3();
            Map map = base.pawn.Map;
            if (!vec.InBounds(map) || !vec.Walkable(map))
            {
                return true;
            }
            return false;
        }
        private Vector3 Vector3Round(Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }
        private bool DoTick()
        {
            return Find.TickManager.TicksGame % this.tickInterval == 0;
        }
    }
}
