using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace CombinationTraps
{
    class SignalVerb_DelayUpdated : SignalVerb_Delay
    {
        protected override void TriggeredSub()
        {
            base.triggeredInt = true;
            base.TimeToTick();
        }
    }
}
