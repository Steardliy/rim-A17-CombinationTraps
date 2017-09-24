using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace CombinationTraps
{
    public static class BehaviorCommandsUtility
    {
        public static IEnumerable<Gizmo> GetParameterCommand(SignalBehavior behavior)
        {
            yield return new Command_ActionHalf
            {
                defaultLabel = "LabelParameterAdd".Translate(),
                defaultDesc = "DescParameterAdd".Translate(),
                icon = CT_TexCommandOf.ParameterCommand_Add,
                action = behavior.Add
            };
            yield return new Command_Action
            {
                defaultLabel = "LabelParameterReset".Translate(),
                defaultDesc = "DescParameterReset".Translate(),
                icon = CT_TexCommandOf.ParameterCommand_Reset,
                action = behavior.Reset
            };
            yield return new Command_ActionHalf
            {
                defaultLabel = "LabelParameterSub".Translate(),
                defaultDesc = "DescParameterSub".Translate(),
                icon = CT_TexCommandOf.ParameterCommand_Sub,
                action = behavior.Sub
            };
        }
    }
}
