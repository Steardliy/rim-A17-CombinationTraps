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
        private HediffComp_TickInterval compInterval;

        public override bool TryMergeWith(Hediff other)
        {
            base.TryMergeWith(other);
            return false;
        }
        public override void PostMake()
        {
            base.PostMake();
            this.compInterval = this.TryGetComp<HediffComp_TickInterval>();
            if (this.compInterval == null)
            {
                Log.Warning(DebugLog.Sign() + "compInterval was null.");
            }
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            Pawn p = base.pawn;
            base.Severity = base.def.initialSeverity;

            float angle = dinfo.HasValue ? dinfo.Value.Angle : 0;
            this.origin = p.TrueCenter();
            this.direction = this.Vector3Round(angle.ToQuat() * Vector3.forward);

            p.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
            int tick = compInterval != null ? compInterval.TickInterval : 1;
            p.stances.SetStance(new Stance_Cooldown((int)base.Severity * tick, p, null));
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsBlockedByAny())
            {
                this.Severity = 0;
                return;
            }
            if (this.Severity >= 0 && (compInterval == null || compInterval.DoTick()))
            {
                base.pawn.Position += this.direction.ToIntVec3();
                base.pawn.pather.Notify_Teleported_Int();
                if (base.pawn.jobs != null && base.pawn.jobs.curJob != null)
                {
                    base.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
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
        private bool IsRange(float a, FloatRange range)
        {
            return a >= range.min && a <= range.max;
        }
    }
}
