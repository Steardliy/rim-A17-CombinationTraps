using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace CombinationTraps
{
    public abstract class SignalBehavior
    {
        public abstract string Label { get; }
        public abstract string LabelCap { get; }
        public abstract string Desc { get; }
        public abstract Texture2D Texture { get; }
        public abstract bool ShouldRun { get; }

        public abstract void Triggered();
        public abstract void Add();
        public abstract void Sub();
        public abstract void Reset();
    }
}
