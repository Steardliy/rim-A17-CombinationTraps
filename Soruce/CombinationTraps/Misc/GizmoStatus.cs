using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace CombinationTraps
{
    class GizmoStatus : Command
    {
        public float width = 180f;
        public override float Width => width;

        public bool doTooltipInt = false;
        protected override bool DoTooltip => this.doTooltipInt;
    }
}
