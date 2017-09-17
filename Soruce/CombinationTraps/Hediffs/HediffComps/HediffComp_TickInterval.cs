using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using Verse.AI;

namespace CombinationTraps
{
    class HediffComp_TickInterval : HediffComp
    {
        private const int IntervalRangeMin = 1;
        private const int IntervalRangeMax = 5;
        
        private int tickIntervalInt;
        public int TickInterval
        {
            get { return this.tickIntervalInt; }
            set
            {
                this.tickIntervalInt = Mathf.Clamp(value, IntervalRangeMin, IntervalRangeMax);
            }
        }

        private HediffCompProperties_TickInterval compDef => (HediffCompProperties_TickInterval)base.props;
        
        public bool DoTick()
        {
            return Find.TickManager.TicksGame % this.TickInterval == 0;
        }
        public override void CompPostMerged(Hediff other)
        {
            var comp = other.TryGetComp<HediffComp_TickInterval>();
            if (comp != null)
            {
                this.TickInterval = Math.Min(comp.TickInterval, this.TickInterval); ;
            }       
        }

        public override void CompPostMake()
        {
            this.TickInterval = this.compDef.interval;
        }
    }
}
