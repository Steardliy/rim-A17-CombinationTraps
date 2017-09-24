using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace CombinationTraps
{
    class Hediff_ForceOutside : HediffWithComps
    {
        public const float Default_SpringTightness = 0.09f;
        public const float SpringTightnessCoefficient = 0.4f;
        private const float SpringTightnessRangeMin = 0.04f;
        private const float SpringTightnessRangeMax = 0.4f;

        public IntVec3 Direction { get; set; }

        private float drawPosTightnessInt;
        public float DrawPosTightness
        {
            get { return this.drawPosTightnessInt; }
            private set
            {
                drawPosTightnessInt = Mathf.Clamp(value, SpringTightnessRangeMin, SpringTightnessRangeMax);
            }
        }
        private int tickInterval;
        private float momentumInt;
        public float Momentum
        {
            get { return this.momentumInt; }
            set
            {
                this.momentumInt = Mathf.Clamp(value, CT_StatDefOf.Momentum.minValue, CT_StatDefOf.Momentum.maxValue);
                this.tickInterval = Mathf.RoundToInt(1f / (this.momentumInt <= 0 ? 0.01f : this.momentumInt));
                this.CorrectDrawPos();
            }
        }
        public override float Severity
        {
            get { return base.severityInt; }
            set
            {
                base.severityInt = Mathf.Clamp(value, CT_StatDefOf.ImpactForce.minValue, CT_StatDefOf.ImpactForce.maxValue);
                this.RestrictPawnBehavior();
            }
        }
        protected virtual bool ShouldDoTick => Find.TickManager.TicksGame % this.tickInterval == 0;

        public override bool TryMergeWith(Hediff other)
        {
            base.TryMergeWith(other);
            base.severityInt = 0;
            return false;
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);

            if (dinfo.HasValue)
            {
                DamageInfo info = dinfo.Value;
                this.Momentum = info.Instigator.GetStatValue(CT_StatDefOf.Momentum);
                this.Severity = info.Instigator.GetStatValue(CT_StatDefOf.ImpactForce);
                this.Direction = info.Angle.AsIntVec3();
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (this.ShouldRemove)
            {
                return;
            }
            if (this.IsBlockedByAny())
            {
                base.severityInt = 0;
                return;
            }
            if (this.ShouldDoTick)
            {
                base.pawn.Position += this.Direction;
                this.ResetPath();
                base.severityInt--;
            }
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (!base.pawn.health.hediffSet.HasHediff(CT_HediffDefOf.ForceOutside))
            {
                this.DrawPosTightness = Hediff_ForceOutside.Default_SpringTightness;
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
        private void CorrectDrawPos()
        {
            this.DrawPosTightness = this.Momentum * Hediff_ForceOutside.SpringTightnessCoefficient;
        }
        private void RestrictPawnBehavior()
        {
            Pawn p = base.pawn;

            Stance_Busy stanceBusy = p.stances.curStance as Stance_Busy;
            int num = Mathf.RoundToInt(this.Severity) * this.tickInterval;
            if (stanceBusy == null || stanceBusy.ticksLeft < num)
            {
                base.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
                Stance_Cooldown stanceCooldown = new Stance_Cooldown(num, p, null);
                stanceCooldown.neverAimWeapon = true;
                p.stances.SetStance(stanceCooldown);
            }
        }
    }
}
