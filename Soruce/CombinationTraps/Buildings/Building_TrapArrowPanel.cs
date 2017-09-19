using RimWorld;
using Verse;

namespace CombinationTraps
{
    class Building_TrapArrowPanel : Building_Trap
    {
        protected override void SpringSub(Pawn p)
        {
            foreach (var a in p.health.hediffSet.GetHediffs<Hediff_ForceOutside>())
            {
                a.Direction = base.Rotation.FacingCell;
            }
        }
    }
}
