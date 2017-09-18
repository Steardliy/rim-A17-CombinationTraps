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
        public Vector3 Origin { get; private set; }
        public IntVec3 Direction { get; set; }
        private int tickInterval;

        public override bool TryMergeWith(Hediff other)
        {
            base.TryMergeWith(other);
            base.Severity = 0;
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

            this.Origin = p.TrueCenter();
            this.Direction = angle.AsIntVec3();

            p.jobs.EndCurrentJob(JobCondition.InterruptForced, false);

            Stance_Busy stanceBusy = p.stances.curStance as Stance_Busy;
            int num = Mathf.RoundToInt(base.Severity) * this.tickInterval;
            if (stanceBusy == null || stanceBusy.ticksLeft < num)
            {
                Stance_Cooldown stanceCooldown = new Stance_Cooldown(num, p, null);
                stanceCooldown.neverAimWeapon = true;
                p.stances.SetStance(stanceCooldown);
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsBlockedByAny())
            {
                base.Severity = 0;
                return;
            }
            if (base.Severity >= 0 && this.DoTick())
            {
                Log.Message("preTweenedPos=" + base.pawn.Drawer.tweener.TweenedPos);
                Vector3 v = base.pawn.Drawer.tweener.TweenedPos;
                
                Log.Message("postTweenedPos=" + base.pawn.Drawer.tweener.TweenedPos);
                base.pawn.Position += this.Direction;
                this.ResetPath();
                base.Severity--;
            }
        }

        private bool IsBlockedByAny()
        {
            IntVec3 vec = base.pawn.Position + this.Direction;
            Map map = base.pawn.Map;
            if (!vec.InBounds(map) || !vec.Walkable(map))
            {
                return true;
            }
            return false;
        }
        private bool DoTick()
        {
            return Find.TickManager.TicksGame % this.tickInterval == 0;
        }
        private void ResetPath()
        {
            Pawn_PathFollower pf = base.pawn.pather;
            if (pf.curPath != null)
            {
                pf.curPath.ReleaseToPool();
            }
            pf.curPath = null;
            pf.ResetToCurrentPosition();
        }
    }
}
