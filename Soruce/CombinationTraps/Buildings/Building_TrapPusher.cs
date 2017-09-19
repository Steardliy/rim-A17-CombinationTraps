using Verse;


namespace CombinationTraps
{
    public class Building_TrapPusher : Building_TrapRange
    {
        protected override DamageDef dDef => CT_DamageDefOf.Push;
    }
}
