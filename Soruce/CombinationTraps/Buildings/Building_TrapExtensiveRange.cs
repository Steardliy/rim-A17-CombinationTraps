using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace CombinationTraps
{
    class Building_TrapExtensiveRange : Building_TrapExplosive
    {
        protected CompPowerTrader powerTraderComp;
        protected CompSignalTrap trapSignalComp;

        public bool IsValid { get; set; }
        public virtual bool IsActive
        {
            get { return this.IsValid; }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.trapSignalComp = base.GetComp<CompSignalTrap>();
            this.powerTraderComp = base.GetComp<CompPowerTrader>();
            this.IsValid = true;
        }

        protected override void SpringSub(Pawn p)
        {
            if (!this.IsActive) { return; }
            this.trapSignalComp?.TrySpreadSignal(this.powerTraderComp);
            this.IsValid = false;

            base.GetComp<CompExplosive>().StartWick(null);
        }
    }
}
