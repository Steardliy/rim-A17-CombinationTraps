using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace CombinationTraps
{
    public abstract class SignalVerbBar : SignalVerb
    {
        public abstract float Max { get; }
        public abstract float Min { get; }
        public virtual string LabelMax => this.Max.ToString("F1");
        public virtual string LabelMin => this.Min.ToString("F1");
        public virtual string LabelValue => this.Value.ToString("F1");
        public virtual string LabelRate => this.Rate.ToString("F1");

        public virtual float Rate { get; protected set; } = 1.0f;

        protected float valueInt;
        public virtual float Value
        {
            get { return this.valueInt; }
            set
            {
                this.valueInt = Mathf.Clamp(value, this.Min, this.Max);
            }
        }
        public virtual void SwitchRate()
        {
            this.Rate = this.Rate == 1.0f ? 0.1f : 1.0f;
        }

        public virtual void Add()
        {
            this.Value += this.Rate;
        }
        public virtual void Sub()
        {
            this.Value -= this.Rate;
        }

        public override IEnumerable<Gizmo> SignalVerbGizmos()
        {
            yield return new GizmoStatus_Bar
            {
                defaultLabel = this.LabelCap,
                defaultDesc = this.Desc,
                verb = this
            };
        }

        public override void Notify_Changed(SignalVerb next)
        {
            base.Notify_Changed(next);
            SignalVerbBar n = next as SignalVerbBar;
            if(n != null)
            {
                n.Value = this.Value;
            }
        }
    }
}
