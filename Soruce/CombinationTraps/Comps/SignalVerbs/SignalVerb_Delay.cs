using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;

namespace CombinationTraps
{
    class SignalVerb_Delay : SignalVerbBar
    {
        public override float Max => 10f;
        public override float Min => 0f;
        private int delayTick;

        public override bool ShouldRun
        {
            get
            {
                if (base.triggeredInt)
                {
                    this.delayTick--;
                    if(this.delayTick <= 0)
                    {
                        this.delayTick = 0;
                        base.triggeredInt = false;
                        return true;
                    }
                }
                return false;
            }
        }
        protected override void TriggeredSub()
        {
            if (!base.triggeredInt)
            {
                base.triggeredInt = true;
                this.TimeToTick();
            }
        }
        protected void TimeToTick()
        {
            this.delayTick = Mathf.RoundToInt(base.Value * 60f);
        }
    }
}
