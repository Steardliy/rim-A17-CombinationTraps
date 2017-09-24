using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;

namespace CombinationTraps
{
    class SignalBehavior_Delay : SignalBehavior
    {
        private const float DelayTimeMax = 99f;
        private const float DelayTimeMin = 0f;

        private int delayTick;
        private float delayTimeInt;
        public float DelayTime
        {
            get { return this.delayTimeInt; }
            set
            {
                this.delayTimeInt = value;
                this.TimeToTick();
            }
        }
        protected float defaultDelayTime;
        protected bool triggeredInt = false;

        public SignalBehavior_Delay() { }
        public SignalBehavior_Delay(float delay)
        {
            this.defaultDelayTime = delay;
            this.DelayTime = delay;
        }

        public override string Desc => "DescSignalBehavior_Delay".Translate();
        public override string Label => "LabelSignalBehavior_Delay".Translate();
        public override string LabelCap
        {
            get
            {
                return (this.delayTimeInt.ToString("f1") + ":" + this.Label).CapitalizeFirst();
            }
        }

        public override Texture2D Texture => CT_TexCommandOf.SignalBehavior_Delay;
        public override bool ShouldRun
        {
            get
            {
                if (this.triggeredInt)
                {
                    this.delayTick--;
                    if(this.delayTick <= 0)
                    {
                        this.delayTick = 0;
                        this.triggeredInt = false;
                        return true;
                    }
                }
                return false;
            }
        }
        public override void Triggered()
        {
            if (!this.triggeredInt)
            {
                this.triggeredInt = true;
                this.TimeToTick();
            }
        }
        public override void Add()
        {
            this.delayTimeInt += this.delayTimeInt < 1 ? 0.1f : 1;
            this.DelayTime = Mathf.Min(this.delayTimeInt, DelayTimeMax);
        }
        public override void Sub()
        {
            this.delayTimeInt -= this.delayTimeInt <= 1 ? 0.1f : 1;
            this.DelayTime = Mathf.Max(this.delayTimeInt, DelayTimeMin) ;
        }
        public override void Reset()
        {
            this.DelayTime = this.defaultDelayTime;
        }
        protected void TimeToTick()
        {
            this.delayTick = Mathf.RoundToInt(this.delayTimeInt * 60f);
        }
    }
}
