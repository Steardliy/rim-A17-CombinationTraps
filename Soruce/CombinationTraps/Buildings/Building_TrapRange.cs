using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using System.Reflection;

namespace CombinationTraps
{
    public abstract class Building_TrapRange : Building_TrapRearmable
    {
        private int inspectRange;
        protected abstract DamageDef dDef { get; }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.inspectRange = Mathf.RoundToInt(this.GetStatValue(CT_StatDefOf.TrapRange));
        }

        public override void Tick()
        {
            base.Tick();
            if (this.Armed)
            {
                /*FieldInfo touchInfo = typeof(Building_Trap).GetField("touchingPawns", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
                List<Pawn> pList = (List<Pawn>)touchInfo.GetValue(this);*/

                for(int i = 1; i < this.inspectRange; i++)
                {
                    IntVec3 pos = base.Position + base.Rotation.FacingCell * i;
                    this.PawnsAt(pos);
                    //pList.AddRange(this.PawnsAt(pos));
                }
            }
        }
        protected void checkSpring(Pawn p)
        {
            if (Rand.Value < this.SpringChance(p))
            {
                base.Spring(p);
                if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
                {
                    Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(new object[]
                    {
                        p.NameStringShort
                    }), "LetterFriendlyTrapSprung".Translate(new object[]
                    {
                        p.NameStringShort
                    }), LetterDefOf.BadNonUrgent, new TargetInfo(base.Position, base.Map, false), null);
                }
            }
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
        protected virtual void DamagePawn(Pawn p)
        {
            BodyPartHeight height = (Rand.Value >= 0.666f) ? BodyPartHeight.Middle : BodyPartHeight.Top;
            int num = Mathf.RoundToInt(this.GetStatValue(StatDefOf.TrapMeleeDamage, true));
            float angle = this.ActAngle();
            DamageInfo dinfo = new DamageInfo(this.dDef, num, angle, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            dinfo.SetBodyRegion(height, BodyPartDepth.Outside);

            p.TakeDamage(dinfo);
        }
        protected virtual float ActAngle()
        {
            return base.Rotation.AsAngle;
        }
        private void PawnsAt(IntVec3 pos)
        {
            List<Thing> thingList = pos.GetThingList(base.Map);
            for (int i = 0; i < thingList.Count; i++)
            {
                Pawn pawn = thingList[i] as Pawn;
                if (pawn != null)
                {
                    this.checkSpring(pawn);
                   // yield return pawn;
                }
            }
        }
    }
}
