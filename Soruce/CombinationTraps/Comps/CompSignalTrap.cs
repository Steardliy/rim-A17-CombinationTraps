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
        public override IEnumerable<SignalVerb> MakeVerbs()
        {
            if (!base.CanTransmitSignal())
            {
                yield break;
            }
            yield return new SignalVerb_Delay
            {
                action = () =>
                {
                    var trap = base.parent as Building_Trap;
                    trap?.Spring(null);
                },
                label = "DelayedLaunch".Translate(),
                desc = "DelayedLaunchDesc".Translate(),
                texture = CT_TexCommandOf.SignalVerb_DelayedLaunch
            };

            yield return new SignalVerb_DelayUpdated
            {
                action = () =>
                {
                    var trap = base.parent as Building_Trap;
                    trap?.Spring(null);
                },
                label = "DelayedLaunchUpdated".Translate(),
                desc = "DelayedLaunchUpdatedDesc".Translate(),
                texture = CT_TexCommandOf.SignalVerb_DelayedLaunchUpdated
            };

            yield return new SignalVerb_Delay
            {
                action = () =>
                {
                    var trap = base.parent as IValidTrap;
                    if (trap != null) { trap.IsValid = true; }
                },
                triggeredSub = () =>
                {
                    var trap = base.parent as IValidTrap;
                    if (trap != null) { trap.IsValid = false; }
                },
                label = "Suppression".Translate(),
                desc = "SuppressionDesc".Translate(),
                texture = CT_TexCommandOf.SignalVerb_Suppression
            };
        }
    }
}
