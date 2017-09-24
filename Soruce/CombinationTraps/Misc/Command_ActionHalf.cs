using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace CombinationTraps
{
    class Command_ActionHalf : Command_Action
    {
        public override float Width
        {
            get
            {
                return base.Width / 2f;
            }
        }
    }
}
