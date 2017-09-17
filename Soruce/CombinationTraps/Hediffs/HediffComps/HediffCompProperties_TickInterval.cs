using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace CombinationTraps
{
    class HediffCompProperties_TickInterval : HediffCompProperties
    {
        public int interval = 1;

        public HediffCompProperties_TickInterval()
        {
            this.compClass = typeof(HediffComp_TickInterval);
        }
    }
}
