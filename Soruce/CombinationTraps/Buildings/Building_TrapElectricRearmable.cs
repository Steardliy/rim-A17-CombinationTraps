using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Harmony;

namespace CombinationTraps
{
    public class Building_TrapElectricRearmable : Building_TrapRearmable, IValidTrap
    {
        protected CompPowerTrader powerTraderComp;
        protected CompSignalTrap trapSignalComp;

        private static readonly FloatRange TrapDamageFactor = new FloatRange(0.7f, 1.3f);
        private static readonly IntRange DamageCount = new IntRange(1, 2);
        private int rearmTickIntervalInt = 0;

        public int RearmTickInterval => this.rearmTickIntervalInt;
        public bool IsValid { get; set; }
        public virtual bool IsActive
        {
            get { return this.IsValid && (powerTraderComp == null || powerTraderComp != null && powerTraderComp.PowerOn); }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.IsValid = true;
            this.trapSignalComp = base.GetComp<CompSignalTrap>();
            this.powerTraderComp = base.GetComp<CompPowerTrader>();
            if (this.powerTraderComp != null)
            {
                AccessTools.Field(typeof(Building_TrapRearmable), "autoRearm").SetValue(this, true);
            }
        }

        public override void Tick()
        {
            if (!this.IsActive) { return; }

            if (powerTraderComp != null)
            {
                if (!this.Armed && GetAutoRearm())
                {
                    this.rearmTickIntervalInt--;
                    if (this.rearmTickIntervalInt <= 0)
                    {
                        this.rearmTickIntervalInt = 0;
                        base.Rearm();
                        Designation des = base.Map.designationManager.DesignationOn(this);
                        des?.Delete();
                    }
                }
            }
            base.Tick();
        }
        protected override void SpringSub(Pawn p)
        {
            if(!this.IsActive){ return; }
            this.trapSignalComp?.TrySpreadSignal(powerTraderComp);

            this.SetArmed(false);
            if (p != null)
            {
                this.DamagePawn(p);
            }
            else
            {
                this.EmptyActivation();
            }

            if (this.GetAutoRearm())
            {
                if (this.powerTraderComp != null)
                {
                    this.rearmTickIntervalInt = Mathf.RoundToInt(this.GetStatValue(CT_StatDefOf.RearmInterval) * 60f);
                }
                else
                {
                    base.Map.designationManager.AddDesignation(new Designation(this, DesignationDefOf.RearmTrap));
                }
            }
        }
        protected virtual void EmptyActivation()
        {

        }
        protected virtual void DamagePawn(Pawn p)
        {
            BodyPartHeight height = (Rand.Value >= 0.666f) ? BodyPartHeight.Middle : BodyPartHeight.Top;
            int num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage, true) * Building_TrapElectricRearmable.TrapDamageFactor.RandomInRange);
            int randomInRange = Building_TrapElectricRearmable.DamageCount.RandomInRange;
            for (int i = 0; i < randomInRange; i++)
            {
                if (num <= 0)
                {
                    break;
                }
                int num2 = Mathf.Max(1, Mathf.RoundToInt(Rand.Value * (float)num));
                num -= num2;
                DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, num2, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
                dinfo.SetBodyRegion(height, BodyPartDepth.Outside);
                p.TakeDamage(dinfo);
            }
        }
        protected void SetArmed(bool value)
        {
            /*FieldInfo armedInfo = typeof(Building_TrapRearmable).GetField("armedInt", BindingFlags.Instance | BindingFlags.NonPublic);
            armedInfo.SetValue(this, value);*/
            
            AccessTools.Field(typeof(Building_TrapRearmable), "armedInt").SetValue(this, value);
        }
        protected bool GetAutoRearm()
        {
            /*FieldInfo autoInfo = typeof(Building_TrapRearmable).GetField("autoRearm", BindingFlags.Instance | BindingFlags.NonPublic);
            return (bool)autoInfo.GetValue(this);*/
            
            return (bool)AccessTools.Field(typeof(Building_TrapRearmable), "autoRearm").GetValue(this);
        }
    }
}
