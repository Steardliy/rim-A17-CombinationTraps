using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace CombinationTraps
{
    public static class Override_PreDrawPosCalculation
    {
        public const float default_SpringTightness = 0.09f;
        public const float springTightnessCoefficient = 0.4f;

        private const float springTightnessRangeMin = 0.05f;
        private const float springTightnessRangeMax = 0.3f;

        private static float springTightnessInt = default_SpringTightness;
        public static float SpringTightness
        {
            get { return springTightnessInt; }
            set
            {
                springTightnessInt = Mathf.Clamp(value, springTightnessRangeMin, springTightnessRangeMax);
            }
        }
        public static void Default()
        {
            springTightnessInt = default_SpringTightness;
        }
    }
}
