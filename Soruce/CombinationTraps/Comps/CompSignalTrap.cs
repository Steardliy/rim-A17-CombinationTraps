using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace CombinationTraps
{
    public class CompSignalTrap : CompSignal
    {
        public override void CompTick()
        {
            if (base.CurBehavior != default(SignalBehavior) && base.CurBehavior.ShouldRun)
            {
                var trap = base.parent as Building_Trap;
                trap?.Spring(null);
            }
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!base.compDef.transmissionStanceFilter.Contains(TransmissionStance.OnlyReceive)
                || !base.compDef.transmissionStanceFilter.Contains(TransmissionStance.Any))
            {
                base.behaviors.Add(new SignalBehavior_Delay());
                base.behaviors.Add(new SignalBehavior_LastDelay());
            }
        }

        public override void Transmit()
        {
            base.CurBehavior?.Triggered();
        }
    }
}
