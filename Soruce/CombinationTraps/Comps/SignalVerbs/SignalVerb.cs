using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace CombinationTraps
{
    public class SignalVerb
    {
        public Action action;
        public Action triggeredSub;
        public Action resetSub;

        public string label = "undefined";
        public string desc = "none.";
        public Texture2D texture = BaseContent.BadTex;

        public virtual string Label => this.label;
        public virtual string LabelCap => this.label.CapitalizeFirst();
        public virtual string Desc => this.desc;
        public virtual Texture2D Texture => this.texture;

        protected bool triggeredInt = false;
        public void Triggered()
        {
            this.triggeredSub?.Invoke();
            this.TriggeredSub();
        }
        protected virtual void TriggeredSub()
        {
            this.triggeredInt = true;
        }
        public virtual bool ShouldRun
        {
            get { return triggeredInt; }
        }
        public void Run()
        {
            this.Reset();
            this.action?.Invoke();
        }
        public virtual void Reset()
        {
            this.resetSub?.Invoke();
            this.triggeredInt = false;
        }
        public virtual void Notify_Changed(SignalVerb next)
        {
        }
        public virtual IEnumerable<Gizmo> SignalVerbGizmos()
        {
            yield break;
        }
    }
}
