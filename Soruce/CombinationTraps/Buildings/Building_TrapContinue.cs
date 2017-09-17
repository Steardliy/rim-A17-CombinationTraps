﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using System.Reflection;

namespace CombinationTraps
{
    public abstract class Building_TrapContinue : Building_TrapRearmable
    {
        protected abstract DamageDef dDef { get; }
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
        }
        protected override void SpringSub(Pawn p)
        {
            FieldInfo armedInfo = typeof(Building_TrapRearmable).GetField("armedInt", BindingFlags.SetField | BindingFlags.Instance | BindingFlags.NonPublic);
            armedInfo.SetValue(this, false);
            if (p != null)
            {
                this.DamagePawn(p);
            }
            FieldInfo autoInfo = typeof(Building_TrapRearmable).GetField("autoRearm", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            if ((bool)autoInfo.GetValue(this))
            {
                base.Map.designationManager.AddDesignation(new Designation(this, DesignationDefOf.RearmTrap));
            }
        }
        protected void DamagePawn(Pawn p)
        {
            BodyPartHeight height = (Rand.Value >= 0.666f) ? BodyPartHeight.Middle : BodyPartHeight.Top;
            int num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage, true));
            int num2 = Mathf.Max(1, Mathf.RoundToInt(Rand.Value * (float)num));
            float angle = base.Rotation.AsAngle;
            DamageInfo dinfo = new DamageInfo(this.dDef, num2, angle, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            dinfo.SetBodyRegion(height, BodyPartDepth.Outside);
            p.TakeDamage(dinfo);
        }
    }
}