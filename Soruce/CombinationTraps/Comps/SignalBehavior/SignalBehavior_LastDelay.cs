using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace CombinationTraps
{
    class SignalBehavior_LastDelay : SignalBehavior_Delay
    {
        public override string Desc => "DescSignalBehavior_LastDelay".Translate();
        public override string Label => "LabelSignalBehavior_LastDelay".Translate();
        public override Texture2D Texture => CT_TexCommandOf.SignalBehavior_LastDelay;

        public SignalBehavior_LastDelay() { }
        public SignalBehavior_LastDelay(float delay) : base(delay) { }

        public override void Triggered()
        {
            base.triggeredInt = true;
            this.TimeToTick();
        }
    }
}
