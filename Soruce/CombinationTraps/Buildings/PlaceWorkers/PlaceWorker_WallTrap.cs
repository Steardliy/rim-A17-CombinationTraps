using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace CombinationTraps
{
    class PlaceWorker_WallTrap : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot)
        {
            int range = Mathf.RoundToInt(def.GetStatValueAbstract(CT_StatDefOf.TrapRange));
            List<IntVec3> drawCells = new List<IntVec3>();
            for (int i = 1; i < range; i++)
            {
                IntVec3 pos = center + rot.FacingCell * i;
                if (!pos.CanBeSeenOver(base.Map))
                {
                    break;
                }
                drawCells.Add(pos);
            }
            GenDraw.DrawFieldEdges(drawCells, Color.white);
        }
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Thing thingToIgnore = null)
        {
            IntVec3 pos = loc + rot.Opposite.FacingCell;
            List<Thing> things = pos.GetThingList(base.Map);
            for(int i = 0; i < things.Count; i++)
            {
                if (things[i].def == ThingDefOf.Wall)
                {
                    return true;
                }
                BuildableDef curBuildDef = GenConstruct.BuiltDefOf(things[i].def);
                if (curBuildDef != null && curBuildDef is ThingDef)
                {
                    ThingDef curDef = (ThingDef)curBuildDef;
                    if (curDef == ThingDefOf.Wall)
                    {
                        return true;
                    }
                }
            }
            return "MustBeAdjacentToTheWall".Translate();
        }
    }
}
