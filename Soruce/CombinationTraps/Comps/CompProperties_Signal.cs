using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace CombinationTraps
{
    public enum TransmissionStance : int
    {
        Undefined,
        ShutDown,
        OnlyReceive,
        OnlyTransmit,
        Any
    }

    public class CompProperties_Signal : CompProperties
    {
        public TransmissionStance defaultTransmissionStance = TransmissionStance.Undefined;
        public List<TransmissionStance> transmissionStanceFilter = new List<TransmissionStance>();

        public CompProperties_Signal()
        {
            base.compClass = typeof(CompSignal);
        }
    }

    static class TransmissionStance_Expansion
    {
        public static string Label(this TransmissionStance value)
        {
            switch (value)
            {
                case TransmissionStance.Any:
                    return "LabelTransmissionStance_Any".Translate();
                case TransmissionStance.ShutDown:
                    return "LabelTransmissionStance_ShutDown".Translate();
                case TransmissionStance.OnlyReceive:
                    return "LabelTransmissionStance_OnlyReceive".Translate();
                case TransmissionStance.OnlyTransmit:
                    return "LabelTransmissionStance_OnlyTransmit".Translate();
                default:
                    return "LabelTransmissionStance_Undefined";
            }
        }
        public static string Desc(this TransmissionStance value)
        {
            switch (value)
            {
                case TransmissionStance.Any:
                    return "DescTransmissionStance_Any".Translate();
                case TransmissionStance.ShutDown:
                    return "DescTransmissionStance_ShutDown".Translate();
                case TransmissionStance.OnlyReceive:
                    return "DescTransmissionStance_OnlyReceive".Translate();
                case TransmissionStance.OnlyTransmit:
                    return "DescTransmissionStance_OnlyTransmit".Translate();
                default:
                    return "DescTransmissionStance_Undefined";
            }
        }
        public static Texture2D Texture(this TransmissionStance value)
        {
            switch (value)
            {
                case TransmissionStance.Any:
                    return CT_TexCommandOf.TransStance_Any;
                case TransmissionStance.ShutDown:
                    return CT_TexCommandOf.TransStance_ShutDown;
                case TransmissionStance.OnlyReceive:
                    return CT_TexCommandOf.TransStance_OnlyReceive;
                case TransmissionStance.OnlyTransmit:
                    return CT_TexCommandOf.TransStance_OnlyTransmit;
                default:
                    return BaseContent.BadTex;
            }
        }
    }
    
}
