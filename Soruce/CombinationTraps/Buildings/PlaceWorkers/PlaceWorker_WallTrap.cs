using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace CombinationTraps
{
    class PlaceWorker_WallTrap : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot)
        {
            int range = Mathf.RoundToInt(def.GetStatValueAbstract(CT_StatDefOf.TrapRange));
            List<IntVec3> drawCells = new List<IntVec3>();
            for (int i = 0; i < range; i++)
            {
                drawCells.Add(center + rot.FacingCell * i);
            }
            GenDraw.DrawFieldEdges(drawCells, Color.white);
        }
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Thing thingToIgnore = null)
        {
            return base.AllowsPlacing(checkingDef, loc, rot, thingToIgnore);
        }
    }
}
